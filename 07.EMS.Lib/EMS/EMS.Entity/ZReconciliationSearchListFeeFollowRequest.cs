using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    // FEE FOLLOW
    public class ZReconciliationSearchListFeeFollowRequest
    {
        public string TypeDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPPCRM { get; set; }
        public string Status { get; set; }
    }
    public class ZReconciliationSearchListFeeFollowResponse
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPPCRM { get; set; }
        public string ListCode { get; set; }
        public string EmsBarCode { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string CreatedDateOrder { get; set; }
        public string TimeRelease { get; set; }
        public string ApprovalDate { get; set; }
        public long TempTransportFee { get; set; }
        public long TransportFee { get; set; }
        public string IsAgree { get; set; }
        public string IsPayment { get; set; }
        public string ReasonReject { get; set; }
        public string PeriodName { get; set; }
    }

    public class ZReconciliationSearchListFeeFollowResponseV2
    {
        public int ShopID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPPCRM { get; set; }
        public string EmsBarCode { get; set; }
        public string OrderCode { get; set; }
        public int OrderID { get; set; }
        public string TimeRelease { get; set; }
        public long TempTransportFee { get; set; }
        public long ServiceFee { get; set; }
        public long MainFee { get; set; }
        public long ReducedRates { get; set; }
        public long TransportFeePublic { get; set; }
        public long TransportFee { get; set; }
        public string TypeItem { get; set; }
    }
}
