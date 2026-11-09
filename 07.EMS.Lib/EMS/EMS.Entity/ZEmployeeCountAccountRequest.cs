using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeCountAccountRequest
    {
        public string MobileNumber { get; set; }
        public string Password { get; set; }
    }
    public class ZEmployeeCountAccountResponse
    {
        public int CountAccount { get; set; }
        public int ShopID { get; set; }
    }
}
