using System.Runtime.Serialization;

namespace EMS.TransportWorkerService;

[Serializable]
[DataContract]
public class ZOrderGetByIDRequest
{
    [DataMember]
    public int ID { get; set; }
    [DataMember]
    public int ShopID { get; set; }
}

[Serializable]
[DataContract]
public class ZOrderGetByOrderCodeRequest
{
    [DataMember]
    public string OrderCode { get; set; }
    [DataMember]
    public int ShopID { get; set; }
}

[Serializable]
[DataContract]
public class ZOrderGetByIDResponse
{
    [DataMember]
    public int ID { get; set; }
    [DataMember]
    public string EMSOneCode { get; set; } // Mã EMSOne
    [DataMember]
    public string OrderCode { get; set; } // Mã đơn hàng
    [DataMember]
    public string BatchCode { get; set; } // Mã lô
    [DataMember]
    public string Status { get; set; }
    [DataMember]
    public string StatusName { get; set; }
    [DataMember]
    public int ReturnCount { get; set; }
    [DataMember]
    public string ReturnShippingCodes { get; set; }
    //[DataMember]
    //public int DeliveryNoteID { get; set; }
    //[DataMember]
    //public string DeliveryNoteCode { get; set; }
    //[DataMember]
    //public string DeliveryNoteStatus { get; set; }
    //[DataMember]
    //public string IsDeliveryNote { get; set; }
    [DataMember]
    public int WareHouseID { get; set; }
    [DataMember]
    public string WareHouseName { get; set; }
    [DataMember]
    public long TotalAmount { get; set; }
    [DataMember]
    public int TotalDiscount { get; set; }
    [DataMember]
    public int TransportFee { get; set; }
    [DataMember]
    public int ActualTransportFee { get; set; }
    [DataMember]
    public long TotalProductValue { get; set; }
    [DataMember]
    public long TotalOrderValue { get; set; }
    [DataMember]
    public long TotalCOD { get; set; }
    [DataMember]
    public long ActualTotalCOD { get; set; }
    [DataMember]
    public long TotalPHU { get; set; }
    [DataMember]
    public string PaymentStatus { get; set; }
    [DataMember]
    public int PaymentMethod { get; set; }
    [DataMember]
    public int CreatedBy { get; set; }
    [DataMember]
    public string CreatedDate { get; set; }
    [DataMember]
    public string Transport { get; set; }
    [DataMember]
    public string Channel { get; set; }
    [DataMember]
    public string Note { get; set; }
    [DataMember]
    public string Message { get; set; }
    [DataMember]
    public string ReasonCancel { get; set; }
    [DataMember]
    public string UpdatedDate { get; set; }
    [DataMember]
    public string UpdatedName { get; set; }
    [DataMember]
    public string TransportOrderName { get; set; }
    [DataMember]
    public double TransportWeight { get; set; }
    [DataMember]
    public double ActualTransportWeight { get; set; }
    [DataMember]
    public double ExchangeTransportWeight { get; set; }
    [DataMember]
    public double TransportSizeLength { get; set; }
    [DataMember]
    public double TransportSizeWidth { get; set; }
    [DataMember]
    public double TransportSizeHeight { get; set; }
    [DataMember]
    public long TransportQuantity { get; set; }
    [DataMember]
    public long TransportOrderValue { get; set; }
    [DataMember]
    public int TransportMainService { get; set; }
    [DataMember]
    public string TransportMainServiceName { get; set; }
    [DataMember]
    public string TransportExtraService { get; set; }
    [DataMember]
    public string TransportExtraServiceName { get; set; }
    [DataMember]
    public int TransportPayer { get; set; }
    [DataMember]
    public string TransportPayerName { get; set; }
    [DataMember]
    public ZOrderAddShipmentInfoRequest ShipmentDetails { get; set; }
    //[DataMember]
    //public string RequestSupportUpdateInformation { get; set; }
    [DataMember]
    public ZBaseResponse CollectionType { get; set; }
    [DataMember]
    public int TypeShop { get; set; }
    [DataMember]
    public string ShippingCode { get; set; }
    [DataMember]
    public string AcceptancePosCode { get; set; }
    [DataMember]
    public string PlaceOfCreation { get; set; }
    [DataMember]
    public string DeviceName { get; set; }
    [DataMember]
    public string PostalType { get; set; }
    [DataMember]
    public string ExpectedDeliveryDate { get; set; }
    [DataMember]
    public string CrmOrPayPostCode { get; set; }
}
