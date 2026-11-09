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
    public class EditInventoryAdjustViewModel: XBaseModel
    {
        [DataMember]
        public XWarehouseModel Warehouse { get; set; }
        [DataMember]
        public List<ZWareHouseSearchListResponse> WarehouseList { get; set; }
        [DataMember]
        public XStockTakeModel StockTake { get; set; }
        [DataMember]
        public string ImplementationDate { get; set; }
        [DataMember]
        public string Note { get; set; }
    }
}