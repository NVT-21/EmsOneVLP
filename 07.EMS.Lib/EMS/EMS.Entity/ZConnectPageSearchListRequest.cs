using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZConnectPageSearchListRequest
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string IsActive { get; set; }
        [DataMember]
        public string IsSyncFollower { get; set; }
        [DataMember]
        public string IsSyncConversion { get; set; }
        [DataMember]
        public string IsSyncMessage { get; set; }
        [DataMember]
        public string IsSyncProduct { get; set; }
        [DataMember]
        public string IsSyncProductPrice { get; set; }
        [DataMember]
        public string IsSyncProductStock { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZConnectPageSearchListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string PageName { get; set; }
        [DataMember]
        public string PageCategory { get; set; }
        [DataMember]
        public string PageDescription { get; set; }
        [DataMember]
        public string PageCipher { get; set; }
        [DataMember]
        public string PageAvatar { get; set; }
        [DataMember]
        public string PageCover { get; set; }
        [DataMember]
        public string IsActive { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string AccessToken { get; set; }
        [DataMember]
        public string RefreshToken { get; set; }
        [DataMember]
        public string UserAccessToken { get; set; }
        [DataMember]
        public string PageAccessToken { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string IsActiveCheck { get; set; }
        [DataMember]
        public string AccountID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZConnectPageUpdateInfoRequest
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string PageName { get; set; }
        [DataMember]
        public string PageAvatar { get; set; }
        [DataMember]
        public string PageCover { get; set; }
    }
}
