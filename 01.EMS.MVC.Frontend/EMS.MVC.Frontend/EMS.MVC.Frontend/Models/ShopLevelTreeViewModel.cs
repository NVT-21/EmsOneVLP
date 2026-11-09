using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ShopLevelTreeViewModel
    {
        public int ParentID { get; set; }
        public string Name { get; set; }
        public int ShopLevel { get; set; }
    }
}