using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class BranchInformationViewModel
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string StoreName { get; set; }
        public string MobileNumber { get; set; }
        public string Description { get; set; }
        public string IsLock { get; set; }
        public int PickUpID { get; set; }
        public int PickUpProvinceID { get; set; }
        public int PickUpDistrictID { get; set; }
        public int PickUpWardID { get; set; }
        public string PickUpAddress { get; set; }
        public string PickUpFullAddress { get; set; }
        public int ShopLevel { get; set; }
        public List<PickUpAddressModel> ListPickUpAddress { get; set; }
    }
}