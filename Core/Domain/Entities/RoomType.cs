using System.Collections.Generic;

namespace Domain.Entities
{
    public class RoomType : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }

        public ICollection<ShowcaseDesign> ShowcaseDesigns { get; set; } = new List<ShowcaseDesign>();
        public ICollection<AIDesign> AIDesigns { get; set; } = new List<AIDesign>();

        public RoomType()
        {
            Name = string.Empty;
        }
    }

}
