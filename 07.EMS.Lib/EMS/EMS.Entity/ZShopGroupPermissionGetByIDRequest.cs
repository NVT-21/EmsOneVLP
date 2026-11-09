using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopGroupPermissionGetByIDRequest
    {
        public int ID { get; set; }
        public int NumberOfEmployeesInGroup { get; set; }
    }
    public class ZShopGroupPermissionGetByIDResponse
    {
        public int ID { get; set; }
        public string GroupPermissionCode { get; set; }
        public string GroupPermissionName { get; set; }
        public int ShopLevel { get; set; }
        public string IsLock { get; set; }
        public string Description { get; set; }
    }
}
