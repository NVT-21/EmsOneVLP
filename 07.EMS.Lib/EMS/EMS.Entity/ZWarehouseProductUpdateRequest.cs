using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWarehouseProductUpdateRequest
    {
        public int ID { get; set; }
        public int WarehouseID { get; set; }
        public int ProductID { get; set; }
        public int VariantID { get; set; }
        public int OldQuantity { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
        public int ShopID { get; set; }
    }
}
