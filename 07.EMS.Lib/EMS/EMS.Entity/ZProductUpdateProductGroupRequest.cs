using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProductUpdateProductGroupRequest
    {
        public string IDs { get; set; }
        public int ProductGroupID { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
    public class ZProductUpdateProductGroupResponse
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
    }
}
