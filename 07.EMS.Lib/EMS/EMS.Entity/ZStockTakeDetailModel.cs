using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStockTakeDetailModel
    {
        public int ID { get; set; }
        public string StockTakeCode { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductVariantID { get; set; }
        public string ProductVariantName { get; set; }
        public string ProductVariantSKU { get; set; }
        public long ProductVariantPrice { get; set; }
        public long NumberOfInventory { get; set; }
        public long NumberOfActual { get; set; }
        public long NumberOfDifferent { get; set; }
        public long MoneyOfDifferent { get; set; }
    }
}
