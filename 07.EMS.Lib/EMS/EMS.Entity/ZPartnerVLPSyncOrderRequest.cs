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
    public class ZPartnerVLPOrderStatusAddRequest
    {
        [DataMember]
        public string AppCode { get; set; }
        [DataMember]
        public string AppApiCode { get; set; }
        [DataMember]
        public string SchemeCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string OrderStatusCode { get; set; }
        [DataMember]
        public string OperatorAccount { get; set; }
        [DataMember]
        public string OperatorName { get; set; }
        [DataMember]
        public long OperatorTime { get; set; }
        [DataMember]
        public string PosCode { get; set; }
        [DataMember]
        public string FailReason { get; set; }
        [DataMember]
        public string FailAction { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public List<ZPartnerVLPOrderStatusAddStatusInfoRequest> ParcelList { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerVLPOrderStatusAddStatusInfoRequest
    {
        [DataMember]
        public string WayBillCode { get; set; }
        [DataMember]
        public string WayBillStatus { get; set; }
        [DataMember]
        public string WayBillStatusCode { get; set; }
        [DataMember]
        public string OperatorAccount { get; set; }
        [DataMember]
        public string OperatorName { get; set; }
        [DataMember]
        public long OperatorTime { get; set; }
    }
}
