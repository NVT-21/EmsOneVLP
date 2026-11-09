using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerStickerListRequest
    {
        public string StickerCode { get; set; }
        public string StickerName { get; set; }
        public string IsLock { get; set; }
        public int ShopID { get; set; }
    }
    public class ZCustomerStickerListResponse
    {
        public int ID { get; set; }
        public string StickerCode { get; set; }
        public string StickerName { get; set; }
        public string CreatedDate { get; set; }
    }
}
