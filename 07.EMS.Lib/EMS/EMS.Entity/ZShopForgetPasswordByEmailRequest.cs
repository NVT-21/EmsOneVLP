using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopForgetPasswordByEmailRequest
    {
        [DataMember]
        public string Email { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZShopForgetPasswordByEmailResponse
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public int OtpID { get; set; }
        [DataMember]
        public string OtpCode { get; set; }

    }
}
