using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductVariantDataByProductIDViewModel
    {
        public int ProductID { get; set; }
        public List<ProductVariantModel> Variants { get; set; }
    }
}