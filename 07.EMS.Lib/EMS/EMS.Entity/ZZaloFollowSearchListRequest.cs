using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloFollowSearchListRequest
    {
        public string PageID { get; set; }
        public int ShopID { get; set; }
    }
    public class ZZaloFollowSearchListResponse
    {
        public int ID { get; set; }
        public string ZaloUserID { get; set; }
        public string ZaloUserIDByApp { get; set; }
        public string FullName { get; set; }
        public string UserGender { get; set; }
        public string MobileNumber { get; set; }
        public string Avatar { get; set; }
        public string BirthDay { get; set; }
        public string DisplayName { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string Street { get; set; }
        public string FullAddress { get; set; }
        public string CreatedDate { get; set; }
    }
}
