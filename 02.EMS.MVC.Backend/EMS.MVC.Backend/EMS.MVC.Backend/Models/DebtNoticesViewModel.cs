using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class DebtNoticesViewModel
    {
        [DataMember]
        public string CustomerCode { get; set; }

        [DataMember]
        public int MonthYear { get; set; }

        [DataMember]
        public DebtNoticesDataInfo DataInfo { get; set; }

        [DataMember]
        public List<DebtNoticesDataInfoDetails> DataInfoDetails { get; set; }
        [DataMember]
        public long SumMainFee { get; set; }
        [DataMember]
        public long SumCODFee { get; set; }
        [DataMember]
        public long SumServiceFee { get; set; }
        [DataMember]
        public long SumAdjustmentFee { get; set; }
        [DataMember]
        public long SumTotalCharge { get; set; }
    }
    [Serializable]
    [DataContract]
    public class DebtNoticesDataInfo
    {
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Payments { get; set; }
        [DataMember]
        public string TaxCode { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string District { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string PostOfficeAccountantName { get; set; }
        [DataMember]
        public string PaymentAddress { get; set; }
        [DataMember]
        public string PostOfficeAccountantPhone { get; set; }
        [DataMember]
        public long ReturnRate { get; set; }
        [DataMember]
        public long AgreementRate { get; set; }
        [DataMember]
        public long EcommerceRate { get; set; }
        [DataMember]
        public long OtherCharges { get; set; }
        [DataMember]
        public long RefundRevenue { get; set; }
        [DataMember]
        public long ParcelRevenue { get; set; }
        [DataMember]
        public long RegularParcelRevenue { get; set; }
        [DataMember]
        public long RegisteredParcelRevenue { get; set; }
        [DataMember]
        public long AdjustmentFeeRevenue { get; set; }
        [DataMember]
        public long OtherAdjustmentChargesRevenue { get; set; }
        [DataMember]
        public long ArrearsRevenue { get; set; }
        [DataMember]
        public long EMSExtraServiceRevenue { get; set; }
        [DataMember]
        public long PromotionCVRevenue { get; set; }
        [DataMember]
        public long InternationalPriorityRevenue { get; set; }
        [DataMember]
        public long DomesticRevenue { get; set; }
        [DataMember]
        public long InternationalRevenue { get; set; }
        [DataMember]
        public long PermuteRevenue { get; set; }
        [DataMember]
        public long NegotiableRevenue { get; set; }
        [DataMember]
        public long EcommerceRevenue { get; set; }
        [DataMember]
        public long DomesticRate { get; set; }
        [DataMember]
        public long InternationalRate { get; set; }
        [DataMember]
        public long PriorityRate { get; set; }
        [DataMember]
        public string InvoiceNumber { get; set; }
    }
    [Serializable]
    [DataContract]
    public class DebtNoticesDataInfoDetails
    {
        [DataMember]
        public int RowNumber { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public long ConversionVolume { get; set; }
        [DataMember]
        public string Internal { get; set; }
        [DataMember]
        public string Receiver { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string ReleaseDate { get; set; }
        [DataMember]
        public string PostOfficeReturn { get; set; }
        [DataMember]
        public long Weight { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public long MainFee { get; set; }
        [DataMember]
        public long CODFee { get; set; }
        [DataMember]
        public long ServiceFee { get; set; }
        [DataMember]
        public string ReceiverAddress { get; set; }
        [DataMember]
        public string ReceiverTel { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public long Value { get; set; }
        [DataMember]
        public string ReferenceNumber { get; set; }
        [DataMember]
        public long AdjustmentFee { get; set; }
        [DataMember]
        public string PostOfficeExploitCode { get; set; }
        [DataMember]
        public string PostOfficeExploit { get; set; }
    }
}