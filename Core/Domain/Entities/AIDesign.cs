namespace Domain.Entities
{
    public class AIDesign
    {
        public string Id { get; set; }
        public string OriginalImageUrl { get; set; }
        public string GeneratedImageUrl { get; set; }
        public string? Prompt { get; set; }
        public DateTime CreatedAt { get; set; }

        public string RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        public string DesignStyleId { get; set; }
        public DesignStyle DesignStyle { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public AIDesign()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
