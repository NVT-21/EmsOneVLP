using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZMapsGetByVMapCodeRequest
    {
        public int ProvinceVMapCode { get; set; }
        public int DistrictVMapCode { get; set; }
        public int WardVMapCode { get; set; }
        public string StreetVMapCode { get; set; }
    }
    public class ZMapsGetByVMapCodeResponse
    {
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public int WardID { get; set; }
        public string WardName { get; set; }
        public string Street { get; set; }
    }
}
