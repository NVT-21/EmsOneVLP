using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductGetByVariantIDRequest
    {
        public int VariantID { get; set; }
    }
    public class ZProductGetByVariantIDResponse
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
        public List<ZVariantOptionModel> Options { get; set; }
        public List<ZInventoryNumberModel> InventoryNumber { get; set; }
    }
    public class ZVariantOptionModel
    {
        public int OptionID { get; set; }
        public string OptionName { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
    }
}
