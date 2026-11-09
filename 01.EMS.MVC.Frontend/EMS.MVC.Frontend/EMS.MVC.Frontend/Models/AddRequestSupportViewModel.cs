using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using System.Web.Mvc;
namespace EMS.MVC.Frontend.Models
{
    public class AddRequestSupportViewModel
    {
        public string EShippingCode { get; set; }
        public long TotalCOD { get; set; }
        public CustomerModel Sender { get; set; }
        public CustomerModel Receiver { get; set; }
        public string RequestSupportGroup { get; set; }
        public List<ZRequestSupportGroupListResponse> ListRequestSupportGroup { get; set; }
        public string RequestSupportType { get; set; }
        public List<ZRequestSupportGroupListResponse> ListRequestSupportType { get; set; }
        public string RequestSupportTypeDifferent { get; set; }
        public string RequestSupportServiceGroup { get; set; }
        public List<SelectListItem> ListRequestSupportServiceGroup { get; set; }
        public string RequestSupportScope { get; set; }
        public List<SelectListItem> ListRequestSupportScope { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Document { get; set; }
        public string FileName { get; set; }
        public int ShopID { get; set; }
        public string Priority { get; set; }
        public string Origin { get; set; }
        public string ServiceGroup { get; set; }
        public string Scope { get; set; }
        public string UpdateInformation { get; set; }
    }
}