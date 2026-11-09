using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderSendTransportSuccessRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string AmndState { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
