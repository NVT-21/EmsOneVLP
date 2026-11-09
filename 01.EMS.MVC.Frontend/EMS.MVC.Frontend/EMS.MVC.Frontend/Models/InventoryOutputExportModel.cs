using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class InventoryOutputExportModel
    {
        public InventoryModel Inventory { get; set; }
        public List<InventoryDetailModel> ListInventoryDetail { get; set; }
    }
}