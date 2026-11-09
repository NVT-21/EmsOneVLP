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
    public class GrabUpdateTransportSuccessRequest
    {
        [DataMember]
        public string itemcode { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string ordercode { get; set; }
        [DataMember]
        public int total_freight { get; set; }
        [DataMember]
        public int distance { get; set; }
        [DataMember]
        public List<GrabUpdateTransportSuccessEstimatedTimeLine> estimatedTimeLine { get; set; }
    }
    [Serializable]
    [DataContract]
    public class GrabUpdateTransportSuccessEstimatedTimeLine
    {
        [DataMember]
        public string pickup { get; set; }
        [DataMember]
        public string dropoff { get; set; }
        [DataMember]
        public string completed { get; set; }
    }
}
