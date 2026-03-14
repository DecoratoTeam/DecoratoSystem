namespace Domain.Entities
{
    public class SavedPost : BaseEntity
    {
        public string UserId { get; set; }
        public string PostId { get; set; }

        // Relations
        public User User { get; set; }
        public Post Post { get; set; }
    }
}
