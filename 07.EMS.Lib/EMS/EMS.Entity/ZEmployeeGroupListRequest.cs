using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZEmployeeGroupListRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string IsLock { get; set; }
        public int ShopID { get; set; }
    }
    public class ZEmployeeGroupListResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsLock { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
