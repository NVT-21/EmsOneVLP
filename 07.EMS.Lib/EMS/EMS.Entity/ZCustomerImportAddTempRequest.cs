using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCustomerImportAddTempRequest
    {
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
        public string Sticker { get; set; }
        public string OrderSuccess { get; set; }
        public string PurchaseMoney { get; set; }
        public string IsReplace { get; set; }
        public int CreatedBy { get; set; }
        public string ImportCode { get; set; }
    }
}
