using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vote : BaseEntity
    {
        public string PostId { get; set; }
        public string UserId { get; set; }

        public bool IsUpvote { get; set; } // true = ↑, false = ↓

        // Relations
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
