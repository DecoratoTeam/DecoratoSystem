using System;

namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }

        public Comment()
        {
            PostId = string.Empty;
            UserId = string.Empty;
            Content = string.Empty;
            Post = null!;
            User = null!;
        }
    }
}
