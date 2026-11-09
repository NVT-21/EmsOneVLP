using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class StatusProcessAddModel
    {
        public int ShopID { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
    }
}