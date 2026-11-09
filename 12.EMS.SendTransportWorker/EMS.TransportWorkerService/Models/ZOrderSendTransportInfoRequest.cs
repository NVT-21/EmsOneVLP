using System.Runtime.Serialization;

namespace EMS.TransportWorkerService;

[Serializable]
[DataContract]
public class ZOrderSendTransportInfoRequest
{
    [DataMember]
    public int OrderID { get; set; }
    [DataMember]
    public int ShopID { get; set; }
    [DataMember]
    public int UpdatedBy { get; set; }
    [DataMember]
    public string AmndState { get; set; }
}
[Serializable]
[DataContract]
public class ZOrderSendTransportInfoResponse
{
    [DataMember]
    public string CrmOrPayPostCode { get; set; }
    [DataMember]
    public string ShopPhone { get; set; }
    [DataMember]
    public string ShopStoreName { get; set; }
    [DataMember]
    public string ShopFullName { get; set; }
    [DataMember]
    public string ShopEmail { get; set; }
    [DataMember]
    public int ShopID { get; set; }
    [DataMember]
    public string CollectionPostOfficeCode { get; set; }


    [DataMember]
    public int OrderID { get; set; }
    [DataMember]
    public string EMSOneCode { get; set; }
    [DataMember]
    public string SenderName { get; set; }
    [DataMember]
    public string SenderCode { get; set; }
    [DataMember]
    public string SenderStoreName { get; set; }
    [DataMember]
    public string SenderPhone { get; set; }
    [DataMember]
    public string SenderStreet { get; set; }
    [DataMember]
    public string SenderFullAddress { get; set; }
    [DataMember]
    public int SenderProvinceID { get; set; }
    [DataMember]
    public int SenderDistrictID { get; set; }
    [DataMember]
    public int SenderWardID { get; set; }
    [DataMember]
    public string SenderEmail { get; set; }
    [DataMember]
    public string ReceiverName { get; set; }
    [DataMember]
    public int ReceiverProvinceID { get; set; }
    [DataMember]
    public string ReceiverPhone { get; set; }
    [DataMember]
    public int ReceiverDistrictID { get; set; }
    [DataMember]
    public int ReceiverWardID { get; set; }
    [DataMember]
    public string ReceiverStreet { get; set; }
    [DataMember]
    public string ReceiverFullAddress { get; set; }
    [DataMember]
    public string ProductName { get; set; }
    [DataMember]
    public long ProductValue { get; set; }
    [DataMember]
    public string ProductItem { get; set; }
    [DataMember]
    public string ProductCode { get; set; }
    [DataMember]
    public string ProductDescription { get; set; }
    [DataMember]
    public string ProductNote { get; set; }
    [DataMember]
    public int ProductQuantity { get; set; }
    [DataMember]
    public int Weight { get; set; }
    [DataMember]
    public long TotalAmount { get; set; }
    [DataMember]
    public long ServiceFee { get; set; }
    [DataMember]
    public long TransportFee { get; set; }
    [DataMember]
    public string OrderStatus { get; set; }
    [DataMember]
    public int CollectionType { get; set; }
    [DataMember]
    public string Transport { get; set; }
    [DataMember]
    public int CreatedBy { get; set; }

    [DataMember]
    public string EspeciallyServiceCode { get; set; }
    [DataMember]
    public int EspeciallyServiceType { get; set; }
    [DataMember]
    public ZBaseResponse Classification { get; set; }

    [DataMember]
    public string AdditionService { get; set; }

    [DataMember]
    public string POCode { get; set; }

    [DataMember]
    public string ShippingCode { get; set; }
    [DataMember]
    public ZBaseResponse Consignment { get; set; }

    [DataMember]
    public long TotalCOD { get; set; }
    [DataMember]
    public int TransportSizeLength { get; set; }
    [DataMember]
    public int TransportSizeWidth { get; set; }
    [DataMember]
    public int TransportSizeHeight { get; set; }
    [DataMember]
    public string Note { get; set; }
    [DataMember]
    public int TransportPayer { get; set; }
}

