using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ShowListErrorMultipleSelectModel
    {
        public string OrderCode { get; set; }
        public string EmsBarCode { get; set; }
        public string RequestSupportCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
    }
}