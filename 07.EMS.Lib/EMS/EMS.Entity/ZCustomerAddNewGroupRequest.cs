using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerAddNewGroupRequest
    {
        public int ShopID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int OrderSuccess { get; set; }
        public int PurchaseMoney { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZCustomerAddNewGroupResponse
    {
        public int CreatedBy { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string OrderSuccess { get; set; }
        public string PurchaseMoney { get; set; }
    }
}
