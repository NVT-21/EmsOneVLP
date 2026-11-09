using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOrderUpdateAmndStateRequest
    {
        public int OrderID { get; set; }
        public string AmndState { get; set; }
        public string IsRequestOrder { get; set; }
        public int RequestOrderID { get; set; }
        public string IsUpdatedCustomer { get; set; }
        public int CustomerID { get; set; }
        public string CustomerFullName { get; set; }
        public int CustomerProvinceID { get; set; }
        public int CustomerDistrictID { get; set; }
        public int CustomerWardID { get; set; }
        public string CustomerAddress { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
