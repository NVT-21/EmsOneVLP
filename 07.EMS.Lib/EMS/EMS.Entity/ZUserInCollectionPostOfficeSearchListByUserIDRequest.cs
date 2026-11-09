using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZUserInCollectionPostOfficeSearchListByUserIDRequest
    {
        [DataMember]
        public int UserID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZUserInCollectionPostOfficeSearchListByUserIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string CollectionPostOfficeCode { get; set; }
        [DataMember]
        public string CollectionPostOfficeName { get; set; }
        [DataMember]
        public string IsChoose { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public int NumberOfChild { get; set; }
    }
}
