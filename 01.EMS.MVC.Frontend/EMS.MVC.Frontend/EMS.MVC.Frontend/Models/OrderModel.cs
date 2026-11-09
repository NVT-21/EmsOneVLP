using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public string ShippingCode { get; set; }
        public string EMSOneCode { get; set; }
        public string OrderCode { get; set; }
        public string BatchCode { get; set; }
        public string IsTransport { get; set; }
        public BaseModel OrderStatus { get; set; }
    }
}