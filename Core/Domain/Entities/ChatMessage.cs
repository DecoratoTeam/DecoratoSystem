namespace Domain.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string UserId { get; set; }
        public string ConversationId { get; set; }

        public bool IsFromUser { get; set; }
        public string Content { get; set; }

        // Relations
        public User User { get; set; }
    }
}
