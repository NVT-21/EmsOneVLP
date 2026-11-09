using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageBasicInfoModel
    {
        public int PostageID { get; set; }
        public string PostageCode { get; set; }
        public string PostageName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int MainServiceID { get; set; }
        public string ExtraServiceIDs { get; set; }
        public int Area { get; set; }
        public int ProvinceApply { get; set; }
        public int AreaApply { get; set; }
        public int SubjectsOfApplication { get; set; }
        public string ApplicationObjectValue { get; set; }
        public string Status { get; set; }
    }
}