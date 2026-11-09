using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFunctionalCheckRequest
    {
        public int EmployeeID { get; set; }
        public int ShopID { get; set; }
        public string FunctionalTitle { get; set; }
    }
}
