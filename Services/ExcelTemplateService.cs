using ClosedXML.Excel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace GFMS.Services
{
    public static class ExcelTemplateService
    {
        /// <summary>
        /// 生成学生信息导入模板
        /// </summary>
        public static async Task GenerateStudentTemplate(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("学生信息模板");

                // 设置标题行
                worksheet.Cell(1, 1).Value = "学号*";
                worksheet.Cell(1, 2).Value = "姓名*";
                worksheet.Cell(1, 3).Value = "性别";
                worksheet.Cell(1, 4).Value = "专业";
                worksheet.Cell(1, 5).Value = "班级";
                worksheet.Cell(1, 6).Value = "身份证号";
                worksheet.Cell(1, 7).Value = "联系电话";
                worksheet.Cell(1, 8).Value = "邮箱地址";
                worksheet.Cell(1, 9).Value = "家庭住址";
                worksheet.Cell(1, 10).Value = "家长姓名";
                worksheet.Cell(1, 11).Value = "家长联系方式";
                worksheet.Cell(1, 12).Value = "出生日期(yyyy-MM-dd)";

                // 设置样式
                var titleRange = worksheet.Range(1, 1, 1, 12);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // 添加示例数据
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "张三";
                worksheet.Cell(2, 3).Value = "男";
                worksheet.Cell(2, 4).Value = "计算机科学与技术";
                worksheet.Cell(2, 5).Value = "计科2401";
                worksheet.Cell(2, 6).Value = "110101199901010001";
                worksheet.Cell(2, 7).Value = "13888888888";
                worksheet.Cell(2, 8).Value = "zhangsan@example.com";
                worksheet.Cell(2, 9).Value = "北京市朝阳区";
                worksheet.Cell(2, 10).Value = "张父";
                worksheet.Cell(2, 11).Value = "13999999999";
                worksheet.Cell(2, 12).Value = "1999-01-01";

                worksheet.Columns().AdjustToContents();

                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 生成成绩导入模板
        /// </summary>
        public static async Task GenerateGradeTemplate(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("成绩信息模板");

                // 设置标题行
                worksheet.Cell(1, 1).Value = "学号*";
                worksheet.Cell(1, 2).Value = "学生姓名*";
                worksheet.Cell(1, 3).Value = "课程名称*";
                worksheet.Cell(1, 4).Value = "成绩类型";
                worksheet.Cell(1, 5).Value = "学期";
                worksheet.Cell(1, 6).Value = "学分";
                worksheet.Cell(1, 7).Value = "成绩";
                worksheet.Cell(1, 8).Value = "备注";

                // 设置样式
                var titleRange = worksheet.Range(1, 1, 1, 8);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // 添加示例数据
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "张三";
                worksheet.Cell(2, 3).Value = "高等数学";
                worksheet.Cell(2, 4).Value = "期末成绩";
                worksheet.Cell(2, 5).Value = "2024-2025春季";
                worksheet.Cell(2, 6).Value = "4";
                worksheet.Cell(2, 7).Value = "85";
                worksheet.Cell(2, 8).Value = "";

                worksheet.Columns().AdjustToContents();

                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 生成奖惩记录导入模板
        /// </summary>
        public static async Task GenerateRecordTemplate(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("奖惩记录模板");

                // 设置标题行
                worksheet.Cell(1, 1).Value = "学号*";
                worksheet.Cell(1, 2).Value = "学生姓名*";
                worksheet.Cell(1, 3).Value = "记录类型*";
                worksheet.Cell(1, 4).Value = "奖惩名称*";
                worksheet.Cell(1, 5).Value = "级别";
                worksheet.Cell(1, 6).Value = "颁发机构";
                worksheet.Cell(1, 7).Value = "详细描述";
                worksheet.Cell(1, 8).Value = "获得时间(yyyy-MM-dd)";

                // 设置样式
                var titleRange = worksheet.Range(1, 1, 1, 8);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // 添加示例数据
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "张三";
                worksheet.Cell(2, 3).Value = "奖励";
                worksheet.Cell(2, 4).Value = "三好学生";
                worksheet.Cell(2, 5).Value = "校级";
                worksheet.Cell(2, 6).Value = "XX大学";
                worksheet.Cell(2, 7).Value = "品学兼优，表现突出";
                worksheet.Cell(2, 8).Value = "2024-12-01";

                worksheet.Columns().AdjustToContents();

                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}