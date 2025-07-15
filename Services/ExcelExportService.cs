using ClosedXML.Excel;
using GFMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace GFMS.Services
{
    /// <summary>
    /// Excel导出和模板生成服务
    /// </summary>
    public static class ExcelExportService
    {
        /// <summary>
        /// 导出学生信息到Excel
        /// </summary>
        public static async Task ExportStudentsToExcelAsync(List<Student> students, StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("学生信息");
                
                // 设置标题行
                worksheet.Cell(1, 1).Value = "学号";
                worksheet.Cell(1, 2).Value = "姓名";
                worksheet.Cell(1, 3).Value = "性别";
                worksheet.Cell(1, 4).Value = "专业";
                worksheet.Cell(1, 5).Value = "毕业年份";
                
                // 设置标题行样式
                var titleRange = worksheet.Range(1, 1, 1, 5);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                
                // 填充数据
                for (int i = 0; i < students.Count; i++)
                {
                    var student = students[i];
                    var row = i + 2;
                    
                    worksheet.Cell(row, 1).Value = student.StudentId;
                    worksheet.Cell(row, 2).Value = student.Name;
                    worksheet.Cell(row, 3).Value = student.Gender;
                    worksheet.Cell(row, 4).Value = student.Major;
                    worksheet.Cell(row, 5).Value = student.GraduationYear?.ToString() ?? "";
                }
                
                // 自动调整列宽
                worksheet.Columns().AdjustToContents();
                
                // 保存文件
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"导出学生信息到Excel时发生错误: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 导出成绩信息到Excel
        /// </summary>
        public static async Task ExportGradesToExcelAsync(List<StudentScore> scores, StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("成绩信息");
                
                // 收集所有科目
                var allSubjects = new HashSet<string>();
                foreach (var score in scores)
                {
                    if (!string.IsNullOrWhiteSpace(score.Score))
                    {
                        try
                        {
                            var gradeDict = JsonSerializer.Deserialize<Dictionary<string, string>>(score.Score);
                            if (gradeDict != null)
                            {
                                foreach (var subject in gradeDict.Keys)
                                {
                                    allSubjects.Add(subject);
                                }
                            }
                        }
                        catch { }
                    }
                }
                
                var subjectList = new List<string>(allSubjects);
                
                // 设置标题行
                worksheet.Cell(1, 1).Value = "学号";
                worksheet.Cell(1, 2).Value = "学期";
                for (int i = 0; i < subjectList.Count; i++)
                {
                    worksheet.Cell(1, i + 3).Value = subjectList[i];
                }
                
                // 设置标题行样式
                var titleRange = worksheet.Range(1, 1, 1, subjectList.Count + 2);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                
                // 填充数据
                for (int i = 0; i < scores.Count; i++)
                {
                    var score = scores[i];
                    var row = i + 2;
                    
                    worksheet.Cell(row, 1).Value = score.StudentUuid;
                    worksheet.Cell(row, 2).Value = score.Term;
                    
                    if (!string.IsNullOrWhiteSpace(score.Score))
                    {
                        try
                        {
                            var gradeDict = JsonSerializer.Deserialize<Dictionary<string, string>>(score.Score);
                            if (gradeDict != null)
                            {
                                for (int j = 0; j < subjectList.Count; j++)
                                {
                                    var subject = subjectList[j];
                                    if (gradeDict.ContainsKey(subject))
                                    {
                                        worksheet.Cell(row, j + 3).Value = gradeDict[subject];
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
                
                // 自动调整列宽
                worksheet.Columns().AdjustToContents();
                
                // 保存文件
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"导出成绩信息到Excel时发生错误: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 生成学生信息导入模板
        /// </summary>
        public static async Task GenerateStudentTemplateAsync(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("学生信息模板");
                
                // 设置标题行
                worksheet.Cell(1, 1).Value = "学号";
                worksheet.Cell(1, 2).Value = "姓名";
                worksheet.Cell(1, 3).Value = "性别";
                worksheet.Cell(1, 4).Value = "专业";
                worksheet.Cell(1, 5).Value = "毕业年份";
                
                // 设置标题行样式
                var titleRange = worksheet.Range(1, 1, 1, 5);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                
                // 添加示例数据
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "张三";
                worksheet.Cell(2, 3).Value = "M";
                worksheet.Cell(2, 4).Value = "计算机科学与技术";
                worksheet.Cell(2, 5).Value = "2024";
                
                worksheet.Cell(3, 1).Value = "2024002";
                worksheet.Cell(3, 2).Value = "李四";
                worksheet.Cell(3, 3).Value = "F";
                worksheet.Cell(3, 4).Value = "软件工程";
                worksheet.Cell(3, 5).Value = "2024";
                
                // 添加注释
                var noteSheet = workbook.Worksheets.Add("填写说明");
                noteSheet.Cell(1, 1).Value = "填写说明：";
                noteSheet.Cell(2, 1).Value = "1. 学号：必填，学生唯一标识";
                noteSheet.Cell(3, 1).Value = "2. 姓名：必填";
                noteSheet.Cell(4, 1).Value = "3. 性别：填写 M（男）或 F（女）";
                noteSheet.Cell(5, 1).Value = "4. 专业：选填";
                noteSheet.Cell(6, 1).Value = "5. 毕业年份：选填，格式为四位数字年份";
                
                noteSheet.Range(1, 1, 6, 1).Style.Font.FontSize = 12;
                noteSheet.Cell(1, 1).Style.Font.Bold = true;
                
                // 自动调整列宽
                worksheet.Columns().AdjustToContents();
                noteSheet.Columns().AdjustToContents();
                
                // 保存文件
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"生成学生信息模板时发生错误: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 生成成绩导入模板
        /// </summary>
        public static async Task GenerateGradeTemplateAsync(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("成绩导入模板");
                
                // 设置标题行
                worksheet.Cell(1, 1).Value = "学号";
                worksheet.Cell(1, 2).Value = "学期";
                worksheet.Cell(1, 3).Value = "高等数学";
                worksheet.Cell(1, 4).Value = "英语";
                worksheet.Cell(1, 5).Value = "计算机基础";
                worksheet.Cell(1, 6).Value = "程序设计";
                
                // 设置标题行样式
                var titleRange = worksheet.Range(1, 1, 1, 6);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                
                // 添加示例数据
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "2024-2025春季";
                worksheet.Cell(2, 3).Value = "88";
                worksheet.Cell(2, 4).Value = "92";
                worksheet.Cell(2, 5).Value = "85";
                worksheet.Cell(2, 6).Value = "90";
                
                // 添加注释
                var noteSheet = workbook.Worksheets.Add("填写说明");
                noteSheet.Cell(1, 1).Value = "成绩导入说明：";
                noteSheet.Cell(2, 1).Value = "1. 学号：必填，与学生信息中的学号对应";
                noteSheet.Cell(3, 1).Value = "2. 学期：必填，格式如 '2024-2025春季'";
                noteSheet.Cell(4, 1).Value = "3. 各科成绩：可根据实际科目调整列标题";
                noteSheet.Cell(5, 1).Value = "4. 成绩：填写数字分数，空白表示该科目无成绩";
                noteSheet.Cell(6, 1).Value = "5. 可以添加更多科目列";
                
                noteSheet.Range(1, 1, 6, 1).Style.Font.FontSize = 12;
                noteSheet.Cell(1, 1).Style.Font.Bold = true;
                
                // 自动调整列宽
                worksheet.Columns().AdjustToContents();
                noteSheet.Columns().AdjustToContents();
                
                // 保存文件
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"生成成绩导入模板时发生错误: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 生成奖惩记录导入模板
        /// </summary>
        public static async Task GenerateRecordTemplateAsync(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("奖惩记录模板");
                
                // 设置标题行
                worksheet.Cell(1, 1).Value = "学号";
                worksheet.Cell(1, 2).Value = "记录类型";
                worksheet.Cell(1, 3).Value = "详细描述";
                
                // 设置标题行样式
                var titleRange = worksheet.Range(1, 1, 1, 3);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                
                // 添加示例数据
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "奖励";
                worksheet.Cell(2, 3).Value = "三好学生，校级，2024年度";
                
                worksheet.Cell(3, 1).Value = "2024002";
                worksheet.Cell(3, 2).Value = "处分";
                worksheet.Cell(3, 3).Value = "违纪处分，院级，2024年春季学期";
                
                // 添加注释
                var noteSheet = workbook.Worksheets.Add("填写说明");
                noteSheet.Cell(1, 1).Value = "奖惩记录导入说明：";
                noteSheet.Cell(2, 1).Value = "1. 学号：必填，与学生信息中的学号对应";
                noteSheet.Cell(3, 1).Value = "2. 记录类型：必填，填写 '奖励' 或 '处分'";
                noteSheet.Cell(4, 1).Value = "3. 详细描述：包含奖惩名称、级别、时间等信息";
                
                noteSheet.Range(1, 1, 4, 1).Style.Font.FontSize = 12;
                noteSheet.Cell(1, 1).Style.Font.Bold = true;
                
                // 自动调整列宽
                worksheet.Columns().AdjustToContents();
                noteSheet.Columns().AdjustToContents();
                
                // 保存文件
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"生成奖惩记录模板时发生错误: {ex.Message}", ex);
            }
        }
    }
}