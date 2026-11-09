using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XStockTakeModel : XBaseModel
    {
        public string CreaterName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdaterName { get; set; }
        public string UpdatedDate { get; set; }
    }
}
