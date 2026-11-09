using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopImagesByShopIDRequest
    {
        public int ShopID { get; set; }
    }
    public class ZShopImagesByShopIDReponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ZImageModel> Images { get; set; }
    }
}
