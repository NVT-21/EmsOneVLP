using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class PostageRefundDataViewModel
    {
        [DataMember]
        public string CustomerCode { get; set; }

        [DataMember]
        public int MonthYear { get; set; }

        [DataMember]
        public List<PostageRefundDataDetailsViewModel> DataDetails { get; set; }

        [DataMember]
        public double SumMoneyforCollection { get; set; }
        [DataMember]
        public long SumCODFee { get; set; }
        [DataMember]
        public long SumCODFeeRefund { get; set; }
        [DataMember]
        public long SumFeeE1TransferRefund { get; set; }
        [DataMember]
        public long SumRefundAmount { get; set; }
    }
    [Serializable]
    [DataContract]
    public class PostageRefundDataDetailsViewModel
    {
        [DataMember]
        public int RowNumber { get; set; }

        [DataMember]
        public string CustomerCode { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public string RefundDate { get; set; }

        [DataMember]
        public string ReleaseDate { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public double MoneyforCollection { get; set; }

        [DataMember]
        public long CODFee { get; set; }

        [DataMember]
        public long DiscountRate { get; set; }

        [DataMember]
        public long CODFeeRefund { get; set; }

        [DataMember]
        public long RefundAmount { get; set; }

        [DataMember]
        public long FeeE1TransferRefund { get; set; }
    }
}