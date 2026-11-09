using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopGroupPermissionListRequest
    {
        public int ShopLevel { get; set; }
        public string IsShop { get; set; }
        public string IsLock { get; set; }
    }
    public class ZShopGroupPermissionListResponse
    {
        public int ID { get; set; }
        public string GroupPermissionCode { get; set; }
        public string GroupPermissionName { get; set; }
        public int ShopLevel { get; set; }
        public string Description { get; set; }
        public string IsLock { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int NumberOfEmployeesInGroup { get; set; }
    }
}
