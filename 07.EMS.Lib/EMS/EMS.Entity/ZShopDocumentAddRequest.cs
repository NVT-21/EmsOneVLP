using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZShopDocumentEditRequest
    {
        public string DocumentCode { get; set; }
        public string FileName { get; set; }
        public int CreatedBy { get; set; }
        public int ShopID { get; set; }
    }
}
