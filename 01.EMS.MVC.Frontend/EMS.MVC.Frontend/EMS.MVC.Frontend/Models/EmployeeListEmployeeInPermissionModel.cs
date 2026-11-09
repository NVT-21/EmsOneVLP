using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class EmployeeListEmployeeInPermissionModel
    {
        public int EmployeeGroupPermissionID { get; set; }
        public string EmployeeGroupPermissionName { get; set; }
        public List<ZEmployeeListEmployeeInPermissionResponse> ListEmployee { get; set; }
    }
}