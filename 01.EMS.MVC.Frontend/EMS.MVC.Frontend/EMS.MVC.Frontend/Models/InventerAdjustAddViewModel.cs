using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class InventerAdjustAddViewModel
    {
        [DataMember]
        public List<ZWareHouseSearchListResponse> WarehouseList { get; set; }
        [DataMember]
        public string AllDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
    }
}