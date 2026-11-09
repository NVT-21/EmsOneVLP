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
    public class ZOrderSuccessHistoryRequest
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZOrderSuccessHistoryResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public long TotalProductValue { get; set; }
        [DataMember]
        public XStatusModel Status { get; set; }
        [DataMember]
        public string CreatedDate  { get; set; }
    }
}
