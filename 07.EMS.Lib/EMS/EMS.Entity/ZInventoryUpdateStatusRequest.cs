using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZInventoryUpdateStatusRequest
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public string Discription { get; set; }
        public int UpdatedBy { get; set; }
        public int ShopID { get; set; }
    }
}
