using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWareHouseGetByCodeRequest
    {
        public string WareHouseCode { get; set; }
        public int ShopID { get; set; }
    }
    public class ZWareHouseGetByCodeResponse
    {
        public int WareHouseID { get; set; }
        public string IsLock { get; set; }
    }
}
