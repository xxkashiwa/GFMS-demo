using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    internal class Material
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        public string FileType { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;

        public int? UploadedBy { get; set; }

        [ForeignKey(nameof(UploadedBy))]
        public User? UploadedByUser { get; set; }

        public DateTime UploadTime { get; set; } = DateTime.Now;

        [RegularExpression("待审核|通过|退回")]
        public string Status { get; set; } = "待审核";

        public string? ReviewComment { get; set; }
    }
}
