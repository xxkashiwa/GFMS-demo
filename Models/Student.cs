using Microsoft.UI.Xaml;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    /// <summary>
    /// Represents a student in the Graduate File Management System
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Unique identifier for the student
        /// </summary>
        public string StudentId { get; set; } = string.Empty;

        /// <summary>
        /// Full name of the student
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gender of the student
        /// </summary>
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Date of graduation
        /// </summary>
        public DateTime GraduationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Academic scores by semester, each semester can have multiple subjects
        /// </summary>
        public Dictionary<string, List<Score>> Scores { get; set; } = new Dictionary<string, List<Score>>();

        /// <summary>
        /// List of rewards and punishments associated with the student
        /// </summary>
        public List<RewardAndPunishment> RewardsAndPunishments { get; set; } = [];

        /// <summary>
        /// List of files associated with the student
        /// </summary>
        public List<StudentFile> Files { get; set; } = [];
    }

    public class Score
    {
        public string Subject { get; set; } = string.Empty;
        public double ScoreValue { get; set; } = 0.0;
    }

    /// <summary>
    /// Represents a score item for display in the scores table
    /// </summary>
    public class ScoreDisplayItem
    {
        public string Semester { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public double ScoreValue { get; set; } = 0.0;
    }

    /// <summary>
    /// Represents semester summary for average score calculation
    /// </summary>
    public class SemesterSummary
    {
        public string Semester { get; set; } = string.Empty;
        public double AverageScore { get; set; } = 0.0;
        public int SubjectCount { get; set; } = 0;
    }

    /// <summary>
    /// Represents a reward or punishment record for a student
    /// </summary>
    public class RewardAndPunishment
    {
        /// <summary>
        /// Type of record: "Reward" or "Punishment"
        /// </summary>
        public RecordType Type { get; set; }

        /// <summary>
        /// Detailed description of the reward or punishment
        /// </summary>
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// Date when the reward or punishment was issued
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Represents a reward or punishment item for display in the table
    /// </summary>
    public class RewardAndPunishmentDisplayItem
    {
        public string Type { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public string TypeDisplayName => Type == "Reward" ? "奖励" : "惩罚";
    }

    /// <summary>
    /// 用于档案管理页面显示的学生信息，包含档案状态
    /// </summary>
    public class StudentFileManagementItem
    {
        /// <summary>
        /// 学生信息
        /// </summary>
        public Student Student { get; set; } = new Student();

        /// <summary>
        /// 毕业登记表状态
        /// </summary>
        public string GraduationFormStatus { get; set; } = "未上传";

        /// <summary>
        /// 体检表状态
        /// </summary>
        public string MedicalExamStatus { get; set; } = "未上传";

        /// <summary>
        /// 实习报告状态
        /// </summary>
        public string InternshipReportStatus { get; set; } = "未上传";

        /// <summary>
        /// 组合档案状态显示，格式为：状态1/状态2/状态3
        /// </summary>
        public string CombinedFileStatus => $"{GraduationFormStatus}/{MedicalExamStatus}/{InternshipReportStatus}";

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentId => Student.StudentId;

        /// <summary>
        /// 获取指定文件类型的状态
        /// </summary>
        /// <param name="fileType">文件类型</param>
        /// <returns>状态字符串</returns>
        public string GetFileStatus(string fileType)
        {
            var file = Student.Files.FirstOrDefault(f => f.FileType == fileType);
            if (file == null)
            {
                return "未上传";
            }

            return file.State switch
            {
                FileState.已上传 => "已上传",
                FileState.已审核 => "已审核",
                FileState.驳回 => "驳回",
                _ => "未上传"
            };
        }

        /// <summary>
        /// 更新档案状态
        /// </summary>
        public void UpdateFileStatuses()
        {
            GraduationFormStatus = GetFileStatus("毕业登记表");
            MedicalExamStatus = GetFileStatus("体检表");
            InternshipReportStatus = GetFileStatus("实习报告");
        }
    }

    /// <summary>
    /// Defines the types of student records
    /// </summary>
    public enum RecordType
    {
        /// <summary>
        /// Represents a positive recognition or award
        /// </summary>
        Reward,

        /// <summary>
        /// Represents a disciplinary action or penalty
        /// </summary>
        Punishment
    }
}
