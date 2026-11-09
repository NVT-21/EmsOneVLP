using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestOrderDivisionAgainRequest
    {
        public int ShopID { get; set; }
        public int EmployeeID { get; set; }
        public int RequestOrderID { get; set; }
        public int[] arrEmployeeID { get; set; }
        public int[] arrRequestOrderID { get; set; }
        public int NumberOfEmployeeOrder { get; set; }
        public int UpdatedBy { get; set; }
    }
}
