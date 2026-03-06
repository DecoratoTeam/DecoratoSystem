namespace Domain.Entities
{
    public class RoomType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public RoomType()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ICollection<ShowcaseDesign> ShowcaseDesigns { get; set; } = new List<ShowcaseDesign>();
        public ICollection<AIDesign> AIDesigns { get; set; } = new List<AIDesign>();
    }
}
