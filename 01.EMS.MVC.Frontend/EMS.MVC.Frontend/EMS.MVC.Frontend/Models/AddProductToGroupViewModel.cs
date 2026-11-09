using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class AddProductToGroupViewModel
    {
        public int ProductGroupID { get; set; }
        public List<ProductGroupModel> ListProductGroup { get; set; }
        public string ListProductID { get; set; }
    }
}