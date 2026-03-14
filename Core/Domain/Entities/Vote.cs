namespace Domain.Entities
{
    public class Vote : BaseEntity
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
        public bool IsUpvote { get; set; }

        // Relations
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
