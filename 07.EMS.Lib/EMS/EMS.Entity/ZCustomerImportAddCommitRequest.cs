using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerImportAddCommitRequest
    {
        public int ShopID { get; set; }
        public string ImportCode { get; set; }
        public int CreatedBy { get; set; }
    }
}
