using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeGetByGroupPermissionIDRequest
    {
        public int ShopID { get; set; }
        public int GroupPermissionID { get; set; }
    }
    public class ZEmployeeGetByGroupPermissionIDResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
