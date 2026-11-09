using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductUnitSearchListRequest
    {
        public int ShopID { get; set; }
        public string IsLock { get; set; }
    }
    public class ZProductUnitSearchListResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
