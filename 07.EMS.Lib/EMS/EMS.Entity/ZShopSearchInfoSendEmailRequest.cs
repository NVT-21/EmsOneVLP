using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopSearchInfoSendEmailRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string ToEmail { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string OtpCode { get; set; }
    }
}
