using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWarehouseImportGetDataByShopIDRequest
    {
        public int ShopID { get; set; }
    }
    public class ZWarehouseImportGetDataByShopIDResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public long TotalAmount { get; set; }
        public string DateImport { get; set; }
        public string CreaterName { get; set; }
        public string CreatedDate { get; set; }
    }
}
