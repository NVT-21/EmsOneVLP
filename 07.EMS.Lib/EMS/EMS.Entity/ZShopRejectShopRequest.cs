using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopRejectShopRequest
    {
        public int ID { get; set; }
        public int ConfirmedBy { get; set; }
        public string ReasonReject { get; set; }
    }
}
