using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZExpectedDeliverySearchDateRequest
    {
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public int TransportServiceID { get; set; }
        [DataMember]
        public int SenderProvinceID { get; set; }
        [DataMember]
        public int SenderDistrictID { get; set; }
        [DataMember]
        public int ReceiverProvinceID { get; set; }
        [DataMember]
        public int ReceiverDistrictID { get; set; }
    }
}
