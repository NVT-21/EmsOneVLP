using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloConnectRequest
    {
        public string Code { get; set; }
        public string OfficialAccountID { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZZaloConnectResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
