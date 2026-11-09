using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWardGetsRequest
    {
    }
    public class ZWardGetsResponse
    {
        public int ProvinceID { get; set; }
        public string ProvinceTitle { get; set; }
        public string ProvinceName { get; set; }

        public int DistrictID { get; set; }
        public string DistrictTitle { get; set; }
        public string DistrictName { get; set; }

        public int WardID { get; set; }
        public string WardTitle { get; set; }
        public string WardName { get; set; }
    }
}
