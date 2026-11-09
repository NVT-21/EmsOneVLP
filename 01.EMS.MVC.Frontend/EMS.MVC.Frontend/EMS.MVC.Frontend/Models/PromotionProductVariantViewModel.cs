using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class PromotionProductVariantViewModel
    {
        public int VariantID { get; set; }
        public string VariantName { get; set; }
        public string VariantImage { get; set; }
        public string ProductName { get; set; }
    }
}