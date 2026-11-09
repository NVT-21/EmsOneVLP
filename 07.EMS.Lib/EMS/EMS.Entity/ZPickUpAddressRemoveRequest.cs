using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPickUpAddressRemoveRequest
    {
        public int PickUpAddressID { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
