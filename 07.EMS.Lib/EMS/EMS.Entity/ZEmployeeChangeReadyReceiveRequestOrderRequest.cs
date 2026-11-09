using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeChangeReadyReceiveRequestOrderRequest
    {
        public int EmployeeID { get; set; }
        public string IsReady { get; set; }
        public int ShopID { get; set; }
    }
}
