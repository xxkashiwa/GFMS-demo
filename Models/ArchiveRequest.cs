using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    internal class ArchiveRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        public string ReceiverName { get; set; } = null!;

        [Required]
        public string ReceiverAddress { get; set; } = null!;

        public string? ProofFilePath { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;

        [RegularExpression("待审核|已通过|已寄出|已签收")]
        public string Status { get; set; } = "待审核";

        public string? CourierCompany { get; set; }

        public string? TrackingNumber { get; set; }

        public DateTime? DispatchDate { get; set; }

        public DateTime? ReceiveDate { get; set; }

        public string? ReceivedBy { get; set; }
    }
}
