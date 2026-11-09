using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZEmployeeGetByMobileNumberRequest
    {
        [DataMember]
        public string MobileNumber { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZEmployeeGetByMobileNumberResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string EmployeeLevel { get; set; }
        [DataMember]
        public int GroupPermissionID { get; set; }
        [DataMember]
        public string StoreOwner { get; set; }
        [DataMember]
        public string ReadyReceiveRequestOrder { get; set; }
        [DataMember]
        public XShopInformationModel ShopInformation { get; set; }
    }
}
