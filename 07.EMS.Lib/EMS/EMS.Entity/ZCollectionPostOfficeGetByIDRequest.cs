using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCollectionPostOfficeGetByIDRequest
    {
        [DataMember]
        public int ID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZCollectionPostOfficeGetByIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string CollectionPostOfficeCode { get; set; }
        [DataMember]
        public string CollectionPostOfficeName { get; set; }
        [DataMember]
        public ZBaseResponse CollectionPostOfficeType { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public ZBaseResponse IsLock { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string UserCMS { get; set; }
    }
}
