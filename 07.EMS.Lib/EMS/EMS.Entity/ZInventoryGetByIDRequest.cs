using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZInventoryGetByIDRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZInventoryGetByIDResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public XProviderModel Provider { get; set; }
        public XWarehouseModel Warehouse { get; set; }
        public XOrderModel Order { get; set; }
        public XStockTakeModel StockTake { get; set; }
        public XStatusModel Status { get; set; }
        public long TotalAmount { get; set; }
        public string ImplementationDate { get; set; }
        public string ReceiverName { get; set; }
        public string MobileNumber { get; set; }
        public string Note { get; set; }
        public string Reason { get; set; }
        public string CreaterName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdaterName { get; set; }
        public string UpdatedDate { get; set; }
        public List<ZInventoryDetailGetByInventoryIDResponse> ListInventoryDetail { get; set; }
    }
}
