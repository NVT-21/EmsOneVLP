using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestOrderAcceptRequest
    {
        public int RequestOrderID { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int ShopID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public string Channel { get; set; }
        public int CreatedBy { get; set; }

    }
}
