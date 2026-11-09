using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderDetailByOrderIDRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderDetailByOrderIDResponse: ZOrderGetByIDResponse
    {
        [DataMember]
        public List<ZEMSSignaruteAndImageInformationResponse> ListEMSSignaruteAndImageInfo { get; set; }
        [DataMember]
        public List<ZVNPSignaruteAndImageInformationResponse> ListVNPSignaruteAndImageInfo{ get; set; }

    }
}
