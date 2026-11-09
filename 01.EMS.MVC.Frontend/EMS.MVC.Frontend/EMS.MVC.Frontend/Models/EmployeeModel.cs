using System;
using System.Runtime.Serialization;
using EMS.Entity;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class EmployeeModel
    {
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string StoreOwner { get; set; }
        [DataMember]
        public string EmployeeLevel { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int GroupPermissionID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string PassWord { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string ReadyReceiveRequestOrder { get; set; }
        [DataMember]
        public string FullNameStoreOwner { get; set; }
        [DataMember]
        public XShopInformationModel ShopInformation { get; set; }
    }
}