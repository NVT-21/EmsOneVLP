using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPrepaidFreightPaymentRequest
    {
        public int ShopID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string EmsBarCode { get; set; }
        public string OrderCode { get; set; }
        public string Status { get; set; }
    }
    public class ZPrepaidFreightPaymentResponse
    {
        public string AmndDate { get; set; }
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
        public long SumAll { get; set; }
        public string Note { get; set; }
        public string IsAgree { get; set; }
    }
}
