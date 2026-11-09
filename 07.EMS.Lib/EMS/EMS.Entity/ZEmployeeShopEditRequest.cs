using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeShopEditRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public string FullName { get; set; }
        public string strGroupPermissionID { get; set; }
        public string IsLock { get; set; }
        public string Description { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZEmployeeShopEditViewRequest
    {
        public string ID { get; set; }
        public string ShopID { get; set; }
        public string FullName { get; set; }
        public string strGroupPermissionID { get; set; }
        public string IsLock { get; set; }
        public string Description { get; set; }
        public int UpdatedBy { get; set; }
    }
}
