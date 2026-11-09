using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class PickUpAddressModel
    {
        public int PickUpAddressID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string PickUpName { get; set; }
        public ProvinceModel Province { get; set; }
        public DistrictModel District { get; set; }
        public WardModel Ward { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
        public string IsDefault { get; set; }
        public int QuantityLabelPrint { get; set; }
        public int CollectionPostOfficeID { get; set; }
    }
}