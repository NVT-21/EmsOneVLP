using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class TemporaryTransportServiceFeeViewModel
    {
        public long MainServiceTotalAmount { get; set; }
        public long ExtraServiceTotalAmount { get; set; }
        public long TotalAmount { get; set; }
        public string Message { get; set; }
        public string ShowView { get; set; }
    }
}