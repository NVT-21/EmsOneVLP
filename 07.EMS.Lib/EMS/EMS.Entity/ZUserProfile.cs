using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZUserProfile
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int ShopID { get; set; }
        public int GroupPermissionID { get; set; }
        public string IsLock { get; set; }
        public string EmployeeLevel { get; set; }
        public string StoreOwner { get; set; }
        public string StoreName { get; set; }
        public string Code { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int ShopLevel { get; set; }
    }
}
