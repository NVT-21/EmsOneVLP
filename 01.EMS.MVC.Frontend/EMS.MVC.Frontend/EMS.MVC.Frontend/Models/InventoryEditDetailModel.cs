using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class InventoryEditDetailModel
    {
        public int DetailID { get; set; }
        public int InventoryID { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
    }
}