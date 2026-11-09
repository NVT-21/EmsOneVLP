using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class PrintSingleOrderViewModel
    {
        public string ShippingCodes { get; set; }
        public string CrmOrPayPostCode { get; set; }
        public string PrintType { get; set; }
    }
}