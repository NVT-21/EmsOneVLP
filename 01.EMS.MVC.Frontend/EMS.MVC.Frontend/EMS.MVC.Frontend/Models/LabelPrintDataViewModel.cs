using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class LabelPrintDataViewModel
    {
        public string PrintType { get; set; }
        public string SenderFullName { get; set; }
        public string SenderFullAddress { get; set; }
        public string SenderDistrictName { get; set; }
        public string SenderProvinceName { get; set; }
        public string SenderMobileNumber { get; set; }
    }
}