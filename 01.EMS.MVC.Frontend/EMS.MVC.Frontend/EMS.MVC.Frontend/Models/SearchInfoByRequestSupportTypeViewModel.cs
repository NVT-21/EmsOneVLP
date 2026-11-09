using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchInfoByRequestSupportTypeViewModel
    {
        public string RequestSupportType { get; set; }
        public long TotalCOD { get; set; }
        public CustomerModel Sender { get; set; }
        public CustomerModel Receiver { get; set; }
    }
}