using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class RejectInventoryOutputViewModel
    {
        public int InventoryOutputID { get; set; }
        public string Reason { get; set; }
        public int UpdatedBy { get; set; }
        public int ShopID { get; set; }
    }
}