using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloUpdateTokenRequest
    {
        public string OfficialAccountID { get; set; }
        public int ShopID { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
