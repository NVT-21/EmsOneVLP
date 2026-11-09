using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class SearchListFreightChargesViewModel
    {
        public int TransportPostageID { get; set; }
        public string TransportPostageCode { get; set; }
        public string TransportPostageName { get; set; }
        public string TransportServiceCode { get; set; }
        public string TransportServiceName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string IsLock { get; set; }
        public BaseModel Status { get; set; }
        public BaseModel VAT { get; set; }
        public string ListCrmOrPaypostCode { get; set; }
    }
}