using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    // Fee Daily
    public class ZReconciliationSearchListFeeRequest
    {
        public int ShopID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
        public string ListCode { get; set; }
        public string EmsBarCode { get; set; }
        public string OrderCode { get; set; }
        public string SearchCode { get; set; }
        public string TypeItem { get; set; }
    }
    public class ZReconciliationSearchListFeeResponse
    {
        public string AmndDate { get; set; }
        public string CustomerCode { get; set; }
        public string ListCode { get; set; }
        public string EmsBarCode { get; set; }
        public string TimeRelease { get; set; }
        public string AdditionService { get; set; }
        public string SpecialService { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public long TempTransportFee { get; set; }
        public string CreatedDateOrder { get; set; }
        public string ReceiveProvice { get; set; }
        public int ProvisionalVolume { get; set; }
        public int ChargeableVolume { get; set; }
        public long ServiceFee { get; set; }
        public long MainFee { get; set; }
        public long ReducedRates { get; set; }
        public long TransportFee { get; set; }
        public long TransportFeePublic { get; set; }
        public long FeePublic { get; set; }
        public string TypeItem { get; set; }
    }
    // Fee Period
    public class ZReconciliationSearchListFeePeriodResponse
    {
        public int ID { get; set; }
        public string EmsBarCode { get; set; }
        public string TimeRelease { get; set; }
        public string ReceiveProvice { get; set; }
        public int ProvisionalVolume { get; set; }
        public int ChargeableVolume { get; set; }
        public long ServiceFee { get; set; }
        public long MainFee { get; set; }
        public long ReducedRates { get; set; }
        public long TransportFee { get; set; }
        public string Note { get; set; }
        public string IsAgree { get; set; }
        public string IsPayment { get; set; }
        public long TransportFeePublic { get; set; }
        public long FeePublic { get; set; }
        public string ApprovalDate { get; set; }
        public string ReasonReject { get; set; }
        public string PeriodName { get; set; }
    }
    public class ZReconciliationUpdateAgreeFeeRequest
    {
        public int ID { get; set; }
        public string Agree { get; set; }
        public string ReasonReject { get; set; }
        public int UpdatedBy { get; set; }
    }
    // Fee TradeDiscounts
    public class ZReconciliationSearchListFeeTradeDiscountsResponse
    {
        public int ID { get; set; }
        public string EmsBarCode { get; set; }
        public string TimeRelease { get; set; }
        public string ReceiveProvice { get; set; }
        public int ProvisionalVolume { get; set; }
        public int ChargeableVolume { get; set; }
        public long CODFee { get; set; }
        public long ServiceFee { get; set; }
        public long MainFee { get; set; }
        public long ReducedRates { get; set; }
        public long TransportFee { get; set; }
        public string Note { get; set; }
        public long TransportFeePublic { get; set; }
        public long FeePublic { get; set; }
    }
    // AdjustingJournalEntries
    [Serializable]
    [DataContract]
    public class ZReconciliationFeeAdjustingJournalEntriesRequest
    {
        [DataMember]
        public string CustomerPPCRM { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string AdjustingPostOffice { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZReconciliationFeeAdjustingJournalEntriesResponse
    {
        [DataMember]
        public int AdjustingID { get; set; }
        [DataMember]
        public string AdjustingPostOffice { get; set; }
        [DataMember]
        public string AdjustingDate { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string TimeRelease { get; set; }
        [DataMember]
        public int ProvisionalVolume { get; set; }
        [DataMember]
        public int ChargeableVolume { get; set; }
        [DataMember]
        public long MainFee { get; set; }
        [DataMember]
        public long ServiceFee { get; set; }
        [DataMember]
        public long RefundFee { get; set; }
        [DataMember]
        public long TransportFeePublic { get; set; }
        [DataMember]
        public long FeePublic { get; set; }
        [DataMember]
        public string Reason { get; set; }
    }
    // ListDetailEditedItemCode
    [Serializable]
    [DataContract]
    public class ZReconciliationListDetailEditedItemCodeRequest
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string PostOffice { get; set; }
        [DataMember]
        public string CustomerPPCRM { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZReconciliationListDetailEditedItemCodeResponse
    {
        [DataMember]
        public int NumRow { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public List<DetailEditedItemCode> ItemCodeDetail { get; set; }
        [DataMember]
        public long AdjBalanceProvisionalVolume { get; set; }
        [DataMember]
        public long AdjBalanceChargeableVolume { get; set; }
        [DataMember]
        public long AdjBalanceMainFee { get; set; }
        [DataMember]
        public long AdjBalanceServiceFee { get; set; }
        [DataMember]
        public long AdjBalanceTransportFeePublic { get; set; }
        [DataMember]
        public long AdjBalanceFeePublic { get; set; }
    }
    [Serializable]
    [DataContract]
    public class DetailEditedItemCode
    {
        [DataMember]
        public int AdjustingID { get; set; }
        [DataMember]
        public string AdjustingDate { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string TimeRelease { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string AdjustingPostOffice { get; set; }
        [DataMember]
        public int ProvisionalVolume { get; set; }
        [DataMember]
        public int ChargeableVolume { get; set; }
        [DataMember]
        public long MainFee { get; set; }
        [DataMember]
        public long ServiceFee { get; set; }
        [DataMember]
        public long TransportFeePublic { get; set; }
        [DataMember]
        public long FeePublic { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public int IsRelease { get; set; }
    }
}
