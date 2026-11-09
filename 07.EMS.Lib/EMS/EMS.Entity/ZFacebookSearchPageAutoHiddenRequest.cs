using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookSearchPageAutoHiddenRequest
    {
        public int ShopID { get; set; }
        public string PageID { get; set; }
    }

    public class ZFacebookSearchPageAutoHiddenResponse
    {
        public int ID { get; set; }
        public int IDHidden { get; set; }
        public string PageID { get; set; }
        public int IsHiddenComment { get; set; }
        public string TextInComment { get; set; }
        public int IsEmail { get; set; }
        public int IsPhoneNumber { get; set; }
        public int IsText { get; set; }
        public int AvailableDate { get; set; }
    }

    public class ZFacebookPageAutoHiddenByIDResponse
    {
        public int ID { get; set; }
        public string PageID { get; set; }
        public int PageAutoHiddenStatus { get; set; }
        public string TextInComment { get; set; }
        public int IsEmail { get; set; }
        public int IsPhoneNumber { get; set; }
        public int IsText { get; set; }
        public int AvailableDate { get; set; }
        public string PageName { get; set; }
        public string PageCategory { get; set; }
    }
}
