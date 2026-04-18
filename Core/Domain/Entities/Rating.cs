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
        public string? ShowcaseDesignId { get; set; }
        public string? PostId { get; set; }
        public int Value { get; set; }
        public string? Review { get; set; }

        public User User { get; set; }
        public ShowcaseDesign? ShowcaseDesign { get; set; }
        public Post? Post { get; set; }

        public Rating()
        {
            UserId = string.Empty;
            User = null!;
        }
    }
}
