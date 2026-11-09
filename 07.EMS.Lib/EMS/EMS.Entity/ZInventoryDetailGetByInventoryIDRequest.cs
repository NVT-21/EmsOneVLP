using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZInventoryDetailGetByInventoryIDRequest
    {
        public int InventoryID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZInventoryDetailGetByInventoryIDResponse
    {
        public int ID { get; set; }
        public XProductVariantModel ProductVariant { get; set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public long WarehouseQuantity { get; set; }
        public long StockTakeQuantity { get; set; }
        public long InventoryQuantity { get; set; }
    }
}
