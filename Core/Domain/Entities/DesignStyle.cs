namespace Domain.Entities
{
    public class DesignStyle : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<ShowcaseDesign> ShowcaseDesigns { get; set; }
        public ICollection<AIDesign> AIDesigns { get; set; }
    }
}
