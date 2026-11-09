using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class InventoryAdjustSearchStockTakeViewModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedName { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
    }
}