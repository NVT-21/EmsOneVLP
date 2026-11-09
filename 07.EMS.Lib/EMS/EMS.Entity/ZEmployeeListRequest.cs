using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeListRequest
    {
        public int ShopID { get; set; }
        public string IsShop { get; set; }
        public int EmployeeID { get; set; }
    }
    public class ZEmployeeListResponse
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string LevelName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string EmployeeLevel { get; set; }
        public string IsLock { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ShopID { get; set; }
        public int CountActiveTime { get; set; }
        public string GroupPermissionName { get; set; }
    }
}
