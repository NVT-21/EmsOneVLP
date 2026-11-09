using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWareHouseAddInventoryRequest
    {
        public int ID { get; set; } // ID WareHuose
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int UpdatedBy { get; set; }
    }
}
