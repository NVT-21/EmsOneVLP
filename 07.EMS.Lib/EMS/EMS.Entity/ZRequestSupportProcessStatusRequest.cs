using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestSupportProcessStatusRequest
    {
        public int RequestSupportID { get; set; }
        public int ShopID { get; set; }
        public string ResultDocument { get; set; }
        public string ResultDescription { get; set; }
        public string ResultIndemnify { get; set; }
        public string ResultRequestSupport { get; set; }
        public int UpdatedBy { get; set; }
    }
}
