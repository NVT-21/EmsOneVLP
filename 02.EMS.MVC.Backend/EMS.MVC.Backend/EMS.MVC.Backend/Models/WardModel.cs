using System;
using System.Runtime.Serialization;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class WardModel
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