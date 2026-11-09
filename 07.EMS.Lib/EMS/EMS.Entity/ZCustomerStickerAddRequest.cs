using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerStickerAddRequest
    {
        public string StickerCode { get; set; }
        public string StickerName { get; set; }
        public string strCustomerID { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
}
