using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZConnectPageAddRequest
    {
        public string AccountID { get; set; }
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageAvatar { get; set; }
        public string PageCover { get; set; }
        public string PageCategory { get; set; }
        public string PageDescription { get; set; }
        public string PageCipher { get; set; }
        public string IsVerified { get; set; }
        public string AuthorizationCode { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserAccessToken { get; set; }
        public string PageAccessToken { get; set; }
        public string Channel { get; set; }
        public int ShopID { get; set; }
        public int CreatedBy { get; set; }
    }
}
