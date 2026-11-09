using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWareHouseListProductByWareHouseIDRequest
    {
        public int WareHouseID { get; set; }
    }

    public class ZWareHouseListProductByWareHouseIDResponse
    {
        public int ID { get; set; }
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
