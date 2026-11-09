using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class TransportFeeViewModel
    {
        public int MainTransportServiceID { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportServiceID { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public int FromProvinceID { get; set; }
        public int ToProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int ToDistrictID { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public int ToWardID { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public long OrderValue { get; set; }
        public long TotalCOD { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public long Quantity { get; set; }
    }
}