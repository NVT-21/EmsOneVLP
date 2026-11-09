using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestOrderDivisionRequest
    {
        public int ShopID { get; set; }
        public string Criteria { get; set; }
        public int Quantity { get; set; }
        public string strEmployeeID { get; set; }
        public int EmployeeID { get; set; }
        public int NumberOfEmployeeOrder { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ZRequestOrderDivisionViewRequest
    {
        public string ShopID { get; set; }
        public string Criteria { get; set; }
        public int Quantity { get; set; }
        public string strEmployeeID { get; set; }
        public int EmployeeID { get; set; }
        public int NumberOfEmployeeOrder { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
