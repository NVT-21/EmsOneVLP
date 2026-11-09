using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class TemplatePrintViewModel
    {
        public string PrintTypeDefault { get; set; }
        public string PrintType { get; set; }
        public List<SelectListItem> ListPrintType { get; set; }
        public List<LabelPrintModel> ListLabelPrint { get; set; }
    }
}