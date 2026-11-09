using System.Collections.Generic;

namespace EMS.MVC.Frontend.Models
{
    public class EditWareHouseViewModel
    {
        public int WareHouseID { get; set; }
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public int ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int DistrictID { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public int WardID { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string Street { get; set; }
        public string IsDefault { get; set; }
        public string IsLock { get; set; }
    }
}