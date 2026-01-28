namespace Domain.Entities
{
    public class ShowcaseDesign
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPopular { get; set; }
        public bool IsTrending { get; set; }
        public DateTime CreatedAt { get; set; }

        public string RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        public string DesignStyleId { get; set; }
        public DesignStyle DesignStyle { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public ShowcaseDesign()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
