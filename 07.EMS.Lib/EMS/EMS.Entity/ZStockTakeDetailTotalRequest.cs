using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStockTakeDetailTotalRequest
    {
    }
    public class ZStockTakeDetailTotalResponse
    {
        public int TotalProduct { get; set; }
        public int TotalInventory { get; set; }
        public int TotalActual { get; set; }
        public int TotalDifferent { get; set; }
        public int TotalMoneyDifferent { get; set; }
    }
}
