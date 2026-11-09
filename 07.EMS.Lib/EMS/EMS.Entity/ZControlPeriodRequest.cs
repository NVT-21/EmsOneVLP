using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZControlPeriodRequest
    {
        public int ShopID { get; set; }
        public string Type { get; set; }
    }

    public class ZControlPeriodResponse
    {
        public int ID { get; set; }
        public string PeriodName { get; set; }
    }

    public class ZControlPeriodDateRequest
    {
        public int ShopID { get; set; }
        public int ID { get; set; }
    }

    public class ZControlPeriodDateResponse
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
