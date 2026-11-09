using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestOrderDashboardRequest
    {
        public int ShopID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ZRequestOrderDashboard
    {
        public int Status { get; set; }
    }

    public class ZRequestOrderDashboardResponse
    { 
        public string Lable { get; set; }
        public int Status { get; set; }
        public long Amount { get; set; }
        public double AmountPerCent { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
}
