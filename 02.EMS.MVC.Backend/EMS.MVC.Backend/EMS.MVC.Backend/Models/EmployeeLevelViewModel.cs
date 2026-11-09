using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class EmployeeLevelViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmployeeLevelCode { get; set; }
        public string IsLock { get; set; }
    }
}