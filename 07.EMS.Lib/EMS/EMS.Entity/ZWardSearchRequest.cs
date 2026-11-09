using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWardSearchRequest
    {
        public string Name { get; set; }
        public int DistrictID { get; set; }
        public int VMapCode { get; set; }
    }
    public class ZWardSearchResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
