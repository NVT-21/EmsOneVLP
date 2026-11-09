using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZUserLoginRequest
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZUserLoginResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }
}
