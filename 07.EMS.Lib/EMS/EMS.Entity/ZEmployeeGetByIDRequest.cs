using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeGetByIDRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public int ShopLevel { get; set; }
    }

    public class ZEmployeeGetByIDViewRequest
    {
        public string ID { get; set; }
        public string ShopID { get; set; }
        public int ShopLevel { get; set; }
    }
    public class ZEmployeeGetByIDResponse
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string EmployeeLevel { get; set; }
        public string IsLock { get; set; }
        public int CountActiveTime { get; set; }
        public string Description { get; set; }
        public string strGroupPermissionID { get; set; }
        public int ShopID { get; set; }
    }
}
