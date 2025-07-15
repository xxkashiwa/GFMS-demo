using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFMS.Models
{
    public class ArchiveRequest
    {
        public int Id { get; set; }
        
        [Required]
        public int StudentId { get; set; }
        
        [Required]
        public string ReceiverName { get; set; } = string.Empty;
        
        [Required]
        public string ReceiverAddress { get; set; } = string.Empty;
        
        public DateTime RequestDate { get; set; } = DateTime.Now;
        
        public string Status { get; set; } = "�����"; // �����, ��ͨ��, �Ѽĳ�, ��ǩ��
        
        public string? TrackingNumber { get; set; }
        
        public DateTime? DispatchDate { get; set; }
        
        public DateTime? ReceiveDate { get; set; }
        
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
    }
}