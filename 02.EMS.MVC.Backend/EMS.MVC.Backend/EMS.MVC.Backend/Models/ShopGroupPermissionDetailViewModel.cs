using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ShopGroupPermissionDetailViewModel
    {
        public int ID { get; set; }
        public string GroupPermissionCode { get; set; }
        public string GroupPermissionName { get; set; }
        public int ShopLevel { get; set; }
        public string IsLock { get; set; }
        public string Description { get; set; }
        public int NumberOfEmployeesInGroup { get; set; }
    }
}