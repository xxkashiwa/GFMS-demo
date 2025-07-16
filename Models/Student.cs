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
