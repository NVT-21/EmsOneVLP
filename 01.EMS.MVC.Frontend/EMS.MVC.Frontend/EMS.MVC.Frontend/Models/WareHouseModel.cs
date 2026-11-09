using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class WareHouseModel
    {
        public int ID { get; set; }
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public ProvinceModel Province { get; set; }
        public DistrictModel District { get; set; }
        public WardModel Ward { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
        public string CreatedName { get; set; }
        public string IsDefault { get; set; }
        public string IsLock { get; set; }
    }
}