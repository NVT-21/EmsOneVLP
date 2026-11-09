using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantGetDetailByIDRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZProductVariantGetDetailByIDResponse
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
        public string Size { get; set; }
    }
}
