using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPermissionChangeRequest
    {
        public int ShopID { get; set; }
        public int GroupPermissionID { get; set; }
        public string strFunctionalID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
