using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopLoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class ZShopLoginResponse
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ProductBusiness { get; set; }
        public string TypeBusiness { get; set; }
        public string IsLock { get; set; }
        public string Status { get; set; }
        public string Expired { get; set; }
    }
}
