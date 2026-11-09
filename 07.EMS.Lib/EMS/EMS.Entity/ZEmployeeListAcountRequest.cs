using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeListAcountRequest
    {
        public string MobileNumber { get; set; }
    }
    public class ZEmployeeListAcountResponse
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public int ShopID { get; set; }
        public string StoreName { get; set; }
    }
}
