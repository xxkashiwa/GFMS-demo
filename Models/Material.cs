using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFMS.Models
{
    public class Material
    {
        public int Id { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public string FileType { get; set; } = string.Empty;
        
        [Required]
        public string FilePath { get; set; } = string.Empty;
        
        public DateTime UploadTime { get; set; } = DateTime.Now;
        
        public string Status { get; set; } = "´ıÉóºË"; // ´ıÉóºË, Í¨¹ı, ÍË»Ø
        
        public string? ReviewComment { get; set; }
        
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
    }
}