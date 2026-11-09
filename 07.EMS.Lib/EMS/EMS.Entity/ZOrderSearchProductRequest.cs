using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOrderSearchProductRequest
    {
        public string Name { get; set; }
        public int ShopID { get; set; }
        public string CrmOrPaypostCode { get; set; }
    }
    public class ZOrderSearchProductResponse
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public List<ZVariantResponse> Variants { get; set; }
    }
    public class ZVariantResponse
    {
        public int ProductID { get; set; }
        public int VariantID { get; set; }
        public string VariantName { get; set; }
        public string Image { get; set; }
        public long Price { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public double Weight { get; set; }
        public bool Selected { get; set; } = false;
        public int NumberOfProduct { get; set; } = 1;
    }
}
