using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductOptionDataByProductIDViewModel
    {
        public List<XProductOptionModel> Option01 { get; set; }
        public List<XProductOptionModel> Option02 { get; set; }
        public List<XProductOptionModel> Option03 { get; set; }
    }
}