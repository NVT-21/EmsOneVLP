using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantGetBySKURequest
    {
        public int ShopID { get; set; }
        public string SKU { get; set; }
    }
    public class ZProductVariantGetBySKUResponse
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int VariantID { get; set; }
        public string VariantName { get; set; }
        public string VariantImage { get; set; }
        public long Price { get; set; }
        public long PurchasePrice { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public int Weight { get; set; }
        public long CurrentQuantity { get; set; }
        public long WareHouseQuantity { get; set; }
        //public List<ZWarehouseProductGetByVariantIDResponse> listWareHouse { get; set; }
    }
}
