using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCollectionPostOfficeAddRequest
    {
        [DataMember]
        public string CollectionPostOfficeCode { get; set; }
        [DataMember]
        public string CollectionPostOfficeName { get; set; }
        [DataMember]
        public int CollectionPostOfficeType { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string UserCMS { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}
