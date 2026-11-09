using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class RequestSupportViewModel
    {
        public int TypeDate { get; set; }
        public string AllDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchKey { get; set; }
        public string RequestSupportGroup { get; set; }
        public List<ZRequestSupportGroupListResponse> ListRequestSupportGroup { get; set; } 
        public string RequestSupportType { get; set; }
        public List<ZRequestSupportGroupListResponse> ListRequestSupportType { get; set; } 
        public string RequestSupportStatus { get; set; }
        public List<SelectListItem> ListRequestSupportStatus { get; set; }
    }
}