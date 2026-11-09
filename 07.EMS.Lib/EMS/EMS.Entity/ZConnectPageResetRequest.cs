using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZConnectPageResetRequest
    {
        public int ShopID { get; set; }
        public string Channel { get; set; }
        public string AccountID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
