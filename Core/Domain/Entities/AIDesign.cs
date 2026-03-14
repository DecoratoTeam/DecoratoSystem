using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AIDesign : BaseEntity
    {
        public string UserId { get; set; }

        // Input
        public string? OriginalImageUrl { get; set; }   // User's uploaded photo
        public string? Prompt { get; set; }              // User's description

        // Output
        public string GeneratedImageUrl { get; set; }   // AI result

        // Categorization
        public string RoomTypeId { get; set; }
        public string DesignStyleId { get; set; }

        // Relations
        public User User { get; set; }
        public RoomType RoomType { get; set; }
        public DesignStyle DesignStyle { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}
