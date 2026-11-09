using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZReconciliationSearchFeeReportRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CustomerPPCRM { get; set; }
        public string PostOffice { get; set; }
        public string TypeItem { get; set; }
    }
    public class ZReconciliationSearchFeeReportResponse
    {
        public string PostOffice { get; set; }
        public long TotalCustomerPPCRM { get; set; }
        public long TotalOrder { get; set; }
        public long TotalWeight { get; set; }
        public long TotalFee { get; set; }
        public string TypeItem { get; set; }
    }

    public class ZReconciliationSearchFeePostOfficeReportRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CustomerPPCRM { get; set; }
        public string PostOffice { get; set; }
        public string TypeItem { get; set; }
    }
    public class ZReconciliationSearchFeePostOfficeReportResponse
    {
        public string CustomerPPCRM { get; set; }
        public long TotalOrder { get; set; }
        public long TotalWeight { get; set; }
        public long TotalFee { get; set; }
        public string TypeItem { get; set; }
    }
}
