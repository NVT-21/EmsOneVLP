using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZEmployeeCheckConditionRequest
    {
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
