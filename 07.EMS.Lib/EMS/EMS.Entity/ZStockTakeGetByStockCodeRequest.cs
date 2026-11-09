using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStockTakeGetByStockCodeRequest
    {
        public string StockTakeCode { get; set; }
        public int ShopID { get; set; }
    }
    public class ZStockTakeGetByStockCodeResponse
    {
        public int ID { get; set; }
        public string StockTakeCode { get; set; }
        public int WareHouseID { get; set; }
        public string Note { get; set; }
        public string CreatedName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedName { get; set; }
        public string UpdatedDate { get; set; }
        public List<ZStockTakeDetailGetByStockCodeResponse> ListStockTakeDetail { get; set; }
    }
    public class ZStockTakeDetailGetByStockCodeResponse
    {
        public int ProductID { get; set; }
        public int ProductVariantID { get; set; }
        public string ProductVariantSKU { get; set; }
        public int NumberOfProduct { get; set; }
        public int NumberOfInventory { get; set; }
        public int NumberOfActual { get; set; }
        public int NumberOfDifferent { get; set; }
        public int MoneyOfDifferent { get; set; }
    }
}
