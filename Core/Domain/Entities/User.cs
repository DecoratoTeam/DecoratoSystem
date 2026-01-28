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
        public string Email { get; set; }
        public Role Role { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }

        public ICollection<ShowcaseDesign> ShowcaseDesigns { get; set; } = new List<ShowcaseDesign>();
        public ICollection<AIDesign> AIDesigns { get; set; } = new List<AIDesign>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

