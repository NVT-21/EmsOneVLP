using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderConvertToOrderCodeRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
