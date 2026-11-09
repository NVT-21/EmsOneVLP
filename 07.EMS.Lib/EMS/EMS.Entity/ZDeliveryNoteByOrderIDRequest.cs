using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZDeliveryNoteByOrderIDRequest
    {
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public int ShopID { get; set; }
        public string BeforeStatus { get; set; }
        public string AfterStatus { get; set; }
    }
    public class ZDeliveryNoteByOrderIDResponse
    {
        public string OrderCode { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public string WareHouseName { get; set; }
        public string WareHouseAddress { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public int WareHouseQuantity { get; set; }
        public int WareHouseProductID { get; set; }
        public int VariantID { get; set; }
        public string VariantName { get; set; }
    }
}
