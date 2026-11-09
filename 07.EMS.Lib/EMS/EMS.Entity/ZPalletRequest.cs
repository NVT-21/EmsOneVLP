using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPalletRequest
    {
    }

    [Serializable]
    [DataContract]
    public class ZPalletResponse
    {
        [DataMember]
        public int PalletID { get; set; }
        [DataMember]
        public string PalletCode { get; set; }
        [DataMember]
        public string PalletName { get; set; }
    }
}
