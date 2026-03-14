using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // للـ Recently Watched و Saved Designs
    public class SavedDesign : BaseEntity
    {
        public string UserId { get; set; }
        public string ShowcaseDesignId { get; set; }

        public bool IsSaved { get; set; }           // حفظ في المفضلة
        public DateTime? LastViewedAt { get; set; }  // آخر مشاهدة

        // Relations
        public User User { get; set; }
        public ShowcaseDesign ShowcaseDesign { get; set; }
    }
}
