using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class FunctionalModel
    {
        public int ID { get; set; }
        public string FunctionalCode { get; set; }
        public string FunctionalName { get; set; }
        public int FunctionalLevel { get; set; }
        public int FunctionalParentID { get; set; }
        public string FunctionalTitle { get; set; }
        public int IsValue { get; set; }
        public string MenuIcon { get; set; }
        public int IsType { get; set; }
        public List<FunctionalModel> ListFunctional { get; set; }
    }
}