using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class AddProductViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
        public string Size { get; set; }
        public int Weight { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Images { get; set; }
        public string BatchNumber { get; set; }
        public List<ProductVariantModel> ListVariant { get; set; }
        public int Unit { get; set; }
        public List<ProductUnitModel> ListProductUnit { get; set; }
        public int ProductGroupID { get; set; }
        public List<ProductGroupModel> ListProductGroup { get; set; }
    }
}