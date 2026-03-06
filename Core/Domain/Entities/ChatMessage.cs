using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChatMessage : BaseEntity
    {
        public int UserId { get; set; }
        public int? ConversationId { get; set; }      // Group by session

        public bool IsFromUser { get; set; }          // true = user, false = bot
        public string MessageText { get; set; }
        public string? ImageUrl { get; set; }         // If image attached

        public int? AIDesignId { get; set; }          // If bot created design

        // Relations
        public User User { get; set; }
        public AIDesign? AIDesign { get; set; }
    }

}
