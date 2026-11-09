using EMS.Entity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class AddProviderViewModel
    {

        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int DistrictID { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public int WardID { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string Note { get; set; }
        public List<SelectListItem> ListIsLock { get; set; }
        public string IsLock { get; set; }
    }
}