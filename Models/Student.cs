using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    internal class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string StudentNo { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(1)]
        [RegularExpression("M|F")]
        public string? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(100)]
        public string? Major { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public ICollection<StudentRecord>? StudentRecords { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<Archive>? Archives { get; set; }
        public ICollection<ArchiveRequest>? ArchiveRequests { get; set; }
    }
}
