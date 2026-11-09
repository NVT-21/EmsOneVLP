using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageSurchargeModel
    {
        public int PostageID { get; set; }
        public string PostageCode { get; set; }
        public string PostageName { get; set; }
        public string SurchargeVAT { get; set; }
        public string Status { get; set; }
        public ZTransportPostageGetByTransportPostageIDFuelResponse Fuel { get; set; } //Xăng dầu
        public ZTransportPostageGetByTransportPostageIDRemoteAreaResponse RemoteArea { get; set; } //Vùng xa
        public ZTransportPostageGetByTransportPostageIDIslandsAreaResponse IslandsArea { get; set; } //Vùng hải đảo
    }
}