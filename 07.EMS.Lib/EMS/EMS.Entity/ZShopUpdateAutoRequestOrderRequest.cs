using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopUpdateAutoRequestOrderRequest
    {
        public int ID { get; set; }
        public string AutoDivision { get; set; }
        public string EmployeeReadyDivision { get; set; }
        public string EmployeeGroupPermissionID { get; set; }
        public int DateProcessRequestOrder { get; set; }
        public int Criteria { get; set; }
        public string TimeDivision { get; set; }
        public int UpdatedBy { get; set; }
    }
}
