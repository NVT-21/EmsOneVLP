using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZUserEditRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string ServesCustomer { get; set; }
        [DataMember]
        public string UserCMS { get; set; }
        [DataMember]
        public List<int> ListCollectionPostOfficeID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
