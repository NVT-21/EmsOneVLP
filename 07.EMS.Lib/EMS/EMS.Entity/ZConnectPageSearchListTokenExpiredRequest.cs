using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZConnectPageSearchListTokenExpiredRequest
    {
        public string Channel { get; set; }
    }
    public class ZConnectPageSearchListTokenExpiredResponse
    {
        public int ID { get; set; }
        public string PageID { get; set; }
        public string RefreshToken { get; set; }
        public int ShopID { get; set; }
    }
}
