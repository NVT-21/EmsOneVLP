using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopSalesChannelAddRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Channel { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
}
