using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    // COD FOLLOW
    [Serializable]
    [DataContract]
    public class ZReconciliationSearchListCODFollowRequest
    {
        [DataMember]
        public string TypeDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerPPCRM { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int CODTransType { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZReconciliationSearchListCODFollowResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerPPCRM { get; set; }
        [DataMember]
        public string ListCode { get; set; }
        [DataMember]
        public string EmsBarCode { get; set; }
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string CreatedDateOrder { get; set; }
        [DataMember]
        public string TimeRelease { get; set; }
        [DataMember]
        public string ApprovalDate { get; set; }
        [DataMember]
        public long CODMoney { get; set; }
        [DataMember]
        public string IsAgree { get; set; }
        [DataMember]
        public string IsPayment { get; set; }
        [DataMember]
        public string ReasonReject { get; set; }
        [DataMember]
        public string PeriodName { get; set; }
        [DataMember]
        public string CODTransTypeName { get; set; }
    }
}
