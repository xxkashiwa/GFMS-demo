using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFMS.Models
{
    public class QueryLog
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        public DateTime QueryTime { get; set; } = DateTime.Now;
        
        public string? QueryKeyWord { get; set; }
        
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}