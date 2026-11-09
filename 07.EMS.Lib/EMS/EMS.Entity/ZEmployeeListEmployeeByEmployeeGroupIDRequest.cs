using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeListEmployeeByEmployeeGroupIDRequest
    {
        public int EmployeeGroupID { get; set; }
        public string strEmployeeGroupID { get; set; }
        public int ShopID { get; set; }
    }

    public class ZEmployeeListEmployeeByEmployeeGroupIDViewRequest
    {
        public int EmployeeGroupID { get; set; }
        public string strEmployeeGroupID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZEmployeeListEmployeeByEmployeeGroupIDResponse
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
    }
}
