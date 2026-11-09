using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListProductGroupViewModel
    {
        public int ProductGroupID { get; set; }
        public string ProductGroupCode { get; set; }
        public string ProductGroupName { get; set; }
        public string Description { get; set; }
        public string EmployeeName { get; set; }
        public string CreatedDate { get; set; }
        public int CountProduct { get; set; }
        public float TotalPriceProduct { get; set; }
        public string SearchKey { get; set; }
    }
}