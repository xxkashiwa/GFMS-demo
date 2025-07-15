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
    /// Excel������ģ�����ɷ���
    /// </summary>
    public static class ExcelExportService
    {
        /// <summary>
        /// ����ѧ����Ϣ��Excel
        /// </summary>
        public static async Task ExportStudentsToExcelAsync(List<Student> students, StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("ѧ����Ϣ");
                
                // ���ñ�����
                worksheet.Cell(1, 1).Value = "ѧ��";
                worksheet.Cell(1, 2).Value = "����";
                worksheet.Cell(1, 3).Value = "�Ա�";
                worksheet.Cell(1, 4).Value = "רҵ";
                worksheet.Cell(1, 5).Value = "��ҵ���";
                
                // ���ñ�������ʽ
                var titleRange = worksheet.Range(1, 1, 1, 5);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                
                // �������
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
                
                // �Զ������п�
                worksheet.Columns().AdjustToContents();
                
                // �����ļ�
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"����ѧ����Ϣ��Excelʱ��������: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// �����ɼ���Ϣ��Excel
        /// </summary>
        public static async Task ExportGradesToExcelAsync(List<StudentScore> scores, StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("�ɼ���Ϣ");
                
                // �ռ����п�Ŀ
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
                
                // ���ñ�����
                worksheet.Cell(1, 1).Value = "ѧ��";
                worksheet.Cell(1, 2).Value = "ѧ��";
                for (int i = 0; i < subjectList.Count; i++)
                {
                    worksheet.Cell(1, i + 3).Value = subjectList[i];
                }
                
                // ���ñ�������ʽ
                var titleRange = worksheet.Range(1, 1, 1, subjectList.Count + 2);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                
                // �������
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
                
                // �Զ������п�
                worksheet.Columns().AdjustToContents();
                
                // �����ļ�
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"�����ɼ���Ϣ��Excelʱ��������: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// ����ѧ����Ϣ����ģ��
        /// </summary>
        public static async Task GenerateStudentTemplateAsync(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("ѧ����Ϣģ��");
                
                // ���ñ�����
                worksheet.Cell(1, 1).Value = "ѧ��";
                worksheet.Cell(1, 2).Value = "����";
                worksheet.Cell(1, 3).Value = "�Ա�";
                worksheet.Cell(1, 4).Value = "רҵ";
                worksheet.Cell(1, 5).Value = "��ҵ���";
                
                // ���ñ�������ʽ
                var titleRange = worksheet.Range(1, 1, 1, 5);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                
                // ���ʾ������
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "����";
                worksheet.Cell(2, 3).Value = "M";
                worksheet.Cell(2, 4).Value = "�������ѧ�뼼��";
                worksheet.Cell(2, 5).Value = "2024";
                
                worksheet.Cell(3, 1).Value = "2024002";
                worksheet.Cell(3, 2).Value = "����";
                worksheet.Cell(3, 3).Value = "F";
                worksheet.Cell(3, 4).Value = "�������";
                worksheet.Cell(3, 5).Value = "2024";
                
                // ���ע��
                var noteSheet = workbook.Worksheets.Add("��д˵��");
                noteSheet.Cell(1, 1).Value = "��д˵����";
                noteSheet.Cell(2, 1).Value = "1. ѧ�ţ����ѧ��Ψһ��ʶ";
                noteSheet.Cell(3, 1).Value = "2. ����������";
                noteSheet.Cell(4, 1).Value = "3. �Ա���д M���У��� F��Ů��";
                noteSheet.Cell(5, 1).Value = "4. רҵ��ѡ��";
                noteSheet.Cell(6, 1).Value = "5. ��ҵ��ݣ�ѡ���ʽΪ��λ�������";
                
                noteSheet.Range(1, 1, 6, 1).Style.Font.FontSize = 12;
                noteSheet.Cell(1, 1).Style.Font.Bold = true;
                
                // �Զ������п�
                worksheet.Columns().AdjustToContents();
                noteSheet.Columns().AdjustToContents();
                
                // �����ļ�
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"����ѧ����Ϣģ��ʱ��������: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// ���ɳɼ�����ģ��
        /// </summary>
        public static async Task GenerateGradeTemplateAsync(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("�ɼ�����ģ��");
                
                // ���ñ�����
                worksheet.Cell(1, 1).Value = "ѧ��";
                worksheet.Cell(1, 2).Value = "ѧ��";
                worksheet.Cell(1, 3).Value = "�ߵ���ѧ";
                worksheet.Cell(1, 4).Value = "Ӣ��";
                worksheet.Cell(1, 5).Value = "���������";
                worksheet.Cell(1, 6).Value = "�������";
                
                // ���ñ�������ʽ
                var titleRange = worksheet.Range(1, 1, 1, 6);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightGreen;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                
                // ���ʾ������
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "2024-2025����";
                worksheet.Cell(2, 3).Value = "88";
                worksheet.Cell(2, 4).Value = "92";
                worksheet.Cell(2, 5).Value = "85";
                worksheet.Cell(2, 6).Value = "90";
                
                // ���ע��
                var noteSheet = workbook.Worksheets.Add("��д˵��");
                noteSheet.Cell(1, 1).Value = "�ɼ�����˵����";
                noteSheet.Cell(2, 1).Value = "1. ѧ�ţ������ѧ����Ϣ�е�ѧ�Ŷ�Ӧ";
                noteSheet.Cell(3, 1).Value = "2. ѧ�ڣ������ʽ�� '2024-2025����'";
                noteSheet.Cell(4, 1).Value = "3. ���Ƴɼ����ɸ���ʵ�ʿ�Ŀ�����б���";
                noteSheet.Cell(5, 1).Value = "4. �ɼ�����д���ַ������հױ�ʾ�ÿ�Ŀ�޳ɼ�";
                noteSheet.Cell(6, 1).Value = "5. ������Ӹ����Ŀ��";
                
                noteSheet.Range(1, 1, 6, 1).Style.Font.FontSize = 12;
                noteSheet.Cell(1, 1).Style.Font.Bold = true;
                
                // �Զ������п�
                worksheet.Columns().AdjustToContents();
                noteSheet.Columns().AdjustToContents();
                
                // �����ļ�
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"���ɳɼ�����ģ��ʱ��������: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// ���ɽ��ͼ�¼����ģ��
        /// </summary>
        public static async Task GenerateRecordTemplateAsync(StorageFile file)
        {
            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("���ͼ�¼ģ��");
                
                // ���ñ�����
                worksheet.Cell(1, 1).Value = "ѧ��";
                worksheet.Cell(1, 2).Value = "��¼����";
                worksheet.Cell(1, 3).Value = "��ϸ����";
                
                // ���ñ�������ʽ
                var titleRange = worksheet.Range(1, 1, 1, 3);
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Fill.BackgroundColor = XLColor.LightYellow;
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                
                // ���ʾ������
                worksheet.Cell(2, 1).Value = "2024001";
                worksheet.Cell(2, 2).Value = "����";
                worksheet.Cell(2, 3).Value = "����ѧ����У����2024���";
                
                worksheet.Cell(3, 1).Value = "2024002";
                worksheet.Cell(3, 2).Value = "����";
                worksheet.Cell(3, 3).Value = "Υ�ʹ��֣�Ժ����2024�괺��ѧ��";
                
                // ���ע��
                var noteSheet = workbook.Worksheets.Add("��д˵��");
                noteSheet.Cell(1, 1).Value = "���ͼ�¼����˵����";
                noteSheet.Cell(2, 1).Value = "1. ѧ�ţ������ѧ����Ϣ�е�ѧ�Ŷ�Ӧ";
                noteSheet.Cell(3, 1).Value = "2. ��¼���ͣ������д '����' �� '����'";
                noteSheet.Cell(4, 1).Value = "3. ��ϸ�����������������ơ�����ʱ�����Ϣ";
                
                noteSheet.Range(1, 1, 4, 1).Style.Font.FontSize = 12;
                noteSheet.Cell(1, 1).Style.Font.Bold = true;
                
                // �Զ������п�
                worksheet.Columns().AdjustToContents();
                noteSheet.Columns().AdjustToContents();
                
                // �����ļ�
                using var stream = await file.OpenStreamForWriteAsync();
                workbook.SaveAs(stream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"���ɽ��ͼ�¼ģ��ʱ��������: {ex.Message}", ex);
            }
        }
    }
}