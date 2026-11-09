using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class AddInventoryAdjustModel
    {
        [DataMember]
        public int WarehouseID { get; set; }
        [DataMember]
        public string ImplementationDate { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int StockTakeID { get; set; }
    }
}