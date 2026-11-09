using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class AddEmployeeGroupViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string EmployeeID { get; set; }
        public List<ZEmployeeListResponse> ListEmployee { get; set; }
        public string IsLock { get; set; }
        public List<SelectListItem> ListIsLock { get; set; }
        public string Description { get; set; }
    }
}