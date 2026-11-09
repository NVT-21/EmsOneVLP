using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportServiceAddRequest
    {
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Classification { get; set; } //Loại hàng hóa
        [DataMember]
        public int Type { get; set; } //Loại bưu gửi
        [DataMember]
        public string IsSpecial { get; set; } //Dịch vụ đặc biệt
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}
