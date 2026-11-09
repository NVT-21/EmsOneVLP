using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderEditRequest
    {
        [DataMember]
        public string EMSOneCode { get; set; } // Mã EMSOne
        [DataMember]
        public string OrderCode { get; set; } // Mã đơn hàng
        [DataMember]
        public List<ZOrderEditItemRequest> OrderedProducts { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public ZOrderEditShipmentInfoRequest ShipmentDetails { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }
        [DataMember]
        public int ProductNumber { get; set; }
        [DataMember]
        public long TotalDiscount { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long TotalOrderValue { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public string PaymentStatus { get; set; }
        [DataMember]
        public int PaymentMethod { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string Transport { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string isRequestOrder { get; set; }
        [DataMember]
        public string isUpdatedCustomer { get; set; }
        [DataMember]
        public string isSendTransport { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public int RequestOrderID { get; set; }
        [DataMember]
        public string ProductSKU { get; set; }
        [DataMember]
        public string TransportOrderName { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public string TransportSize { get; set; }
        [DataMember]
        public long TransportQuantity { get; set; }
        [DataMember]
        public long TransportOrderValue { get; set; }
        [DataMember]
        public int TransportMainService { get; set; }
        [DataMember]
        public int TransportPayer { get; set; }
        [DataMember]
        public string TransportExtraService { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public string ExpectedDeliveryDate { get; set; }
        [DataMember]
        public int CollectionPostOfficeRecentID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderEditItemRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int VariantID { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
        [DataMember]
        public long Price { get; set; }
        [DataMember]
        public long Discount { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderEditShipmentInfoRequest
    {
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public string ProvinceName { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string WardName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public int PickUpAddressID { get; set; }
        [DataMember]
        public string PickupFullName { get; set; }
        [DataMember]
        public string PickupMobileNumber { get; set; }
        [DataMember]
        public int PickupProvinceID { get; set; }
        [DataMember]
        public string PickupProvinceName { get; set; }
        [DataMember]
        public int PickupDistrictID { get; set; }
        [DataMember]
        public string PickupDistrictName { get; set; }
        [DataMember]
        public int PickupWardID { get; set; }
        [DataMember]
        public string PickupWardName { get; set; }
        [DataMember]
        public string PickupStreet { get; set; }
        [DataMember]
        public string PickupFullAddress { get; set; }
    }
}
