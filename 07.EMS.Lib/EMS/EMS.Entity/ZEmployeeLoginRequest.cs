using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZEmployeeLoginRequest
    {
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string IsShop { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZEmployeeLoginResponse
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
        //public int ShopID { get; set; }
        [DataMember]
        public int GroupPermissionID { get; set; }
        [DataMember]
        public string StoreOwner { get; set; }
        //public string StoreName { get; set; }
        //public int ShopLevel { get; set; }
        //public string ShopIsLock { get; set; }
        //public string ShopEndDate { get; set; }
        [DataMember]
        public string ReadyReceiveRequestOrder { get; set; }
        //public XPickupInformationModel PickupInformation { get; set; }
        [DataMember]
        public XShopInformationModel ShopInformation { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZEmployeeLoginAPIResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string CrmOrPayPostCode { get; set; }
        [DataMember]
        public XTokenModel TokenDetail { get; set; }
    }
}
