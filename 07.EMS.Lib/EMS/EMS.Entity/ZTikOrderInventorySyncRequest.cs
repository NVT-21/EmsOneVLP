using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTikOrderInventorySyncRequest
    {
        public int ShopID { get; set; }
        public string PageID { get; set; }
        public string TiktokOrderID { get; set; }
        public List<ZTikOrderInventoryUpdateRequest> ListItem { get; set; }
        public string Note { get; set; }
        public string ReceiverName { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZTikOrderInventoryUpdateRequest
    {
        public string SKUID { get; set; }
        public string SellerSKU { get; set; }
        public int Quantity { get; set; }
    }
}
