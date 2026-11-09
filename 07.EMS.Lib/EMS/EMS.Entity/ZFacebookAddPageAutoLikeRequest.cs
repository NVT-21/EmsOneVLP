using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{

    public class ZFacebookSearchPageAutoLikeRequest
    {
        public int ShopID { get; set; }
        public string PageID { get; set; }
    }

    public class ZFacebookPageAutoLikeResponse
    {
        public int ID { get; set; }
        public string PageID { get; set; }
        public string IsLikeComment { get; set; }
    }
    public class ZFacebookReactionPageAutoLikeRequest
    {
        public int ID { get; set; }
        public int UpdatedBy { get; set; }
        public string Status { get; set; }
    }
}
