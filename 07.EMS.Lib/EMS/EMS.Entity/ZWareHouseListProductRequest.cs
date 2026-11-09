using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWareHouseListProductRequest
    {
        public int ShopID { get; set; }
    }
    public class ZWareHouseListProductResponse
    {
        public int ID { get; set; }
        public string WareHouseCode { get; set; }
        public string WareHouseName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
