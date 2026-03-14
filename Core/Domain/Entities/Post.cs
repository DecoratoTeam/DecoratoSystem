using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Post : BaseEntity
    {
        public string UserId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        
        public DateTime? UpdatedAt { get; set; }

        // Stats
        public int ViewCount { get; set; }

        // Relations
        public User User { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<SavedPost> SavedPosts { get; set; }
    }

}
