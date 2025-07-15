using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFMS.Models
{
    public class Archive
    {
        public int Id { get; set; }
        
        public string? MeterialUuid { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        
        public DateTime ArchiveDate { get; set; } = DateTime.Now;
        
        public bool IsSealed { get; set; } = false;
        
        public string? Notes { get; set; }
        
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
    }
}