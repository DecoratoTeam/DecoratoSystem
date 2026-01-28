namespace Domain.Entities
{
    public class ChatMessage
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string Content { get; set; }
        public bool IsFromUser { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ChatMessage()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
