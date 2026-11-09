using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class StatisticalDeliveryTransportViewModel
    {
        public string CreatedDate { get; set; }
        public string StatusName_G { get; set; }
        public string StatusName_F { get; set; }
        public string Status_G { get; set; }
        public string Status_F { get; set; }
        public long TotalOrder_G { get; set; }
        public long TotalOrder_F { get; set; }
        public long TotalCOD_G { get; set; }
        public long TotalCOD_F { get; set; }
    }
}