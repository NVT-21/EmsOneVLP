using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloSearchListOARequest
    {
        public int ShopID { get; set; }
        public string IsSyncFirst { get; set; }
    }
    public class ZZaloSearchListOAResponse
    {
        public int ID { get; set; }
        public string AuthorizationCode { get; set; }
        public string RefreshToken { get; set; }
        public string OfficialAccountID { get; set; }
        public string OfficialAccountName { get; set; }
        public string AccessToken { get; set; }
        public string Avatar { get; set; }
        public string Cover { get; set; }
        public string Description { get; set; }
        public int ShopID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
