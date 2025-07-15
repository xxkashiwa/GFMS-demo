using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GFMS.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        public string Username { get; set; } = string.Empty; // 工号/学号
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty; // Admin, Teacher, Student
        
        public string? Name { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 兼容性属性，保持现有代码正常工作
        public string UserId
        {
            get => Username;
            set => Username = value;
        }

        public string UserName
        {
            get => Name ?? Username;
            set => Name = value;
        }

        public string GrantedType
        {
            get => Role;
            set => Role = value;
        }
    }
}
