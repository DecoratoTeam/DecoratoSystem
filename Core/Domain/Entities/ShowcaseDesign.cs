using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShowcaseDesign : BaseEntity
    {
        public string Title { get; set; }             // "Living Room Serenbe, Georgia"
        public string? Description { get; set; }      // Changed from Location
        public string ImageUrl { get; set; }          // Main image

        // Categorization
        public string RoomTypeId { get; set; }
        public string DesignStyleId { get; set; }
        public string UserId { get; set; }

        // Display Flags
        public bool IsPopular { get; set; }           // Show in Popular section?
        public bool IsTrending { get; set; }          // Show in Trending?

        // Stats
        public int ViewCount { get; set; }
        public double AverageRating { get; set; }     // 7.5 shown on card

        // Relations
        public RoomType RoomType { get; set; }
        public DesignStyle DesignStyle { get; set; }
        public User User { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<SavedDesign> SavedDesigns { get; set; }
    }

}
