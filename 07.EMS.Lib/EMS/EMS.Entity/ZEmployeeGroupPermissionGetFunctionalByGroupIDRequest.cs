using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeGroupPermissionGetFunctionalByGroupIDRequest
    {
        public int GroupPermissionID { get; set; }
        public string IsShop { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class ZEmployeeGroupPermissionGetFunctionalByGroupIDResponse
    {
        public int ID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public FunctionalGroup FunctionalGroup { get; set; }
    }
}
