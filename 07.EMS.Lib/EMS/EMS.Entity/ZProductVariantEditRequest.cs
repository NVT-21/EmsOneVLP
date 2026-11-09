using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantEditRequest
    {
        public int ID { get; set; }
        public string Options { get; set; }
        public string OptionName1 { get; set; }
        public string OptionValue1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionValue2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionValue3 { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public string Avatar { get; set; }
        public long Price { get; set; }
        public int Weight { get; set; }
        public string Size { get; set; }
        public int UpdatedBy { get; set; }
        public int ShopID { get; set; }
        public string BatchNumber { get; set; }
    }
    public class ZOptionModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
    }
}
