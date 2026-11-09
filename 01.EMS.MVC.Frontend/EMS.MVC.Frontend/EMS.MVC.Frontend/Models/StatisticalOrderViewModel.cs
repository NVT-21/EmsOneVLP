using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class StatisticalOrderViewModel
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long Quantity { get; set; }
        public long TotalCOD { get; set; }
    }
}