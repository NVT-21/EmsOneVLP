using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopDocumentDataByIDRequest
    {
        public int ShopID { get; set; }
    }
    public class ZShopDocumentDataByIDResponse
    {
        public string VerificationDocuments { get; set; }
        public string OtherDocuments { get; set; }
    }
}
