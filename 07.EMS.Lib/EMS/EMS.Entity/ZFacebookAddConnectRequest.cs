using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAddConnectRequest
    {
        public string FBID { get; set; }
        public int ShopID { get; set; }
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageCategory { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookDeleteConnectRequest
    {
        public string PageID { get; set; }
        public int ShopID { get; set; }
        public string Channel { get; set; }
        public int UpdatedBy { get; set; }
    }
    public class ZFacebookConnectPageGetsResponse
    {
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string PageCategory { get; set; }
        public string PageConnectStatus { get; set; }
    }
    public class ZFacebookConnectGetByIdRequest
    {
        public string FBID { get; set; }
        public int ShopID { get; set; }
        public string PageID { get; set; }
    }
    public class ZFacebookResetConnectPageRequest
    {
        public int ShopID { get; set; }
        public string UpdatedBy { get; set; }
    }
}
