using System.Collections.Generic;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class AddCollectionPostOfficeViewModel
    {
        public int ParentID { get; set; }
        public string CollectionPostOfficeName { get; set; }
        public string CollectionPostOfficeCode { get; set; }
        public int CollectionPostOfficeType { get; set; }
        public List<SelectListItem> ListCollectionPostOfficeType { get; set; }
        public int ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int DistrictID { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public int WardID { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string Street { get; set; }
        public string RegionName { get; set; }
        public string IsLock { get; set; }
        public List<SelectListItem> ListIsLock { get; set; }
        public string Email { get; set; }
        public string UserCMS { get; set; }
    }
}