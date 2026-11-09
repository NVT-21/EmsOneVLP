using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantGetByIDsRequest
    {
        public string IDs { get; set; } //vd: 1,2,3,4,5
    }
    public class ZProductVariantGetByIDsResponse
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
        public string Weight { get; set; }
        public int Quantity { get; set; }
    }
}
