using System.Collections.Generic;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class EditBasicInfoTabViewModel
    {
        public int TransportPostageID { get; set; }
        public string TransportPostageCode { get; set; }
        public string TransportPostageName { get; set; }
        public int TransportPostageType { get; set; }
        public List<SelectListItem> ListTransportPostageType { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int MainTransportServiceID { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportServiceID { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public int ProvinceApply { get; set; }
        public List<SelectListItem> ListProvinceApply { get; set; }
        public int Area { get; set; }
        public int AreaApply { get; set; }
        public List<SelectListItem> ListAreaApply { get; set; }
        public int SubjectOfApplication { get; set; }
        public List<SelectListItem> ListSubjectOfApplication { get; set; }
        public string ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        //public string ApplicationObjectValue { get; set; }
        public string AppliesToShop { get; set; }
        public string AppliesToProvinceFrom { get; set; }
        public string Status { get; set; }
    }
}