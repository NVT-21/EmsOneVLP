using System.Collections.Generic;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class EditCustomerViewModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int DistrictID { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public int WardID { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string Street { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Note { get; set; }
        public string Channel { get; set; }
        public List<SelectListItem> ListChannel { get; set; }
        public string CustomerStickerID { get; set; }
        public List<CustomerStickerModel> ListCustomerSticker { get; set; }
    }
}