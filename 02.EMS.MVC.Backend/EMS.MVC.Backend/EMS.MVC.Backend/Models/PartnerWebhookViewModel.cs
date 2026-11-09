using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class PartnerWebhookViewModel
    {
        public int ID { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerURL { get; set; }
        public string PartnerKey { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
    }
}