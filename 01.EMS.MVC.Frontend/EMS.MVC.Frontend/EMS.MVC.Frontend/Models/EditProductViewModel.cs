using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class EditProductViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int ProductGroupID { get; set; }
        public List<ProductGroupModel> ListProductGroup { get; set; }
        public long PricesSell { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedByFullName { get; set; }
        public int Unit { get; set; }
        public List<ProductUnitModel> ListProductUnit { get; set; }
    }
}