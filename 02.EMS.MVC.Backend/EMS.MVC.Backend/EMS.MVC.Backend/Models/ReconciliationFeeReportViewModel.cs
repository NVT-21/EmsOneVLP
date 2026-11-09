using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ReconciliationFeeReportViewModel
    {
        public string PostOffice { get; set; }
        public long TotalCustomerPPCRM { get; set; }
        public long TotalOrder { get; set; }
        public long TotalWeight { get; set; }
        public long TotalFee { get; set; }
        public string TypeItemName { get; set; }
    }
}