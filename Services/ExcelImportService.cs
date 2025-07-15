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
    /// Excel�������
    /// </summary>
    public static class ExcelImportService
    {
        /// <summary>
        /// ��Excel�ļ�����ѧ����Ϣ
        /// </summary>
        public static async Task<List<Student>> ImportStudentsFromExcelAsync(StorageFile file)
        {
            var students = new List<Student>();
            
            try
            {
                var stream = await file.OpenStreamForReadAsync();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                
                // �����һ���Ǳ�����
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
                    
                    // ��֤�����ֶ�
                    if (!string.IsNullOrWhiteSpace(student.StudentId) && 
                        !string.IsNullOrWhiteSpace(student.Name))
                    {
                        students.Add(student);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"��ȡExcel�ļ�ʱ��������: {ex.Message}", ex);
            }
            
            return students;
        }
        
        /// <summary>
        /// ��Excel�ļ�����ɼ���Ϣ
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
                
                // ��ȡ��ͷ����Ŀ���ƣ�
                var subjects = new List<string>();
                for (int col = 3; col <= lastColumnUsed; col++) // ����ǰ������ѧ�ź�����
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
                    
                    // ��ȡ���Ƴɼ�
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
                throw new InvalidOperationException($"��ȡ�ɼ�Excel�ļ�ʱ��������: {ex.Message}", ex);
            }
            
            return scores;
        }
        
        /// <summary>
        /// ��Excel�ļ����뽱�ͼ�¼
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
                    
                    // ������Ҫ����StudentId���Ҷ�Ӧ��Student.Id
                    // ��ʱʹ��StudentId��Ϊռλ����ʵ��ʹ��ʱ��Ҫ���ݿ��ѯ
                    var record = new StudentRecord
                    {
                        StudentId = 0, // ��Ҫͨ��StudentId��ѯ�����ʵ��Student.Id
                        RecordType = MapRecordType(recordType),
                        Description = description
                    };
                    
                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"��ȡ���ͼ�¼Excel�ļ�ʱ��������: {ex.Message}", ex);
            }
            
            return records;
        }
        
        /// <summary>
        /// ���Խ�������
        /// </summary>
        private static int? TryParseInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return null;
        }
        
        /// <summary>
        /// ӳ���¼����
        /// </summary>
        private static string MapRecordType(string recordType)
        {
            return recordType switch
            {
                "����" => "Award",
                "����" => "Punishment", 
                "�ɼ�" => "Grade",
                _ => recordType
            };
        }
    }
}