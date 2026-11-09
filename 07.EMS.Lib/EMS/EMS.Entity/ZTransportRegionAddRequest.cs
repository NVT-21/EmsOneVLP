using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportRegionAddRequest
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string ProvincesID { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
