using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int ProviderID { get; set; }
        public int ShopID { get; set; }
        public int ProductGroupID { get; set; }
        public string SearchKey { get; set; }
    }
}