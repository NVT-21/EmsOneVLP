using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOtpVerifyCodeRequest
    {
        [DataMember]
        public int OtpID { get; set; }
        [DataMember]
        public string OtpCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOtpVerifyCodeResponse
    {
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string OtpCode { get; set; }
    }
}
