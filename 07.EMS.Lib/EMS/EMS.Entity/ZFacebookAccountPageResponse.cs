using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookAccountPageResponse
    {
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string AccountToken { get; set; }
        public int ShopID { get; set; }
        public string PageAccessToken { get; set; }
        public string AmndDate { get; set; }
    }

    public class ZFacebookAccountPageAutoHiddenResponse
    {
        public string PageID { get; set; }
        public string AccountToken { get; set; }
        public int ShopID { get; set; }
        public string TextInComment { get; set; }
        public int IsEmail { get; set; }
        public int IsPhonenumber { get; set; }
        public int IsText { get; set; }
        public int AvailableDate { get; set; }
    }
}
