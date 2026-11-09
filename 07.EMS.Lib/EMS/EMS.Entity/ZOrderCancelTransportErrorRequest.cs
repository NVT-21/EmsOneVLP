using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderCancelTransportErrorRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
