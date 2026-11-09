using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPermissionMenuListRequest
    {
        public int ShopID { get; set; }
        public int EmployeeID { get; set; }
    }
    public class ZPermissionMenuListResponse: ZFunctionalListResponse
    {
        
    }
}
