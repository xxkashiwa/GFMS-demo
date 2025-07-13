using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    internal class Archive
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        public DateTime ArchiveDate { get; set; } = DateTime.Now;

        public bool IsSealed { get; set; } = false;

        public string? Notes { get; set; }
    }
}
