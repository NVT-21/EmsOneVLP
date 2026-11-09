using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZOrderChangeStatusRequest
    {
        public int ID { get; set; }
        public string BeforeStatus { get; set; }
        public string AfterStatus { get; set; }
        public string ReasonCancel { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZOrderCheckTransportRequest
    {
        public int ShopID { get; set; }
        public string OrderCode { get; set; }
    }
}
