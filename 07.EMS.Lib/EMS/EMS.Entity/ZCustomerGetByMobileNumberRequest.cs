using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerGetByMobileNumberRequest
    {
        public string MobileNumber { get; set; }
        public int ShopID { get; set; }
    }
    public class ZCustomerGetByMobileNumberResponse
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Channel { get; set; }
        public ZProvinceModel Province { get; set; }
        public ZDistrictModel District { get; set; }
        public ZWardModel Ward { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string BlackList { get; set; }
        public int SuccessOrder { get; set; }
        public int TotalOrder { get; set; }
        public ZCustomerGroupModel Group { get; set; }
        public int TypeShop { get; set; }
    }
}
