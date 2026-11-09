using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPeriodListRequest
    {
        public string Type { get; set; }
        public int ShopID { get; set; }
    }
    public class ZPeriodListResponse
    {
        public int ID { get; set; }
        public int Value { get; set; }
    }
}
