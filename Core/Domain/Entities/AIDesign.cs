using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AIDesign : BaseEntity
    {
        public int UserId { get; set; }

        // Input
        public string Prompt { get; set; }            // User's description
        public string? InputImageUrl { get; set; }    // User's uploaded photo

        // Output
        public string ResultImageUrl { get; set; }    // AI result

        // Relations
        public User User { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
