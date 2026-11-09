using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class AutoRequestOrderViewModel
    {
        public string AutoDivision { get; set; }
        public string EmployeeReadyDivision { get; set; }
        public string EmployeeGroupPermissionID { get; set; }
        public int DateProcessRequestOrder { get; set; }
        public int Criteria { get; set; }
        public string TimeDivision { get; set; }

        public List<ZShopGroupPermissionListResponse> ListGroupPermission { get; set; }
    }
}