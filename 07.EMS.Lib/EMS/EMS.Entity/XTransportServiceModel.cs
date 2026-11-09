using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XTransportServiceModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string SpecialService { get; set; }
        public string IsLock { get; set; }
        public int Sort { get; set; }
        public string IsDefault { get; set; }
        public int CreatedBy { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
    }
}
