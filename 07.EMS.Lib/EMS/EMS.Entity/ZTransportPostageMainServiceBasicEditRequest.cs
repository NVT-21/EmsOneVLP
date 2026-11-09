using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageMainServiceBasicEditRequest
    {
        [DataMember]
        public int TransportPostageID { get; set; }
        [DataMember]
        public string TransportPostageCode { get; set; }
        [DataMember]
        public string TransportPostageName { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int MainTransportServiceID { get; set; }
        [DataMember]
        public string ExtraTransportServiceID { get; set; }
        [DataMember]
        public int Area { get; set; }
        [DataMember]
        public int SubjectOfApplication { get; set; }
        //[DataMember]
        //public string ApplicationObjectValue { get; set; }
        [DataMember]
        public string AppliesToShop { get; set; }
        [DataMember]
        public string AppliesToProvinceFrom { get; set; }
        [DataMember]
        public int ProvinceApply { get; set; }
        [DataMember]
        public int AreaApply { get; set; }
        [DataMember]
        public int UpdateBy { get; set; }
    }
}
