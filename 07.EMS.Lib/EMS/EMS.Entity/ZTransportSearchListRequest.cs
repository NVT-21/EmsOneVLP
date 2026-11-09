using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTransportSearchListRequest
    {
        public int ShopID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string MobileNumber { get; set; }
        public string EmsBarCode { get; set; }
        public string OrderCode { get; set; }
        public string Status { get; set; }
    }
    public class ZTransportSearchListResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string CreatedDate { get; set; }
        public string RequestSubmissionDate { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public long TotalAmount { get; set; }
        public long TotalDiscount { get; set; }
        public long TransportFee { get; set; }
        public long TotalOrderValue { get; set; }
        public string Channel { get; set; }
        public long TotalProductValue { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string Transport { get; set; }
        public string TransportStatus { get; set; }
        public string IsDeliveryNote { get; set; }
        public int PickupProvinceID { get; set; }
        public string EmsBarCode { get; set; }
        public long TotalMoney { get; set; }
        public long ActualCharges { get; set; }
    }

    public class ZTransportInfoShippingRequest
    {
        public int ShopID { get; set; }
    }
    public class ZTransportInfoShippingResponse
    {
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int CustomerProvinceID { get; set; }
        public int CustomerDistrictID { get; set; }
        public int CustomerWardID { get; set; }
        public string CustomerAdress { get; set; }
        public int PickupProvinceID { get; set; }
        public int PickupDistrictID { get; set; }
        public int PickupWardID { get; set; }
        public string PickupStreet { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
    }

    public class ZTransportSearchInfoShopOrderDetailRequest
    {
        public int ShopID { get; set; }
        public string OrderCode { get; set; }
    }
    public class ZTransportSearchInfoShopOrderDetailResponse
    {
        //public string SenderName { get; set; }
        //public string SenderCode { get; set; }
        //public string SenderPhone { get; set; }
        //public string SenderEmail { get; set; }
        //public int SenderProvinceID { get; set; }
        //public int SenderDistrictID { get; set; }
        //public int SenderWardID { get; set; }
        //public string SenderAddress { get; set; }
        public string ReceiverName { get; set; }
        public int ReceiverProvinceID { get; set; }
        public int ReceiverDistrictID { get; set; }
        public int ReceiverWardID { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPhone { get; set; }
        public string ProductName { get; set; }
        public int ProductValue { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription{ get; set; }
        public int ProductQuantity{ get; set; }
        public int Weight{ get; set; }
        public long TotalAmount{ get; set; }
        public string AdditionServices{ get; set; }
        public string EspeciallyServices { get; set; }
        public long TotalCOD { get; set; }
    }
    public class ZTransportUpdateOrderRequest
    {
        public string OrderCode { get; set; }
        public string Status { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ZTransportGetEmsBarCodeRequest
    {
        public string CustomerCode { get; set; }
        public int ProvinceID { get; set; }
        public string OrderCode { get; set; }
        public string CreatedBy { get; set; }
        public string AmndState { get; set; }
        public int IsConsignment { get; set; }
        public string ShippingID { get; set; }
        public string IsCommit { get; set; }
    }
}
