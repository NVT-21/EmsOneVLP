using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class TransportPostageCommonModel
    {
        public int PostageID { get; set; }
        public string PostageName { get; set; }
        public int GroupID { get; set; }
        public int RegionGroupID { get; set; }
        public int ParentID { get; set; }
        public List<SelectListItem> ProvincesList { get; set; }
        public List<SelectListItem> RegionList { get; set; }
    }
}