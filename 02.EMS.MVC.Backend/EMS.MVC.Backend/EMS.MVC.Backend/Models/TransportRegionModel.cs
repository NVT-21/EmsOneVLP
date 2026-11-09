using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportRegionModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string ProvincesID { get; set; }
        public string Description { get; set; }
    }
}