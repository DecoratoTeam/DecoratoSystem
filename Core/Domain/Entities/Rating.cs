namespace Domain.Entities
{
    public class Rating : BaseEntity
    {
        public string UserId { get; set; }
        public string ShowcaseDesignId { get; set; }

        public int Value { get; set; }
        public string? Review { get; set; }

        // Relations
        public User User { get; set; }
        public ShowcaseDesign ShowcaseDesign { get; set; }
    }
}
