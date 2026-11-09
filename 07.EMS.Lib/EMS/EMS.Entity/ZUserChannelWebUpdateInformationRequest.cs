using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZUserChannelWebUpdateInformationRequest
    {
        public int UserChannelID { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public string Channel { get; set; }
        public string isUpdatedCustomer { get; set; }
        public string IsBlackList { get; set; }
        public int ShopID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
