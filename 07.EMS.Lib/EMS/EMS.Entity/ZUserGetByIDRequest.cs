using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZUserGetByIDRequest
    {
        [DataMember]
        public int UserID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZUserGetByIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public string ServesCustommer { get; set; }
        [DataMember]
        public string UserCMS { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
