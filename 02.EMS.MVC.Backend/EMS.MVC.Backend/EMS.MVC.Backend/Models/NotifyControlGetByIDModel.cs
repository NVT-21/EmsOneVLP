using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class NotifyControlGetByIDModel
    {
        [DataMember]
        public int NotifyID { get; set; }
    }
}