using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopGetListCrmOrPaypostCodeRequest
    {
        public int ShopID { get; set; }
    }
    public class ZShopGetListCrmOrPaypostCodeResponse
    {
        public string StoreName { get; set; }
        public string MobileNumber { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string MobileAndCrmOrPaypostCode { get; set; }
    }
}
