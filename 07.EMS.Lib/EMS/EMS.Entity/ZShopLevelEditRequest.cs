using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopLevelEditRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string NameAddress { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string IsLock { get; set; }
        public int UpdatedBy { get; set; }
        public int PickupAddress { get; set; }
    }

    public class ZShopLevelEditViewRequest
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string NameAddress { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string IsLock { get; set; }
        public int UpdatedBy { get; set; }
        public int PickupAddress { get; set; }
    }
}
