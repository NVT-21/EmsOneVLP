using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XOrderModel : XBaseModel
    {
        public string EMSOneCode { get; set; }
        public string OrderCode { get; set; }
        public string Transport { get; set; }
        public string ReceiverName { get; set; }
    }
}
