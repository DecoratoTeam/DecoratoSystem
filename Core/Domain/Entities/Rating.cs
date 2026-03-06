namespace Domain.Entities
{
    public class Rating
    {
        public string Id { get; set; }
        public int Value { get; set; } // 1-5
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ShowcaseDesignId { get; set; }
        public ShowcaseDesign ShowcaseDesign { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public Rating()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
