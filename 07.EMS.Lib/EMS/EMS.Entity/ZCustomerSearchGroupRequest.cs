using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerSearchGroupRequest
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int ShopID { get; set; }
    }

    public class ZCustomerSearchGroupResponse
    {
        public int ID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int OrderSuccess { get; set; }
        public int PurchaseMoney { get; set; }
    }
}
