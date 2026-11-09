using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWarehouseImportAddDetailRequest
    {
        public int ShopID { get; set; }
        public int WarehouseImportID { get; set; }
        public int ProductVariantID { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public int CreatedBy { get; set; }
    }
}
