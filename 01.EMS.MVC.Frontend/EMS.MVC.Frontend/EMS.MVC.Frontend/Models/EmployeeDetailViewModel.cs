using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class EmployeeDetailViewModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string EmployeeLevel { get; set; }
        public string IsLock { get; set; }
        public int CountActiveTime { get; set; }
        public string Description { get; set; }
        public string strGroupPermissionID { get; set; }
        public string ShopID { get; set; }
    }
}