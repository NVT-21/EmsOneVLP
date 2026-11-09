using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchInfoSendCMSRequest
    {
        [DataMember]
        public int RequestSupportID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UserID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchInfoSendCMSResponse
    {
        [DataMember]
        public ZRequestSupportSearchInfoSendCMSRequestSupportInfoResponse RequestSupportInfo { get; set; }
        [DataMember]
        public ZRequestSupportSearchInfoSendCMSShopInfoResponse ShopInfo { get; set; }
        [DataMember]
        public ZRequestSupportSearchInfoSendCMSEmployeeInfoResponse EmployeeInfo { get; set; }
        [DataMember]
        public ZRequestSupportSearchInfoSendCMSUserInfoResponse UserInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchInfoSendCMSRequestSupportInfoResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public ZBaseResponse Group { get; set; }
        [DataMember]
        public ZBaseResponse Type { get; set; }
        [DataMember]
        public string Document { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ReasonForComplaint { get; set; }
        [DataMember]
        public string Priority { get; set; }
        [DataMember]
        public string Origin { get; set; }
        [DataMember]
        public string ServiceGroup { get; set; }
        [DataMember]
        public string Scope { get; set; }
        [DataMember]
        public string TransportServiceCode { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public string PosC { get; set; }
        [DataMember]
        public string UpdateInformation { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchInfoSendCMSShopInfoResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string CrmOrPayPostCode { get; set; }
        [DataMember]
        public string EmailBCG { get; set; }
        [DataMember]
        public string EmailCSKH { get; set; }
        [DataMember]
        public string AcceptancePosCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchInfoSendCMSEmployeeInfoResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public ZBaseResponse Province { get; set; }
        [DataMember]
        public ZBaseResponse District { get; set; }
        [DataMember]
        public ZBaseResponse Ward { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchInfoSendCMSUserInfoResponse
    {
        [DataMember]
        public string UserCMS { get; set; }
    }
}
