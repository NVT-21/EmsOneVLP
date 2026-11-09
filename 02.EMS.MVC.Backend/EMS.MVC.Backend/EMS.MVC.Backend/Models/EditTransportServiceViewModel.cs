using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EMS.MVC.Backend.Models
{
    public class EditTransportServiceViewModel
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; }
        public List<SelectListItem> ListTransportServiceClassification { get; set; }
        public int Type { get; set; }
        public List<SelectListItem> ListTransportServiceType { get; set; }
        public bool IsSpecial { get; set; }
        public string Description { get; set; }
    }
}