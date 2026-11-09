using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPartnerWebhookListHistoryStatusRequest
    {
        public string PartnerCode { get; set; }
    }

    public class ZPartnerWebhookListHistoryStatusResponse
    {
        public int ID { get; set; }
        public string PartnerCode { get; set; }
        public string ItemCode { get; set; }
        public string OrderCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusTime { get; set; }
        public string StatusNote { get; set; }
        public string PosCode { get; set; }
    }

    public class ZPartnerWebhookUpdateHistoryStatusRequest
    {
        public int ID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
