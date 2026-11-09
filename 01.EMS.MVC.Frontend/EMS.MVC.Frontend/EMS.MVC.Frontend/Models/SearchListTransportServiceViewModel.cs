using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListTransportServiceViewModel
    {        
        public List<TransportServiceModel> ListTransportService { get; set; }
        public int Type { get; set; }
        public string ServiceID { get; set; }
    }
}