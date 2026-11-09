using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductVariantAddViewModel
    {
        public int ProductID { get; set; }
        public List<ProductOptionNameModel> ProductOptions { get; set; }
    }
    public class ProductOptionNameModel
    {
        public string Name { get; set; }
    }
}