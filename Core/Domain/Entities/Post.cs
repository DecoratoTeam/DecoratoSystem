namespace Domain.Entities
{
    public class Post
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public Post()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
