using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTikWebhookSearchListDataLogRequest
    {
        [DataMember]
        public string PageID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTikWebhookSearchListDataLogResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string OrderID { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string UpdateTime { get; set; }
    }
}
