using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZDistrictSearchRequest
    {
        public string Name { get; set; }
        public int ProvinceID { get; set; }
        public int VMapCode { get; set; }
    }
    public class ZDistrictSearchResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
