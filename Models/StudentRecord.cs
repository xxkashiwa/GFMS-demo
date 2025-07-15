using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFMS.Models
{
    public class StudentRecord
    {
        public int Id { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public string RecordType { get; set; } = string.Empty; // Grade, Award, Punishment
        
        public string? Description { get; set; }
        
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
    }
}