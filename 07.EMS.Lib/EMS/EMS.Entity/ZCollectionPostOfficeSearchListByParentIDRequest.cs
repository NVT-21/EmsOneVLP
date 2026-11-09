using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCollectionPostOfficeSearchListByParentIDRequest
    {
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZCollectionPostOfficeSearchListByParentIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string CollectionPostOfficeCode { get; set; }
        [DataMember]
        public string CollectionPostOfficeName { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public int NumberOfChild { get; set; }
    }
}
