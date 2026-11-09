using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerAddNewRequest
    {
        public int ShopID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public string BirthDay { get; set; }
        public string TaxCode { get; set; }
        public string Channel { get; set; }
        public string Note { get; set; }
        public string strStickerID { get; set; }
        public string strStickerNew { get; set; }
        public int CreatedBy { get; set; }
        public string IDAccountChannel { get; set; }
        public string PageID { get; set; }
    }
    public class ZCustomerAddNewResponse
    {
        public int ID { get; set; }
    }
}
