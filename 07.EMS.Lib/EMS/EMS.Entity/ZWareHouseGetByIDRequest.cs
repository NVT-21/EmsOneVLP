using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWareHouseGetByIDRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZWareHouseGetByIDResponse
    {
        public int WareHouseID { get; set; }
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public int WardID { get; set; }
        public string WardName { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
        public string IsDefault { get; set; }
        public string IsLock { get; set; }
    }
}