[Serializable]
[DataContract]
public class ZOrderSendTransportInfoVNPOSTResponse
{
    public ZOrderSendTransportInfoVNPOSTResponse()
    {
        Products = new List<ItemVNPOST>();
    }
    [DataMember]
    public int Amnd_User { get; set; }
    [DataMember]
    public int Id { get; set; }
    [DataMember]
    public string Code { get; set; }
    [DataMember]
    public string CustomerCode { get; set; }
    [DataMember]
    public long MainFee { get; set; }
    [DataMember]
    public string CustomerName { get; set; }
    [DataMember]
    public string CustomerEmail { get; set; }
    [DataMember]
    public string CustomerPhone { get; set; }
    [DataMember]
    public string Transport { get; set; }
    [DataMember]
    public int OrderId { get; set; }
    [DataMember]
    public string OrderCode { get; set; }
    [DataMember]
    public string TrackingCode { get; set; }
    [DataMember]
    public string Ref_Code { get; set; }
    [DataMember]
    public int StoreId { get; set; }
    [DataMember]
    public string SenderCode { get; set; }
    [DataMember]
    public string SenderName { get; set; }
    [DataMember]
    public string SenderPhone { get; set; }
    [DataMember]
    public string SenderEmail { get; set; }
    [DataMember]
    public int SenderProvinceId { get; set; }
    [DataMember]
    public int SenderDistrictId { get; set; }
    [DataMember]
    public int SenderWardId { get; set; }
    [DataMember]
    public int SenderHamletId { get; set; }
    [DataMember]
    public string SenderStreet { get; set; }
    [DataMember]
    public string SenderAddress { get; set; }
    [DataMember]
    public string ReceiverName { get; set; }
    [DataMember]
    public string ReceiverEmail { get; set; }
    [DataMember]
    public string ReceiverPhone { get; set; }
    [DataMember]
    public int ReceiverProvinceId { get; set; }
    [DataMember]
    public int ReceiveDistrictId { get; set; }
    [DataMember]
    public int ReceiveWardId { get; set; }
    [DataMember]
    public int ReceiveHamletId { get; set; }
    [DataMember]
    public string ReceiveStreet { get; set; }
    [DataMember]
    public string ReceiveAddress { get; set; }
    [DataMember]
    public int ProductQuantity { get; set; }
    [DataMember]
    public string ProductCode { get; set; }
    [DataMember]
    public string ProductName { get; set; }
    [DataMember]
    public string ProductDescription { get; set; }
    [DataMember]
    public string StatusCode { get; set; }
    [DataMember]
    public string StatusDescription { get; set; }
    [DataMember]
    public long TotalAmount { get; set; }
    [DataMember]
    public long Weight { get; set; }
    [DataMember]
    public int ServiceType { get; set; }
    [DataMember]
    public string Channel { get; set; }
    [DataMember]
    public string Po_Create { get; set; }
    [DataMember]
    public string Order_Join { get; set; }
    [DataMember]
    public string Order_Status { get; set; }
    [DataMember]
    public string Order_Status_Desc { get; set; }
    [DataMember]
    public bool Check { get; set; }
    [DataMember]
    public bool IsConsorShip { get; set; }
    [DataMember]
    public string FileName { get; set; }
    [DataMember]
    public string CreateDate { get; set; }
    [DataMember]
    public string LastUpdateTime { get; set; }
    [DataMember]
    public int Complete { get; set; }
    [DataMember]
    public string SubCustomerCode { get; set; }
    [DataMember]
    public long ProductValue { get; set; }
    [DataMember]
    public int Cod { get; set; }
    [DataMember]
    public string ToCountryCode { get; set; }
    [DataMember]
    public string ToZipCode { get; set; }
    [DataMember]
    public string StatusTct { get; set; }
    [DataMember]
    public string ScheduleAt { get; set; }
    [DataMember]
    public string LLMOrderID { get; set; }
    [DataMember]
    public string LLMCustomerOrderID { get; set; }

    [DataMember]
    public string Promotion { get; set; }
    [DataMember]
    public int LlmKm { get; set; }
    [DataMember]
    public int LlmFee { get; set; }
    [DataMember]
    public string PickupTime { get; set; }
    [DataMember]
    public string StatusLlm { get; set; }
    [DataMember]
    public string StatusLlmDate { get; set; }
    [DataMember]
    public string AdditionService { get; set; }
    [DataMember]
    public List<ItemVNPOST> Products { get; set; }

    [DataMember]
    public string OrderNumber { get; set; }

    [DataMember]
    public string StatusRelaseCode { get; set; }

    [DataMember]
    public string StatusRelaseTime { get; set; }

    [DataMember]
    public string SpecialService { get; set; }
    [DataMember]
    public string ServiceCode { get; set; }
    [DataMember]
    public int Service_Fee { get; set; }
    [DataMember]
    public string CONTRACT_CODE_C { get; set; }
    [DataMember]
    public string ValueAddedList { get; set; }
    [DataMember]
    public string ReceiverCode { get; set; }
    [DataMember]
    public string FlagConfig { get; set; }

}

[Serializable]
[DataContract]
public class ItemVNPOST
{
    [DataMember]
    public int ID { get; set; }
    [DataMember]
    public string ITEM_NAME { get; set; }
    [DataMember]
    public int ITEM_VALUE { get; set; }
    [DataMember]
    public string ITEM_CODE { get; set; }
    [DataMember]
    public string ITEM_DESCRIPTION { get; set; }
    [DataMember]
    public int ITEM_QUANTITY { get; set; }
    [DataMember]
    public int ITEM_WEIGHT { get; set; }
}