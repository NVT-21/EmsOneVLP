using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCollectionPostOfficeSearchListRequest
    {
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZCollectionPostOfficeSearchListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string CollectionPostOfficeCode { get; set; }
        [DataMember]
        public string CollectionPostOfficeName { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }
}
