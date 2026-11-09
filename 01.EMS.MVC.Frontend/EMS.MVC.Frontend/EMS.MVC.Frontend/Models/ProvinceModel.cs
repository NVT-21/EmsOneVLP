using System;
using System.Runtime.Serialization;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class ProvinceModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}