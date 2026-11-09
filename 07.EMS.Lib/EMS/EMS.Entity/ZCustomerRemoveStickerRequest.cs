using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerRemoveStickerRequest
    {
        public int ID { get; set; }
        public string StickerName { get; set; }
        public int ShopID { get; set; }
    }
}
