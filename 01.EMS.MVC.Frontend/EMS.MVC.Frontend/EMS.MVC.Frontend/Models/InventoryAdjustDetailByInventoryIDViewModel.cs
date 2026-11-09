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
    public class InventoryAdjustDetailByInventoryIDViewModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public XProductVariantModel ProductVariant { get; set; }
        [DataMember]
        public long Quantity { get; set; }
        [DataMember]
        public long StockTakeQuantity { get; set; }
        [DataMember]
        public long WarehouseQuantity { get; set; }
        [DataMember]
        public long QuantityAdjustment { get; set; }
        [DataMember]
        public long QuantityAfterAdjustment { get; set; }
        [DataMember]
        public long InventoryQuantity { get; set; }
    }
}