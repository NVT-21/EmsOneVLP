using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class EditOrderReceiverAddressViewModel
    {
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string BatchCode { get; set; }
        public int ProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int DistrictID { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }
        public int WardID { get; set; }
        public List<WardModel> ListWard { get; set; }
        public string Street { get; set; }
    }
}