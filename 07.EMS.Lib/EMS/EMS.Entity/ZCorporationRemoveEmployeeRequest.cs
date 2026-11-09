using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCorporationRemoveEmployeeRequest
    {
        public int CorporationID { get; set; }
        public int EmployeeID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
