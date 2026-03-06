using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating : BaseEntity
    {
        public int UserId { get; set; }

        // What did they rate? (Only ONE will have value)
        public int? ShowcaseDesignId { get; set; }
        public int? PostId { get; set; }

        public int Stars { get; set; }                // 1-5 stars
        public string? Review { get; set; }           // Optional review text

        // Relations
        public User User { get; set; }
        public ShowcaseDesign? ShowcaseDesign { get; set; }
        public Post? Post { get; set; }
    }
}
