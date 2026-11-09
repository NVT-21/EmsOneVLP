using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWarehouseImportAddRequest
    {
        public int ProviderID { get; set; }
        public string DateImport { get; set; }
        public int WarehouseID { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
        public int ShopID { get; set; }
    }
    public class ZWarehouseImportAddResponse
    {
        public int ID { get; set; }
    }
}
