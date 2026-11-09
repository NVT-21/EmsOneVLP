using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWarehouseImportGetDataByIDRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZWarehouseImportGetDataByIDResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public long TotalAmount { get; set; }
        public string DateImport { get; set; }
        public string Note { get; set; }
        public string CreaterName { get; set; }
        public string CreatedDate { get; set; }
    }
}
