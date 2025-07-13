using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    internal class QueryLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        public int? TargetStudentId { get; set; }

        [ForeignKey(nameof(TargetStudentId))]
        public Student? TargetStudent { get; set; }

        public DateTime QueryTime { get; set; } = DateTime.Now;

        public string? QueryReason { get; set; }
    }
}
