using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string UserId { get; set; }
        public string? ConversationId { get; set; }      // Group by session

        public bool IsFromUser { get; set; }          // true = user, false = bot
        public string Content { get; set; }           // Changed from MessageText to Content
        public string? ImageUrl { get; set; }         // If image attached

        public string? AIDesignId { get; set; }          // If bot created design

        // Relations
        public User User { get; set; }
        public AIDesign? AIDesign { get; set; }
    }

}
