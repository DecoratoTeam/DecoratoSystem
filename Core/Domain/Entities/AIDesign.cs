namespace Domain.Entities
{
    public class AIDesign : BaseEntity
    {
        public string UserId { get; set; }
        public string RoomTypeId { get; set; }
        public string DesignStyleId { get; set; }

        public string? Prompt { get; set; }
        public string OriginalImageUrl { get; set; }
        public string GeneratedImageUrl { get; set; }

        // Relations
        public User User { get; set; }
        public RoomType RoomType { get; set; }
        public DesignStyle DesignStyle { get; set; }
    }
}
