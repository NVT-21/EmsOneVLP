using System;
using System.Runtime.Serialization;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class UpdatePasswordViewModel
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string NewPassword { get; set; }
        [DataMember]
        public string ConfirmPassword { get; set; }
    }
}