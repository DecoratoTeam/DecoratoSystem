using System.Collections.Generic;

namespace Domain.Entities
{
    public class ShowcaseDesign : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string ImageUrl { get; set; }

        public string RoomTypeId { get; set; }
        public string DesignStyleId { get; set; }
        public string UserId { get; set; }

        public bool IsPopular { get; set; }
        public bool IsTrending { get; set; }

        public int ViewCount { get; set; }
        public double AverageRating { get; set; }

        public RoomType RoomType { get; set; }
        public DesignStyle DesignStyle { get; set; }
        public User User { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<SavedDesign> SavedDesigns { get; set; } = new List<SavedDesign>();

        public ShowcaseDesign()
        {
            Title = string.Empty;
            ImageUrl = string.Empty;
            RoomTypeId = string.Empty;
            DesignStyleId = string.Empty;
            UserId = string.Empty;
            RoomType = null!;
            DesignStyle = null!;
            User = null!;
        }
    }
}
