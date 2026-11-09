using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
   public class ZProductAddImagesRequest
    {
        public int ProductID { get; set; }
        public List<ZImageModel> FileNames { get; set; }
    }

}
