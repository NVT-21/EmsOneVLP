using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ProductGroupViewModel
    {
        public string ProductGroupCode { get; set; }
        public string ProductGroupName { get; set; }
        public List<ProductGroupModel> ListProductGroup { get; set; }
        public string SearchKey { get; set; }
    }
}