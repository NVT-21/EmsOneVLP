using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTikWebhookAddDataLogRequest
    {
        public int Type { get; set; }
        public string PageID { get; set; }
        public string Timestamp { get; set; }
        public string OrderID { get; set; }
        public string OrderStatus { get; set; }
        public string UpdateTime { get; set; }
    }

    public class ZTikWebhookUpdateDataLogRequest
    {
        public string PageID { get; set; }
        public string OrderID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
