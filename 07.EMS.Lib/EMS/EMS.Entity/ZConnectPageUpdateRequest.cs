using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZConnectPageUpdateRequest
    {
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageAvatar { get; set; }
        public string PageCover { get; set; }
        public string PageCategory { get; set; }
        public string PageDescription { get; set; }
        public string IsVerified { get; set; }
        public string IsActive { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Channel { get; set; }
        public int ShopID { get; set; }
    }
}
