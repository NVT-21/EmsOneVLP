using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProviderModel
    {
        public int ID { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string FullAddress { get; set; }
        public string TaxCode { get; set; }
        public string IsLock { get; set; }
    }
}