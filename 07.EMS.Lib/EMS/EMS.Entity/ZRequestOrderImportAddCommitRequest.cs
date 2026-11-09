using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestOrderImportAddCommitRequest
    {
        public int ShopID { get; set; }
        public string ImportCode { get; set; }
        public int CreatedBy { get; set; }
        public string Criteria { get; set; }
        public int Quantity { get; set; }
        public int EmployeeID { get; set; }
        public int NumberOfEmployeeOrder { get; set; }
        public int TotalRowExcel { get; set; }
        public string strEmployeeID { get; set; }
        public string IsDivideEmployee { get; set; }
    }
}
