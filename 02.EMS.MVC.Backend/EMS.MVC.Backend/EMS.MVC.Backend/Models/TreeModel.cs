using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace EMS.MVC.Backend.Models
{
    public class TreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; }
        public TreeStateModel state { get; set; }
        public TreeLiAttrModel li_attr {get;set;}
    }
    public class TreeStateModel
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
    public class TreeLiAttrModel
    {
        public string id { get; set; }
    }
}