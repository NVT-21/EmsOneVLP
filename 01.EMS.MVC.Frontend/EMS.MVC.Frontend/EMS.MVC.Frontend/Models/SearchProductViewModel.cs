using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class SearchProductViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public List<ZVariantResponse> Variants { get; set; }
    }
}