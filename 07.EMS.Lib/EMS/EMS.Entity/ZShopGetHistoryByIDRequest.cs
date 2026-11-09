using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopGetHistoryByIDRequest
    {
        public int ID { get; set; }
    }
    public class ZShopGetHistoryByIDResponse
    {
        public string ActionName { get; set; }
        public string ActionCode { get; set; }
        public string Description { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
    }
}
