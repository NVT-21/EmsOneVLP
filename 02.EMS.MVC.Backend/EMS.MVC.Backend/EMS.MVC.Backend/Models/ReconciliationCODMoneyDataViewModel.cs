using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class ReconciliationCODMoneyDataViewModel
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
        public string StatusIsAgree { get; set; }
        [DataMember]
        public string StatusIsPayment { get; set; }
        [DataMember]
        public string ReasonReject { get; set; }
        [DataMember]
        public string PeriodName { get; set; }
        [DataMember]
        public string CODTransTypeName { get; set; }
    }
}