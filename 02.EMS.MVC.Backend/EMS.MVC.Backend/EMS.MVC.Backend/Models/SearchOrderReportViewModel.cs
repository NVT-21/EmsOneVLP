using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class SearchOrderReportViewModel
    {
        [DataMember]
        public long SumOrderMonthQuantity { get; set; }
        [DataMember]
        public long SumOrderMonthTurnover { get; set; }
        [DataMember]
        public List<SearchOrderReportDataViewModel> Data { get; set; }

    }
    [Serializable]
    [DataContract]
    public class SearchOrderReportDataViewModel
    {
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ConfirmDate { get; set; }
        [DataMember]
        public string AreaName { get; set; }
        [DataMember]
        public int OrderMonthQuantity { get; set; }
        [DataMember]
        public long OrderMonthTurnover { get; set; }
        [DataMember]
        public int PartnerID { get; set; }
        [DataMember]
        public string PartnerName { get; set; }
    }
}