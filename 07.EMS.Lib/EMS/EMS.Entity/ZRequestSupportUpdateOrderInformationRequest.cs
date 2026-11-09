using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportUpdateOrderInformationRequest
    {
        [DataMember]
        public int RequestSupportID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
