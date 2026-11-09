using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeShopAddRequest
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string strGroupPermissionID { get; set; }
        public string IsLock { get; set; }
        public string Description { get; set; }
        //public string PassWord { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ZEmployeeShopAddViewRequest
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string strGroupPermissionID { get; set; }
        public string IsLock { get; set; }
        public string Description { get; set; }
        //public string PassWord { get; set; }
        public string ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
}
