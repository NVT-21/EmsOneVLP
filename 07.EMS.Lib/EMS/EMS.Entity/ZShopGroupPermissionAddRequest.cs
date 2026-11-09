using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopGroupPermissionAddRequest
    {
        public string GroupPermissionCode { get; set; }
        public string GroupPermissionName { get; set; }
        public int ShopLevel { get; set; }
        public string IsLock { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
    }
}
