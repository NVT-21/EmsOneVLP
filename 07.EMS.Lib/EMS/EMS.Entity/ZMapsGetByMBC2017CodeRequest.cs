using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZMapsGetByMBC2017CodeRequest
    {
        public string ProvinceMBC2017Code { get; set; }
        public string DistrictMBC2017Code { get; set; }
        public string WardMBC2017Code { get; set; }
        public string Street { get; set; }
    }
    public class ZMapsGetByMBC2017CodeResponse
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
