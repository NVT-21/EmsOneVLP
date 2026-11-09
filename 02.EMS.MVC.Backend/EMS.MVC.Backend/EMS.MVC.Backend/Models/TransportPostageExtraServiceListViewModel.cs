using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageExtraServiceListViewModel
    {
        public int ID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string IsLock { get; set; }
        public string VAT { get; set; }
    }
}