using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerEditRequest
    {
        public int ID { get; set; }
        public int ShopID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Channel { get; set; }
        public string Note { get; set; }
        public string strStickerID { get; set; }
        public string strStickerNew { get; set; }
        public int UpdatedBy { get; set; }
        public string BirthDate { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
    }
}
