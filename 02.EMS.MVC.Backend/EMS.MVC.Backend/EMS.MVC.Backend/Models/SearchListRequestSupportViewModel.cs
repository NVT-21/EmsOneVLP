using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Backend.Models
{
    public class SearchListRequestSupportViewModel
    {
        public int RequestSupportID { get; set; }
        public string RequestSupportCode { get; set; }
        public string EShippingCode { get; set; }
        public BaseModel RequestSupportType { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public BaseModel RequestSupportStatus { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }

        public int ShopID { get; set; }
        public string CrmOrPaypostCode { get; set; }

        public BaseModel RequestSupportGroup { get; set; }

        public string SearchKey { get; set; }
    }
}