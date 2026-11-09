using EMS.Entity;
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
    public class EditDetailInventoryAdjustViewModel
    {
        [DataMember]
        public int InventoryID { get; set; }
        [DataMember]
        public string InventoryCode { get; set; }
        [DataMember]
        public List<SelectListItem> ProductVariantList { get; set; }
        [DataMember]
        public XWarehouseModel Warehouse { get; set; }
        [DataMember]
        public XStatusModel Status { get; set; }
        [DataMember]
        public string ImplementationDate { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public XStockTakeModel StockTake { get; set; }
        [DataMember]
        public string CreaterName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}