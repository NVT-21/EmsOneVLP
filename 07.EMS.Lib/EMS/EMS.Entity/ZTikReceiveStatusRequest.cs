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
    public class ZTikReceiveStatusRequest
    {
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string DateTime { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTikReverseStatusRequest
    {
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ReverseOrderCode { get; set; }
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public string DateTime { get; set; }
    }
}
