using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZProviderImportAddCommitRequest
    {
        public string ImportCode { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
}
