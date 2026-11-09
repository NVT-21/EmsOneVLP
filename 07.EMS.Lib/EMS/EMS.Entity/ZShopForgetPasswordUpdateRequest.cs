using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZShopForgetPasswordUpdateRequest
    {
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string OtpCode { get; set; }
    }
}
