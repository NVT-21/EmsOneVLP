using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZInventoryEditRequest
    {
        public int ID { get; set; }
        public int WarehouseID { get; set; }
        public int ProviderID { get; set; }
        public int OrderID { get; set; }
        public int StockTakeID { get; set; }
        public string ImplementationDate { get; set; }
        public string ReceiverName { get; set; }
        public string Note { get; set; }
        public string Type { get; set; }
        public int UpdatedBy { get; set; }
        public int ShopID { get; set; }
        public List<ZInventoryDetailAddRequest> ListInventoryDetail { get; set; }
        public string MobileNumber { get; set; }
    }
}
