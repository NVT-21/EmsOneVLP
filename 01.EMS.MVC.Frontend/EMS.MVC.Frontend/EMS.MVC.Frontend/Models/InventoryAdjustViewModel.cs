using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class InventoryAdjustViewModel
    {
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public List<WareHouseModel> ListWareHouse { get; set; }
        [DataMember]
        public string AllDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string DateType { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public List<SelectListItem> ListInventoryAjustStatus { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
    }
}