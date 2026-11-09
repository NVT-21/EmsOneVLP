using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductUnitAddModel
    {
        public string UnitName { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
}