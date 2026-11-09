using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZInventorySearchListRequest
    {
        public string Code { get; set; }
        public int WarehouseID { get; set; }
        public int ProviderID { get; set; }
        public string OrderCode { get; set; }
        public string StockTakeCode { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateType { get; set; }
        public string Type { get; set; }
        public int ShopID { get; set; }
        public string SearchKey { get; set; }
    }
    public class ZInventorySearchListResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public XWarehouseModel Warehouse { get; set; }
        public XProviderModel Provider { get; set; }
        public XOrderModel Order { get; set; }
        public XStockTakeModel StockTake { get; set; }
        public XStatusModel Status { get; set; }
        public long TotalAmount { get; set; }
        public string ImplementationDate { get; set; }
        public string Note { get; set; }
        public string Reason { get; set; }
        public string CreaterName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdaterName { get; set; }
        public string UpdatedDate { get; set; }
        public string Type { get; set; }
    }
}
