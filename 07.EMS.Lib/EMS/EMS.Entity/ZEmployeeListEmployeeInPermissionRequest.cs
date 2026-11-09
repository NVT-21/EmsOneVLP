using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeListEmployeeInPermissionRequest
    {
        public int ShopID { get; set; }
        public int ShopLevel { get; set; }
    }
    public class ZEmployeeListEmployeeInPermissionResponse
    {
        public int EmployeeGroupPermissionID { get; set; }
        public string EmployeeGroupPermissionName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeMobileNumber { get; set; }
        public List<ZEmployeeListEmployeeInPermissionResponse> ListEmployee { get; set; }
    }
}
