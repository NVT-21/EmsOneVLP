using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class StatisticalProductViewModel
    {
        public List<StatisticalProductSelling> ProductSelling { get; set; }
        public List<StatisticalSearchProductRefund> ProductRefund { get; set; }
    }
    public class StatisticalProductSelling
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long TotalProduct { get; set; }
        public long TotalCOD { get; set; }
    }
    public class StatisticalSearchProductRefund
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long TotalProduct { get; set; }
        public long TotalCOD { get; set; }
    }
}