namespace Domain.Entities
{
    public class ShowcaseDesign : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }

        public string RoomTypeId { get; set; }
        public string DesignStyleId { get; set; }

        public bool IsPopular { get; set; }
        public bool IsTrending { get; set; }

        // Relations
        public User User { get; set; }
        public RoomType RoomType { get; set; }
        public DesignStyle DesignStyle { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
