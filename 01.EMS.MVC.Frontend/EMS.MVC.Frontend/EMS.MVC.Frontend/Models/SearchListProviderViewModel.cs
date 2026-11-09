using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListProviderViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string TaxCode { get; set; }
        public string FullAddress { get; set; }
        public string UpdatedDate { get; set; }
        public string IsLock { get; set; }
    }
}