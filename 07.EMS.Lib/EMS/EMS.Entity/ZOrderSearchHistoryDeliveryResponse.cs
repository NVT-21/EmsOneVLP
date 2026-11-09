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
    public class ZOrderSearchHistoryDeliveryRequest
    {
        [DataMember]
        public string ShippingCode { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZOrderSearchHistoryDeliveryResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int DeliveryIndex { get; set; }
        [DataMember]
        public string DeliveryDate { get; set; }
        [DataMember]
        public string InputDate { get; set; }
        [DataMember]
        public string PostmanName { get; set; }
        [DataMember]
        public string PostOffice { get; set; }
        [DataMember]
        public string ReasonDeliveryFailure { get; set; }
        [DataMember]
        public string SolutionName { get; set; }
        [DataMember]
        public string DeliveryNote { get; set; }
        [DataMember]
        public int DeliveryReturn { get; set; }
        [DataMember]
        public int IsDeliverable { get; set; }
        [DataMember]
        public string RealReciverName { get; set; }
    }
}