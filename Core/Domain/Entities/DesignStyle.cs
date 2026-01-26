using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DesignStyle : BaseEntity
    {
        public string Name { get; set; }              // "Modern", "Rustic"
        public string PreviewImage { get; set; }      // Preview image

        public ICollection<ShowcaseDesign> ShowcaseDesigns { get; set; }
    }

}
