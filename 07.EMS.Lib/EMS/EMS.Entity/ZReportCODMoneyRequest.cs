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
    public class ZReportCODMoneyRequest
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public int CODTransType { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZReportCODMoneyResponse
    {
        [DataMember]
        public string TimeRelease { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public long AmountOfMoney { get; set; }
    }
}
