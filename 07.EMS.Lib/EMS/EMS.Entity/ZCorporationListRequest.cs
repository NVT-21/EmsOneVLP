using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCorporationListRequest
    {
        public string IsLock { get; set; }
        public int ShopID { get; set; }
    }
    public class ZCorporationListResponse
    {
        public int ID { get; set; }
        public string CorporationName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string Address { get; set; }
        public string EmployeeName { get; set; }
        public string IsLock { get; set; }
        public string CreatedDate { get; set; }
    }
}
