using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOtpAddRequest
    {
        [DataMember]
        public string OtpCode { get; set; }
        [DataMember]
        public int OtpType { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
    }

    //[Serializable]
    //[DataContract]
    //public class ZOtpAddResponse
    //{
    //    [DataMember]
    //    public string OtpID { get; set; }
    //}
}
