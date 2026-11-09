using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class OrderCheckboxModel
    {
        public string Key { get; set; }
        public int ProductID { get; set; }
        public int VariantID { get; set; }
    }
}