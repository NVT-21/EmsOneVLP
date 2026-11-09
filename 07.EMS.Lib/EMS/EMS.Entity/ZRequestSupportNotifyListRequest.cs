using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestSupportNotifyListRequest
    {
        public int MaxRequestSupportID { get; set; }
        public int UserID { get; set; }
    }
    public class ZRequestSupportNotifyListResponse
    {
        public int MaxRequestSupportID { get; set; }
        public int CountRequestSupport { get; set; }
        public List<ZRequestSupportNotifyListDetail> ListRequestSupportNotify { get; set; }
    }
    public class ZRequestSupportNotifyListDetail
    {
        public int ID { get; set; }
        public string RequestSupportCode { get; set; }
        public string EShippingCode { get; set; }
        public int ShopID { get; set; }
        public string CreatedDate { get; set; }
    }
}
