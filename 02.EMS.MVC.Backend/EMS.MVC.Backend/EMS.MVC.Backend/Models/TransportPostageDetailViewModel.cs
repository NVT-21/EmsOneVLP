using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageDetailViewModel
    {
        public int PostageID { get; set; }
        public string PostageCode { get; set; }
        public string PostageName { get; set; }
        public int PostageRange { get; set; }
    }
}