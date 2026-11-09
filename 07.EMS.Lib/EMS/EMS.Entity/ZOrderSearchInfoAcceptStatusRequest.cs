using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderSearchInfoAcceptStatusRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderSearchInfoAcceptStatusResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }

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
        public string SenderName { get; set; }
        [DataMember]
        public string SenderPhone { get; set; }
        [DataMember]
        public int SenderProvinceID { get; set; }
        [DataMember]
        public int SenderDistrictID { get; set; }
        [DataMember]
        public int SenderWardID { get; set; }
        [DataMember]
        public string SenderStreet { get; set; }

        [DataMember]
        public string IsTransport { get; set; }
        [DataMember]
        public long TransportOrderValue { get; set; }
        [DataMember]
        public int TransportMainService { get; set; }
        [DataMember]
        public string TransportExtraService { get; set; }
        [DataMember]
        public int TransportPayer { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public long TransportQuantity { get; set; }
        

        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int TypeShop { get; set; }

        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public string WareHouseIsLock { get; set; }

        [DataMember]
        public int CustomerID { get; set; }

        [DataMember]
        public int PickUpID { get; set; }
        [DataMember]
        public string PickUpName { get; set; }
        [DataMember]
        public string PickUpPhone { get; set; }
        [DataMember]
        public int PickUpProvinceID { get; set; }
        [DataMember]
        public int PickUpDistrictID { get; set; }
        [DataMember]
        public int PickUpWardID { get; set; }
        [DataMember]
        public string PickUpStreet { get; set; }

        [DataMember]
        public List<ZOrderSearchInfoAcceptStatusOrderItemResponse> ListOrderItem { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderSearchInfoAcceptStatusOrderItemResponse
    {
        [DataMember]
        public int ProvisionalProductID { get; set; }
        [DataMember]
        public int ActualProductID { get; set; }
        [DataMember]
        public string ActualProductIsLock { get; set; }

        [DataMember]
        public int ProvisionalProductVariantID { get; set; }
        [DataMember]
        public int ActualProductVariantID { get; set; }
        [DataMember]
        public string ActualProductVariantIsLock { get; set; }

        [DataMember]
        public long ProvisionalProductVariantPrice { get; set; }
        [DataMember]
        public long ActualProductVariantPrice { get; set; }

        [DataMember]
        public int NumberOfProduct { get; set; }

        [DataMember]
        public string ProductAndVariantName { get; set; }
    }
}
