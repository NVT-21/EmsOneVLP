using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloGetByOfficialAccountIDRequest
    {
        public string OfficialAccountID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZZaloGetByOfficialAccountIDResponse
    {
        public int ID { get; set; }
        public string OfficialAccountID { get; set; }
        public string OfficialAccountName { get; set; }
        public string OfficialAccountAvatar { get; set; }
        public string OfficialAccountCover { get; set; }
        public string OfficialAccountDescription { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
