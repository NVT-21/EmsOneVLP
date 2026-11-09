using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class SearchCustomerViewModel
    {
        public CustomerModel Buyer { get; set; }
        public CustomerModel Receiver { get; set; }
        public CustomerModel Sender { get; set; }
        public int TypeShop { get; set; }
        public string IsUpdateReceiver { get; set; }
        public int PickUpAddressID { get; set; }
        public List<PickUpAddressModel> ListPickUpAddress { get; set; }
        public string IsTransport { get; set; }
        public int CollectionPostOfficeRecentID { get; set; }
    }
}