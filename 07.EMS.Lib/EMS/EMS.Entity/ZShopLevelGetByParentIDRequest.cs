using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopLevelGetByParentIDRequest
    {
        public int ParentID { get; set; }
        public int ShopLevel { get; set; }
    }
    public class ZShopLevelGetByParentIDResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Description { get; set; }
        public string IsLock { get; set; }
        public int ShopLevel { get; set; }
        public string FullAddress { get; set; }
    }
}
