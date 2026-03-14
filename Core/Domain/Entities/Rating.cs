using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rating : BaseEntity
    {
        public string UserId { get; set; }

        // What did they rate? (Only ONE will have value)
        public string? ShowcaseDesignId { get; set; }
        public string? PostId { get; set; }

        public int Value { get; set; }                // Was "Stars", now matches migration: 1-5 rating value
        public string? Review { get; set; }           // Optional review text

        // Relations
        public User User { get; set; }
        public ShowcaseDesign? ShowcaseDesign { get; set; }
        public Post? Post { get; set; }
    }
}
