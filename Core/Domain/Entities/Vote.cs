namespace Domain.Entities
{
    public class Vote
    {
        public string Id { get; set; }
        public bool IsUpvote { get; set; }
        public DateTime CreatedAt { get; set; }

        public string PostId { get; set; }
        public Post Post { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public Vote()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
