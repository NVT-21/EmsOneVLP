using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageMainFeeModel
    {
        public int PostageID { get; set; }
        public int MainFeeRange { get; set; }
        public string MainFeeVAT { get; set; }
        public string Status { get; set; }
        public List<SelectListItem> RangeList { get; set; }
    }
}