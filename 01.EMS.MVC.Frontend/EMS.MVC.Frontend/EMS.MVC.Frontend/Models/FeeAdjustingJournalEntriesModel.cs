using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class FeeAdjustingJournalEntriesModel
    {
        public string CustomerPPCRM { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AdjustingPostOffice { get; set; }
    }
}