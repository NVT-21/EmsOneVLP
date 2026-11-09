using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPaypostEditCODMoneyRequest
    {
        public string ParcelCode { get; set; }
        public double Amount { get; set; }
        public string PoCode { get; set; }
    }
    public class ZPaypostEditCODMoneyResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}
