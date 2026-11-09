using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeEditRequest
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string EmployeeLevel { get; set; }
        public string IsLock { get; set; }
        public int UpdatedBy { get; set; }
        public int GroupPermissionID { get; set; }
        public string CorporationID { get; set; }
        public string IsShop { get; set; }
    }
}
