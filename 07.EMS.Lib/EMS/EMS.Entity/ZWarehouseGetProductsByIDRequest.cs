using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWarehouseGetProductsByIDRequest
    {
        public int WarehouseID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ShopID { get; set; }
        public string SearchCode { get; set; }
    }
    public class ZWarehouseGetProductsByIDResponse
    {
        public int WarehouseProductID { get; set; }
        //public int WarehouseID { get; set; }
        //public string WarehouseCode { get; set; }
        //public string WarehouseName { get; set; }
        public int FirstQuantity { get; set; }
        public int QuantityIn { get; set; }
        public int QuantityOut { get; set; }
        public int QuantityAdjust { get; set; }
        public int FinalQuantity { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int VariantID { get; set; }
        public string VariantName { get; set; }
        public string VariantImage { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public int TotalInventory { get; set; }
        public string PalletCode { get; set; }
        public List<ZWarehouseProductGetByVariantIDResponse> ListWareHouse { get; set; }
    }
}
