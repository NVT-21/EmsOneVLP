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
    public class ZGrabReceiveStatusRequest
    {
        [DataMember]
        public string TrackingCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Locate { get; set; }
        [DataMember]
        public string DateTime { get; set; }
        [DataMember]
        public long TotalFee { get; set; }
        [DataMember]
        public string PartnerCode { get; set; }
        [DataMember]
        public long Distance { get; set; }
        [DataMember]
        public string DriverName { get; set; }
        [DataMember]
        public string DriverPhone { get; set; }
        [DataMember]
        public string DriverLicensePlate { get; set; }
    }
}