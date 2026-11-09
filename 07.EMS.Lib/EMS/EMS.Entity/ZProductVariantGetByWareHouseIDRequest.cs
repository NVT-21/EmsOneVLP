using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantGetByWareHouseIDRequest
    {
        public int WareHouseID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZProductVariantGetByWareHouseIDResponse
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductVariantID { get; set; }
        public string ProductVariantName { get; set; }
        public string ProductVariantSKU { get; set; }
        public long ProductVariantPrice { get; set; }
        public int NumberOfInventory { get; set; }
        public string DetailName { get; set; }
    }
}
