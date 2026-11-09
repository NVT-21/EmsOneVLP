using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageExtraServiceAddViewModel
    {
        public List<SelectListItem> ListChargingConditions { get; set; }
        public List<SelectListItem> MainServiceList { get; set; }
        public List<SelectListItem> ExtraServiceList { get; set; }
    }
}