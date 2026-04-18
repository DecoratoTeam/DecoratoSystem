using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        public string Id { get; set; }                          // GUID
        
        // Basic Info
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public Role Role { get; set; }                          // Admin or Customer
        
        // OTP Authentication
        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }
        
        // Social Login
        public string? GoogleId { get; set; }
        public string? FacebookId { get; set; }
        
        // Settings
        public bool IsDarkMode { get; set; } = false;
        public string Language { get; set; } = "English";
        
        // Relations
        public ICollection<AIDesign> MyAIDesigns { get; set; } = new List<AIDesign>();
        public ICollection<Post> MyPosts { get; set; } = new List<Post>();
        public ICollection<Rating> MyRatings { get; set; } = new List<Rating>();
        public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        
        public User()
        {
            Id = Guid.NewGuid().ToString();
            UserName = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}

