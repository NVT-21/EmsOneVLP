using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopResetPasswordShopRequest
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int UpdatedBy { get; set; }
    }
}
