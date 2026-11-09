using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWarehouseProductGetByVariantIDRequest
    {
        public int VariantID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ShopID { get; set; }
    }
    public class ZWarehouseProductGetByVariantIDResponse
    {
        public int WarehouseProductID { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int Quantity { get; set; }
        public int FirstQuantity { get; set; }
        public int QuantityIn { get; set; }
        public int QuantityOut { get; set; }
        public int QuantityAdjust { get; set; }
        public int FinalQuantity { get; set; }
    }
}
