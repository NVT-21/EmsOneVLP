using System.Collections.Generic;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class RequestSupportViewModel
    {
        public int TypeDate { get; set; }
        public List<SelectListItem> ListTypeDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RequestSupportCode { get; set; }
        public string EShippingCode { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string RequestSupportType { get; set; }
        public string RequestSupportGroup { get; set; }
        public List<RequestSupportGroupModel> ListRequestSupportGroup { get; set; }
        public string RequestSupportStatus { get; set; }
        public List<SelectListItem> ListRequestSupportStatus { get; set; }
        
        
    }
}