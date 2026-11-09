using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZLabelPrintAddRequest
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Street { get; set; }
        public string IsDefault { get; set; }
        public int PickUpAddressID { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
}
