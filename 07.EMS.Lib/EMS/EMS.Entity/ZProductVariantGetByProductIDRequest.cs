using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductVariantGetByProductIDRequest
    {
        public int ProductID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZProductVariantGetByProductIDResponse
    {
        public int ProductID { get; set; }
        public int ID { get; set; }
        public string Options { get; set; }
        public string Avatar { get; set; }
        public long Price { get; set; }
        public long InventoryNumber { get; set; }
    }
}
