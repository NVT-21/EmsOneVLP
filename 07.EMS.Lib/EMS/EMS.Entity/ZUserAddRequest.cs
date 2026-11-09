using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZUserAddRequest
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string ServesCustomer { get; set; }
        [DataMember]
        public string UserCMS { get; set; }
        [DataMember]
        public List<int> ListCollectionPostOfficeID { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}
