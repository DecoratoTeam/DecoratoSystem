namespace Domain.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string UserId { get; set; }
        public string? ConversationId { get; set; }
        public bool IsFromUser { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? AIDesignId { get; set; }

        public User User { get; set; }
        public AIDesign? AIDesign { get; set; }

        public ChatMessage()
        {
            UserId = string.Empty;
            Content = string.Empty;
            User = null!;
        }
    }
}
