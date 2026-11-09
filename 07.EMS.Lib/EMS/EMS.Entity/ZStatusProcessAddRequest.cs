using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStatusProcessAddRequest
    {
        public int ShopID { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
    }
}
