using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWarehouseImportGetDetailByIDRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZWarehouseImportGetDetailByIDResponse
    {
        public int ID { get; set; }
        public string ProductVariantName { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
    }
}
