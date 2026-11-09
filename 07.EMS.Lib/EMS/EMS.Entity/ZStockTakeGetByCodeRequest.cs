using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStockTakeGetByCodeRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZStockTakeGetByCodeResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int WareHouseID { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public List<ZStockTakeDetailModel> ListStockTakeDetail { get; set; }
    }
}
