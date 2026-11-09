using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCorporationGetByIDRequest
    {
        public int ID { get; set; }
    }

    public class ZCorporationGetByIDResponse
    {
        public int ID { get; set; }
        public string CorporationName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string IsLock { get; set; }
    }
}
