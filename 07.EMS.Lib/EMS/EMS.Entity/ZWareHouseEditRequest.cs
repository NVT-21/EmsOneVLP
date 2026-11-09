using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWareHouseEditRequest
    {
        public int WareHouseID { get; set; }
        public string WareHouseName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Street { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
        public string IsDefault { get; set; }
        public string IsWareHouse { get; set; }
        public string IsLock { get; set; }
    }
}
