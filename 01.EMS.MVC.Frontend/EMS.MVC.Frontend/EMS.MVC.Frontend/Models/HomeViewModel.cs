using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class HomeViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public List<ZShopGetListCrmOrPaypostCodeResponse> ListCrmOrPaypostCode { get; set; }
        public List<FunctionalModel> ListTab { get; set; }
    }
}