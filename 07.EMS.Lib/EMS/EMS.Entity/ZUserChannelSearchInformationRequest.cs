using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZUserChannelSearchInformationRequest
    {
        public string PageID { get; set; }
        public string UserChannelID { get; set; }
        public string Channel { get; set; }
        public int ShopID { get; set; }
    }
    public class ZUserChannelSearchInformationResponse
    {

    }
}
