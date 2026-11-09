using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportHistorySearchListUpdateInformationRequest
    {
        [DataMember]
        public string EShippingCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportHistorySearchListUpdateInformationResponse
    {
        [DataMember]
        public ZBaseResponse RequestSupportType { get; set; }
        [DataMember]
        public string OldUpdateInformation { get; set; }
        [DataMember]
        public string NewUpdateInformation { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
