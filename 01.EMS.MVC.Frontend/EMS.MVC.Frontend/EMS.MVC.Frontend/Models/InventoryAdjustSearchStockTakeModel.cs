using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class InventoryAdjustSearchStockTakeModel
    {
        [DataMember]
        public int WarehouseID { get; set; }
        [DataMember]
        public string StockTakeCode { get; set; }
        [DataMember]
        public int TypeDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
    }
}