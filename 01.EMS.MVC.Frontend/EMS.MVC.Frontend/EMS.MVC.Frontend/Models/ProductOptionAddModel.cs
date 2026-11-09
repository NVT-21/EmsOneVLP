using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductOptionAddModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}