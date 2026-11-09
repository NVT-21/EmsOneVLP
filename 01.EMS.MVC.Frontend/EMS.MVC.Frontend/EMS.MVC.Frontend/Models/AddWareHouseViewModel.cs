using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class AddWareHouseViewModel
    {
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Street { get; set; }
        public int CreatedBy { get; set; }
        public int ShopID { get; set; }
        public int ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int DistrictID { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public int WardID { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string IsDefault { get; set; }
    }
}