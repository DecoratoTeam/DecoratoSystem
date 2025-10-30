using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public User()
        {
          Id=Guid.NewGuid().ToString(); 
        }
    }
}

