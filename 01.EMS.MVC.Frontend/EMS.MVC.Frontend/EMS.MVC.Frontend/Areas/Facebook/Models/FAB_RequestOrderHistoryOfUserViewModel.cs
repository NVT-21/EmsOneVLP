using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    public class FAB_RequestOrderHistoryOfUserViewModel
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string Product { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
    }
}