using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFMS.Models
{
    public class StudentScore
    {
        public int Id { get; set; }
        
        [Required]
        public string StudentUuid { get; set; } = string.Empty;
        
        [Required]
        public string Term { get; set; } = string.Empty;
        
        [Required]
        public string Score { get; set; } = string.Empty; // JSON ��ʽ�洢���Ƴɼ�
        
        [ForeignKey("StudentUuid")]
        public Student? Student { get; set; }
    }
}