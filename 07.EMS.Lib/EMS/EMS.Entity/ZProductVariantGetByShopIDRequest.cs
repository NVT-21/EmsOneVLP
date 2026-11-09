using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantGetByShopIDRequest
    {
        public int ShopID { get; set; }
    }
    public class ZProductVariantGetByShopIDResponse
    {
        public int ID { get; set; }
        public string SKU { get; set; }
        public string Options { get; set; }
        public string ProductName { get; set; }
    }
}
