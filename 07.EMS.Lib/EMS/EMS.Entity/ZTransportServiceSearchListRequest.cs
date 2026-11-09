using System;
using System.Runtime.Serialization;
namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportServiceSearchListRequest
    {
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportServiceSearchListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public ZBaseResponse SpecialService { get; set; }
        //public string SpecialService { get; set; } //Dịch vụ đặc biệt
        //public string SpecialServiceName
        //{
        //    get
        //    {
        //        return XData.GetNameYesOrNo(SpecialService);
        //    }
        //}
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public string IsDefault { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
