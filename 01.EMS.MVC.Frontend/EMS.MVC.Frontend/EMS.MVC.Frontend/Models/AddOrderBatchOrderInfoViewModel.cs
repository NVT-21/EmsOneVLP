using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class AddOrderBatchOrderInfoViewModel
    {
        public string OrderCode { get; set; }
        public string OrderName { get; set; }
        public int OrderQuantity { get; set; }
        public long OrderValue { get; set; }
        public long TransportFee { get; set; }
        public long TotalAmount { get; set; }
        public long TotalDiscount { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double ExchangeWeight { get; set; }
        public long TotalPHU { get; set; }
        public string Message { get; set; }
        public List<ProductInfoViewModel> ListProductInfo { get; set; } = new List<ProductInfoViewModel>();
        public int NumberOfDetail { get; set; }
    }
}