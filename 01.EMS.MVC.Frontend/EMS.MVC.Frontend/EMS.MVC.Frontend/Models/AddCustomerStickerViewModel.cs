using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class AddCustomerStickerViewModel
    {
        public string CustomerID { get; set; }
        public string StickerCode { get; set; }
        public string StickerName { get; set; }
        public List<CustomerModel> ListCustomer { get; set; }
    }
}