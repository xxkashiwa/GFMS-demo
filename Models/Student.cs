using System;
using System.ComponentModel.DataAnnotations;

namespace GFMS.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        [Required]
        public string StudentId { get; set; } = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public int? GraduationYear { get; set; }
        
        public string? Gender { get; set; } // M, F
        
        public string? Major { get; set; }
    }
}