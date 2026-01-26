using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RoomType : BaseEntity
    {
        public string Name { get; set; }              // "Living Room", "Kitchen"
        public string IconUrl { get; set; }           // Icon for filter button

        public ICollection<ShowcaseDesign> ShowcaseDesigns { get; set; }
    }

}
