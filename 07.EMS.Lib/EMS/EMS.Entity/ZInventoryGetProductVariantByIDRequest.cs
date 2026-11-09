using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZInventoryGetProductVariantByIDRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZInventoryGetProductVariantByIDResponse
    {
        public int ProductVariantID { get; set; }
        public string ProductName { get; set; }
        public string ProductVariantOptions { get; set; }
        public int NumberOfInventory { get; set; }
        public int NumberOfDifferent { get; set; }
    }
}
