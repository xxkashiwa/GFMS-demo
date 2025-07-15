using ClosedXML.Excel;
using GFMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace GFMS.Services
{
    /// <summary>
    /// Excel导入服务
    /// </summary>
    public static class ExcelImportService
    {
        /// <summary>
        /// 从Excel文件导入学生信息
        /// </summary>
        public static async Task<List<Student>> ImportStudentsFromExcelAsync(StorageFile file)
        {
            var students = new List<Student>();
            
            try
            {
                var stream = await file.OpenStreamForReadAsync();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                
                // 假设第一行是标题行
                var lastRowUsed = worksheet.LastRowUsed()?.RowNumber() ?? 1;
                
                for (int row = 2; row <= lastRowUsed; row++)
                {
                    var student = new Student
                    {
                        StudentId = worksheet.Cell(row, 1).GetString().Trim(),
                        Name = worksheet.Cell(row, 2).GetString().Trim(),
                        Gender = worksheet.Cell(row, 3).GetString().Trim(),
                        Major = worksheet.Cell(row, 4).GetString().Trim(),
                        GraduationYear = TryParseInt(worksheet.Cell(row, 5).GetString())
                    };
                    
                    // 验证必填字段
                    if (!string.IsNullOrWhiteSpace(student.StudentId) && 
                        !string.IsNullOrWhiteSpace(student.Name))
                    {
                        students.Add(student);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"读取Excel文件时发生错误: {ex.Message}", ex);
            }
            
            return students;
        }
        
        /// <summary>
        /// 从Excel文件导入成绩信息
        /// </summary>
        public static async Task<List<StudentScore>> ImportGradesFromExcelAsync(StorageFile file)
        {
            var scores = new List<StudentScore>();
            
            try
            {
                var stream = await file.OpenStreamForReadAsync();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                
                var lastRowUsed = worksheet.LastRowUsed()?.RowNumber() ?? 1;
                var lastColumnUsed = worksheet.LastColumnUsed()?.ColumnNumber() ?? 1;
                
                // 读取表头（科目名称）
                var subjects = new List<string>();
                for (int col = 3; col <= lastColumnUsed; col++) // 假设前两列是学号和姓名
                {
                    var subject = worksheet.Cell(1, col).GetString().Trim();
                    if (!string.IsNullOrWhiteSpace(subject))
                    {
                        subjects.Add(subject);
                    }
                }
                
                for (int row = 2; row <= lastRowUsed; row++)
                {
                    var studentId = worksheet.Cell(row, 1).GetString().Trim();
                    var term = worksheet.Cell(row, 2).GetString().Trim();
                    
                    if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(term))
                        continue;
                    
                    var gradeDict = new Dictionary<string, string>();
                    
                    // 读取各科成绩
                    for (int col = 3; col <= Math.Min(lastColumnUsed, subjects.Count + 2); col++)
                    {
                        var subjectIndex = col - 3;
                        if (subjectIndex < subjects.Count)
                        {
                            var score = worksheet.Cell(row, col).GetString().Trim();
                            if (!string.IsNullOrWhiteSpace(score))
                            {
                                gradeDict[subjects[subjectIndex]] = score;
                            }
                        }
                    }
                    
                    if (gradeDict.Any())
                    {
                        var studentScore = new StudentScore
                        {
                            StudentUuid = studentId,
                            Term = term,
                            Score = JsonSerializer.Serialize(gradeDict, new JsonSerializerOptions 
                            { 
                                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
                            })
                        };
                        
                        scores.Add(studentScore);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"读取成绩Excel文件时发生错误: {ex.Message}", ex);
            }
            
            return scores;
        }
        
        /// <summary>
        /// 从Excel文件导入奖惩记录
        /// </summary>
        public static async Task<List<StudentRecord>> ImportRecordsFromExcelAsync(StorageFile file)
        {
            var records = new List<StudentRecord>();
            
            try
            {
                var stream = await file.OpenStreamForReadAsync();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                
                var lastRowUsed = worksheet.LastRowUsed()?.RowNumber() ?? 1;
                
                for (int row = 2; row <= lastRowUsed; row++)
                {
                    var studentIdStr = worksheet.Cell(row, 1).GetString().Trim();
                    var recordType = worksheet.Cell(row, 2).GetString().Trim();
                    var description = worksheet.Cell(row, 3).GetString().Trim();
                    
                    if (string.IsNullOrWhiteSpace(studentIdStr) || string.IsNullOrWhiteSpace(recordType))
                        continue;
                    
                    // 这里需要根据StudentId查找对应的Student.Id
                    // 暂时使用StudentId作为占位符，实际使用时需要数据库查询
                    var record = new StudentRecord
                    {
                        StudentId = 0, // 需要通过StudentId查询获得真实的Student.Id
                        RecordType = MapRecordType(recordType),
                        Description = description
                    };
                    
                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"读取奖惩记录Excel文件时发生错误: {ex.Message}", ex);
            }
            
            return records;
        }
        
        /// <summary>
        /// 尝试解析整数
        /// </summary>
        private static int? TryParseInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return null;
        }
        
        /// <summary>
        /// 映射记录类型
        /// </summary>
        private static string MapRecordType(string recordType)
        {
            return recordType switch
            {
                "奖励" => "Award",
                "处分" => "Punishment", 
                "成绩" => "Grade",
                _ => recordType
            };
        }
    }
}