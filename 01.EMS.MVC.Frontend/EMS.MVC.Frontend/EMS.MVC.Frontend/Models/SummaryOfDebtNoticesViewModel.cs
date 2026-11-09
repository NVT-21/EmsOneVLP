using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class SummaryOfDebtNoticesViewModel
    {
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public int MonthYear { get; set; }
        [DataMember]
        public SummaryOfDebtNoticesDataInfoModel DataInfo { get; set; }
        [DataMember]
        public SummaryOfDebtNoticesDataInfoDetailsModel DataInfoDetails { get; set; }
        [DataMember]
        public string TotalRevenueToText { get; set; }
    }

    [Serializable]
    [DataContract]
    public class SummaryOfDebtNoticesDataInfoModel
    {
        [DataMember]
        public string BranchNameMenu { get; set; }
        [DataMember]
        public string CustomerServiceCenterNameMenu { get; set; }
        [DataMember]
        public string PaymentInformationMenu { get; set; }
    }

    [Serializable]
    [DataContract]
    public class SummaryOfDebtNoticesDataInfoDetailsModel
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerCode { get; set; }

        [DataMember]
        public string PaymentPostCode { get; set; }

        [DataMember]
        public string AccountantPhone { get; set; }

        [DataMember]
        public string AccountantName { get; set; }

        [DataMember]
        public string PaymentPostAddress { get; set; }

        [DataMember]
        public string InvoiceNumber { get; set; }

        [DataMember]
        public List<SummaryOfDebtNoticesDataInfoDetailsItemModel> ListItem { get; set; }
    }

    [Serializable]
    [DataContract]
    public class SummaryOfDebtNoticesDataInfoDetailsItemModel
    {
        [DataMember]
        public int RowIndex { get; set; }
        [DataMember]
        public string NumRow { get; set; }
        [DataMember]
        public string Explain { get; set; }
        [DataMember]
        public long NumberOfParcels { get; set; }
        [DataMember]
        public long FeeBeforeVAT { get; set; }
        [DataMember]
        public long FeeVAT { get; set; }
        [DataMember]
        public long FeeAfterVAT { get; set; }
    }
}