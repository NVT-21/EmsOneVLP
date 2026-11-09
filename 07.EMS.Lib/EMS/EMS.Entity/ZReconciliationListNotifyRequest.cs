using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZReconciliationListNotifyRequest
    {
        public int ShopID { get; set; }
    }

    public class ZReconciliationListNotifyResponse
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string TypeText { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CreatedDate { get; set; }
        public string NotifyDate { get; set; }
        public string NotifyMessage { get; set; }
        public int ShopID { get; set; }
        public string IsCheck { get; set; }
    }

    public class ZReconciliationUpdateNotifyRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
