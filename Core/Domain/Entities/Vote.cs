using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Vote : BaseEntity
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public bool IsUpvote { get; set; }            // true = ↑, false = ↓

        // Relations
        public Post Post { get; set; }
        public User User { get; set; }
    }

}
