using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class StatisticalCashflowViewModel
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long TotalCOD { get; set; }
        public long TotalTransportFee { get; set; }
    }
}