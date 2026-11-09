using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZGetReceiverInforRequest
    {
        public string CustomerCode { get; set; } = string.Empty;
        public List<string> ItemCodes { get; set; } = new List<string>();
        public string Channel { get; set; } = string.Empty;
        public string TokenKey { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }

    public class ZReceiverInforDCSModel
    {
        public string ItemCode { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverAddress { get; set; } = string.Empty;
    }
}
