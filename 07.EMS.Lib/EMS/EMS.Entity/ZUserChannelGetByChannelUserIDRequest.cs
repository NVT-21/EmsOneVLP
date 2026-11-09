using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZUserChannelGetByChannelUserIDRequest
    {
        public string PageID { get; set; }
        public string ChannelUserID { get; set; }
        public string Channel { get; set; }
    }
    public class ZUserChannelGetByChannelUserIDResponse
    {
        public int UserChannelID { get; set; }
        public string ChannelUserID { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public ZProvinceModel Province { get; set; }
        public ZDistrictModel District { get; set; }
        public ZWardModel Ward { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string IsBlackList { get; set; }
    }
}
