using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    public class User
    {
        public required string UserId { get; set; }
        public string? UserName { get; set; }
        public string? AuthType { get; set; }
    }
}
