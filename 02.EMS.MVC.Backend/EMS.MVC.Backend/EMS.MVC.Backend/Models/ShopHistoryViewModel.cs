using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ShopHistoryViewModel
    {
        public string ActionName { get; set; }
        public string ActionCode { get; set; }
        public string Description { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
    }
}