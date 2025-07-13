using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    internal class StudentRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        [RegularExpression("Grade|Award|Punishment")]
        public string RecordType { get; set; } = null!;

        public string? Title { get; set; }

        public string? Term { get; set; }

        public double? ScoreJson { get; set; }

        public string? Description { get; set; }

        public DateTime? Date { get; set; }
    }
}
