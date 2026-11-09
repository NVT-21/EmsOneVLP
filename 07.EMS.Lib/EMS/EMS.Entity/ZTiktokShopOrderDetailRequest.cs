using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopOrderDetailRequest
    {
        public List<string> order_id_list { get; set; }
    }
    public class ZTiktokShopOrderDetailResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopOrderDetailDataResponse Data { get; set; }
    }
    public class ZTiktokShopOrderDetailDataResponse
    {
        [JsonProperty("order_list")]
        public List<ZTiktokShopOrderDetailDataOrderListResponse> OrderList { get; set; }
    }
    public class ZTiktokShopOrderDetailDataOrderListResponse
    {
        [JsonProperty("order_id")]
        public string OrderID { get; set; }
        [JsonProperty("order_status")]
        public int OrderStatus { get; set; }
        [JsonProperty("shipping_provider")]
        public string ShippingProvider { get; set; }
        [JsonProperty("shipping_provider_id")]
        public string ShippingProviderID { get; set; }
        [JsonProperty("create_time")]
        public string CreateTime { get; set; }
        [JsonProperty("paid_time")]
        public int PaidTime { get; set; }
        [JsonProperty("buyer_message")]
        public string BuyerMessage { get; set; }
        [JsonProperty("payment_info")]
        public ZTiktokShopOrderDetailDataOrderListPaymentInfoResponse PaymentInfo { get; set; }
        [JsonProperty("recipient_address")]
        public ZTiktokShopOrderDetailDataOrderListRecipientAddressResponse RecipientAddress { get; set; }
        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }
        [JsonProperty("cancel_user")]
        public string CancelUser { get; set; }
        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }
        [JsonProperty("rts_time")]
        public long RtsTime { get; set; }
        [JsonProperty("rts_sla")]
        public long RtsSla { get; set; }
        [JsonProperty("tts_sla")]
        public long TtsSla { get; set; }
        [JsonProperty("cancel_order_sla")]
        public long CancelOrderSla { get; set; }
        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }
        [JsonProperty("package_list")]
        public List<ZTiktokShopOrderDetailDataOrderListPackageListResponse> PackageList { get; set; }
        [JsonProperty("receiver_address_updated")]
        public int ReceiverAddressUpdated { get; set; }
        [JsonProperty("buyer_uid")]
        public string BuyerUID { get; set; }
        [JsonProperty("split_or_combine_tag")]
        public string SplitOrCombineTag { get; set; }
        [JsonProperty("fulfillment_type")]
        public int FulfillmentType { get; set; }
        [JsonProperty("seller_note")]
        public string SellerNote { get; set; }
        [JsonProperty("warehouse_id")]
        public string WarehouseID { get; set; }
        [JsonProperty("payment_method_type")]
        public int PaymentMethodType { get; set; }
        [JsonProperty("payment_method_name")]
        public string PaymentMethodName { get; set; }
        [JsonProperty("delivery_option_type")]
        public int DeliveryOptionType { get; set; }
        [JsonProperty("delivery_option_description")]
        public string DeliveryOptionDescription { get; set; }
        [JsonProperty("delivery_option_id")]
        public string DeliveryOptionID { get; set; }
        [JsonProperty("delivery_sla")]
        public int DeliverySla { get; set; }
        [JsonProperty("is_cod")]
        public bool isCOD { get; set; }
        [JsonProperty("item_list")]
        public List<ZTiktokShopOrderDetailDataOrderListItemListResponse> ItemList { get; set; }
        [JsonProperty("need_upload_invoice")]
        public int NeedUploadInvoice { get; set; }
        [JsonProperty("order_line_list")]
        public List<ZTiktokShopOrderDetailDataOrderListOrderLineListResponse> OrderLineList { get; set; }
        [JsonProperty("cpf")]
        public string CPF { get; set; }
        [JsonProperty("district_info_list")]
        public List<ZTiktokShopOrderDetailDataOrderListDistrictInfoListResponse> DistrictInfoList { get; set; }
    }
    public class ZTiktokShopOrderDetailDataOrderListPaymentInfoResponse
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("sub_total")]
        public long SubTotal { get; set; }
        [JsonProperty("shipping_fee")]
        public long ShippingFee { get; set; }
        [JsonProperty("seller_discount")]
        public long SellerDiscount { get; set; }
        [JsonProperty("platform_discount")]
        public long PlatformDiscount { get; set; }
        [JsonProperty("total_amount")]
        public long TotalAmount { get; set; }
        [JsonProperty("original_total_product_price")]
        public long OriginalTotalProductPrice { get; set; }
        [JsonProperty("original_shipping_fee")]
        public long OriginalShippingFee { get; set; }
        [JsonProperty("shipping_fee_seller_discount")]
        public long ShippingFeeSellerDiscount { get; set; }
        [JsonProperty("shipping_fee_platform_discount")]
        public long ShippingFeePlatformDiscount { get; set; }
        [JsonProperty("taxes")]
        public long Taxes { get; set; }
        [JsonProperty("small_order_fee")]
        public long SmallOrderFee { get; set; }
        [JsonProperty("shipping_fee_tax")]
        public long ShippingFeeTax { get; set; }
        [JsonProperty("product_taxes")]
        public long ProductTaxes { get; set; }
    }
    public class ZTiktokShopOrderDetailDataOrderListRecipientAddressResponse
    {
        [JsonProperty("full_address")]
        public string FullAddress { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("zipcode")]
        public string ZipCode { get; set; }
        [JsonProperty("address_detail")]
        public string AddressDetail { get; set; }
        [JsonProperty("region_code")]
        public string RegionCode { get; set; }
        [JsonProperty("addressline1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("addressline2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("addressline3")]
        public string AddressLine3 { get; set; }
        [JsonProperty("addressline4")]
        public string AddressLine4 { get; set; }
    }
    public class ZTiktokShopOrderDetailDataOrderListPackageListResponse
    {
        [JsonProperty("package_id")]
        public string PackageID { get; set; }
    }
    public class ZTiktokShopOrderDetailDataOrderListOrderLineListResponse
    {
        [JsonProperty("order_line_id")]
        public string OrderLineID { get; set; }
        [JsonProperty("sku_id")]
        public string SKUID { get; set; }
        [JsonProperty("display_status")]
        public int DisplayStatus { get; set; }
        [JsonProperty("product_id")]
        public string ProductID { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("sku_name")]
        public string SKUName { get; set; }
        [JsonProperty("seller_sku")]
        public string SellerSKU { get; set; }
        [JsonProperty("sku_image")]
        public string SKUImage { get; set; }
        [JsonProperty("sku_type")]
        public int SKUType { get; set; }
        [JsonProperty("original_price")]
        public float OriginalPrice { get; set; }
        [JsonProperty("sale_price")]
        public float SalePrice { get; set; }
        [JsonProperty("platform_discount")]
        public float PlatformDiscount { get; set; }
        [JsonProperty("seller_discount")]
        public float SellerDiscount { get; set; }
        [JsonProperty("rts_time")]
        public int Rtstime { get; set; }
        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }
        [JsonProperty("cancel_user")]
        public string CancelUser { get; set; }
        [JsonProperty("package_id")]
        public string PackageID { get; set; }
        [JsonProperty("package_status")]
        public int PackageStatus { get; set; }
        [JsonProperty("package_freeze_status")]
        public int PackageFreezeStatus { get; set; }
        [JsonProperty("shipping_provider_id")]
        public string ShippingProviderID { get; set; }
        [JsonProperty("shipping_provider_name")]
        public string ShippingProviderName { get; set; }
        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }
        //[JsonProperty("small_order_fee")]
        //public float SmallOrderFee { get; set; }
        [JsonProperty("is_gift")]
        public bool isGift { get; set; }
        [JsonProperty("item_tax")]
        public List<ZTiktokShopOrderDetailDataOrderListOrderLineListItemTaxResponse> ItemTax { get; set; }
    }
    public class ZTiktokShopOrderDetailDataOrderListOrderLineListItemTaxResponse
    {
        [JsonProperty("tax_type")]
        public int TaxType { get; set; }
        [JsonProperty("tax_amount")]
        public float TaxAmount { get; set; }
    }
    public class ZTiktokShopOrderDetailDataOrderListDistrictInfoListResponse
    {
        [JsonProperty("address_level_name")]
        public string AddressLevelName { get; set; }
        [JsonProperty("address_name")]
        public string AddressName { get; set; }
    }
    public class ZTiktokShopOrderDetailDataOrderListItemListResponse
    {
        [JsonProperty("product_id")]
        public string ProductID { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("seller_sku")]
        public string SellerSKU { get; set; }
        [JsonProperty("sku_display_status")]
        public int SKUDisplayStatus { get; set; }
        [JsonProperty("sku_ext_status")]
        public int SKUExtStatus { get; set; }
        [JsonProperty("sku_id")]
        public string SKUID { get; set; }
        [JsonProperty("sku_image")]
        public string SKUImage { get; set; }
        [JsonProperty("sku_name")]
        public string SKUName { get; set; }
        [JsonProperty("sku_original_price")]
        public long SKUOriginalPrice { get; set; }
        [JsonProperty("sku_platform_discount")]
        public long SKUPlatformDiscount { get; set; }
        [JsonProperty("sku_platform_discount_total")]
        public long SKUPlatformDiscountTotal { get; set; }
        [JsonProperty("sku_sale_price")]
        public long SKUSalePrice { get; set; }
        [JsonProperty("sku_seller_discount")]
        public long SKUSellerDiscount { get; set; }
        [JsonProperty("sku_type")]
        public int SKUType { get; set; }
    }
}