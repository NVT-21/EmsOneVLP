using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListWareHouseViewModel
    {
        public int ID { get; set; }
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string Address { get; set; }
        public string EmployeeName { get; set; }
        public string IsDefault { get; set; }
        public string FullAddress { get; set; }
        public string IsLock { get; set; }
    }
}