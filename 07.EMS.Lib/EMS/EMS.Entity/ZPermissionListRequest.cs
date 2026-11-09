using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPermissionListRequest
    {
        public int ShopLevel { get; set; }
        public string IsLock { get; set; }
        public string IsShop { get; set; }
    }
    public class ZPermissionListResponse
    {
        public int ID { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public int ShopLevel { get; set; }
        public string Description { get; set; }
    }
}
