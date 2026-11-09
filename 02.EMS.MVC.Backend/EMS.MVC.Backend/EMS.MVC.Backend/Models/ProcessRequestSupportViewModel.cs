using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class ProcessRequestSupportViewModel
    {
        public int RequestSupportID { get; set; }
        public int ShopID { get; set; }
        public string ResultDocument { get; set; }
        public string ResultDescription { get; set; }
        public string ResultIndemnify { get; set; }
        public string ResultRequestSupport { get; set; }
        public List<SelectListItem> ListResultRequestSupport { get; set; }
    }
}