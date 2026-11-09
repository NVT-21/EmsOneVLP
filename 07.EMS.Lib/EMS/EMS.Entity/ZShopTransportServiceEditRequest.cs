using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
   public class ZShopTransportServiceEditRequest
    {
        public string Data { get; set; }
        public int Type { get; set; }
        public int UpdatedBy { get; set; }
        public int ShopID { get; set; }
    }
}
