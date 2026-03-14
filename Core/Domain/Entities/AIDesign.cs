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
        public string RoomTypeId { get; set; }
        public string DesignStyleId { get; set; }

        // Input
        public string Prompt { get; set; }            // User's description
        public string? InputImageUrl { get; set; }    // User's uploaded photo (for migration: OriginalImageUrl)

        // Output
        public string ResultImageUrl { get; set; }    // AI result (for migration: GeneratedImageUrl)

        // Relations
        public User User { get; set; }
        public RoomType RoomType { get; set; }
        public DesignStyle DesignStyle { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
    }
}
