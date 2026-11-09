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
    public class ZPartnerHMOrderAddResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public int TransportFee { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public ZTransportPostageChargedResponse TransportFeeDetail { get; set; }

    }

    [Serializable]
    [DataContract]
    public class ZPartnerHMOrderCreateLabelRequest
    {
        [DataMember]
        public string Poscode { get; set; }
        [DataMember]
        public string Sendingtime { get; set; }
        [DataMember]
        public string Trackingcode { get; set; }
        [DataMember]
        public string Sendername { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public int TotalAmount { get; set; }
        [DataMember]
        public string ReceiverPhone { get; set; }
        [DataMember]
        public int TotalFee { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public int Weight { get; set; }
        [DataMember]
        public string Service { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Ordercode { get; set; }
    }

    public class ZPartnerHMOrderCreateLabelResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string oOrderHM { get; set; } = String.Empty;
        public string oOrderHMZcode { get; set; }
    }
}
