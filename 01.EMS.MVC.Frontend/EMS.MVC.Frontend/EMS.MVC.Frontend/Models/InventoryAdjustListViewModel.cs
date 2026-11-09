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
    public class InventoryAdjustListViewModel : XBaseModel
    {
        [DataMember]
        public XWarehouseModel Warehouse { get; set; }
        [DataMember]
        public XStockTakeModel StockTake { get; set; }
        [DataMember]
        public XStatusModel Status { get; set; }
        [DataMember]
        public string ImplementationDate { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public string CreaterName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdaterName { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
    }
}