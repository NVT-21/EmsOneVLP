using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ShowInfoPickUpAddressViewModel
    {
        public int PickUpAddressID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string PickUpName { get; set; }
        public int CollectionPostOfficeID { get; set; }
        public List<CollectionPostOfficeModel> ListCollectionPostOffice { get; set; }
        public int ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int DistrictID { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public int WardID { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
        public int ShopID { get; set; }
        public string IsDefault { get; set; }
    }
}