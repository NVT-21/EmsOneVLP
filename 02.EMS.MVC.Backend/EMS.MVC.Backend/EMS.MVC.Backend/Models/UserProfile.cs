using System;
using System.Runtime.Serialization;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class UserProfile
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
    }
}