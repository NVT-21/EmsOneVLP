using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTemplatePrintUpdateRoleRequest
    {
        public string strTemplatePrintID { get; set; }
        public string PrintType { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
