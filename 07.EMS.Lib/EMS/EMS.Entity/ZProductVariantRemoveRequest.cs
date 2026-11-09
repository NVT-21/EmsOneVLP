using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantRemoveRequest
    {
        public int ID { get; set; }
        public int UpdatedBy { get; set; }
        public int ShopID { get; set; }
    }
}
