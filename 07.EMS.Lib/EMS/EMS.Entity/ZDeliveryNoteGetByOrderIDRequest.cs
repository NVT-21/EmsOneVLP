using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZDeliveryNoteGetByOrderIDRequest
    {
        public int ShopID { get; set; }
        public int OrderID { get; set; }
    }
    public class ZDeliveryNoteGetByOrderIDResponse
    {
        public int WareHouseProductID { get; set; }
        public int VariantID { get; set; }
        public int Quantity { get; set; }
        public string WareHouseName { get; set; }
        public int OutputQuantity { get; set; }
    }
}
