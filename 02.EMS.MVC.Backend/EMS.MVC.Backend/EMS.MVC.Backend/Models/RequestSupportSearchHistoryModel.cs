using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class RequestSupportSearchHistoryModel
    {
        public string Description { get; set; }
        public string ResultDocument { get; set; }
        public string Reason { get; set; }
        public BaseModel Status { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
    }
}