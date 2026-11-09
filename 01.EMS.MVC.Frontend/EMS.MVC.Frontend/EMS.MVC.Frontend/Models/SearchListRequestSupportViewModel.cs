using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListRequestSupportViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string EShippingCode { get; set; }
        public string RequestGroup { get; set; }
        public string RequestType { get; set; }
        public BaseModel Status { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
    }
}