using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPaymentListCODRequest
    {
        public string CustomerCode { get; set; }
        public string CustomerPPCRM { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string ApprovalDate { get; set; }
        public string OrderCode { get; set; }
        public string EmsBarCode { get; set; }
    }

    public class ZPaymentListCODResponse
    {
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPPCRM { get; set; }
        public int OrderID { get; set; }
        public int ShopID { get; set; }
        public string OrderCode { get; set; }
        public string EmsBarCode { get; set; }
        public long CODMoney { get; set; }
        public string IsAgree { get; set; }
        public string IsPayment { get; set; }
    }

    public class ZPaymentUpdateCODRequest
    {
        public int ID { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZPaymentUpdateCODResponse
    {
        public string EMSBarCode { get; set; }
    }
}
