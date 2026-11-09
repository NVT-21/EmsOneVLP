using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class StatisticalCustomerViewModel
    {
        public List<StatisticalCustomerSelling> CustomerSelling { get; set; }
        public List<StatisticalCustomerRefund> CustomerRefund { get; set; }
    }

    public class StatisticalCustomerSelling
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalCOD { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    public class StatisticalCustomerRefund
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalCOD { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
}