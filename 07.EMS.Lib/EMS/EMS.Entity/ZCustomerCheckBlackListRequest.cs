using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerCheckBlackListRequest
    {
        public int ID { get; set; }
        public string IsChecked { get; set; }
        public int UpdatedBy { get; set; }
    }
}
