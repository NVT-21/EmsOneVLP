using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZRequestSupportAddRequest
    {
        public string EShippingCode { get; set; }
        public int RequestGroupID { get; set; }
        public string RequestGroup { get; set; }
        public string RequestType { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string UpdateInformation { get; set; }
        public string Document { get; set; }
        public string Priority { get; set; }
        public string Origin { get; set; }
        public string ServiceGroup { get; set; }
        public string Scope { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
    public class ZUpdateInformation
    {
        public string MoneyCOD { get; set; }
        public string MobileNumber { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
    }
}
