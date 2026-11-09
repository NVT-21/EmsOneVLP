using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class EmployeeListViewModel
    {
        public string ID { get; set; }
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
        public string LevelName { get; set; }
        public string IsLock { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int CountActiveTime { get; set; }
        public string GroupPermissionName { get; set; }
    }
}