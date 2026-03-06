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
        // Basic Info (Sign Up Screen)
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public Role Role { get; set; }

        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }

        // Social Login (Facebook/Google buttons) >> if you needed
        public string? GoogleId { get; set; }
        public string? FacebookId { get; set; }

        // Settings (Profile Screen)
        public bool IsDarkMode { get; set; } = false;
        public string Language { get; set; } = "English";

        // Relations
        public ICollection<AIDesign> MyAIDesigns { get; set; }
        public ICollection<Post> MyPosts { get; set; }
        public ICollection<Rating> MyRatings { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

