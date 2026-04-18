using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Post : BaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ViewCount { get; set; }

        public User User { get; set; }
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();

        public Post()
        {
            UserId = string.Empty;
            Title = string.Empty;
            Content = string.Empty;
            User = null!;
        }
    }

}
