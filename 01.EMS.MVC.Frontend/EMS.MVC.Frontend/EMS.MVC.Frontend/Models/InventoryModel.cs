using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class InventoryModel
    {
        public int ID { get; set; }
        public string InventoryCode { get; set; }
        public string ReceiverName { get; set; }
        public string ImporterName { get; set; }
        public string MobileNumber { get; set; }
        public string DateOfInventoryImport { get; set; }
        public string Note { get; set; }
        public string Reason { get; set; }
        public WareHouseModel WareHouse { get; set; }
        public ProviderModel Provider { get; set; }
        public BaseModel Status { get; set; }
        public long TotalAmount { get; set; }
        public string TotalAmountByText { get; set; }
        public ShopModel Shop { get; set; }
        public OrderModel Order { get; set; }
    }
}