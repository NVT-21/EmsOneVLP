using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopOrderListRequest_v202309
    {
        //public string buyer_user_id { get; set; }
        //public int create_time_ge { get; set; }
        //public int create_time_lt { get; set; }
        //public string order_status { get; set; }
        //public string shipping_type { get; set; }
        //public int update_time_ge { get; set; }
        //public int update_time_lt { get; set; }
    }

    public class ZTiktokShopOrderListResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopOrderListDataResponse_v202309 Data { get; set; }
    }
    public class ZTiktokShopOrderListDataResponse_v202309
    {
        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        [JsonProperty("orders")]
        public List<ZTiktokShopOrderListDataOrdersResponse_v202309> Orders { get; set; }
    }
    public class ZTiktokShopOrderListDataOrdersResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("shipping_provider")]
        public string ShippingProvider { get; set; }
        [JsonProperty("shipping_provider_id")]
        public string ShippingProviderID { get; set; }
        [JsonProperty("create_time")]
        public int CreateTime { get; set; }
        [JsonProperty("paid_time")]
        public int PaidTime { get; set; }
        [JsonProperty("buyer_message")]
        public string BuyerMessage { get; set; }
        [JsonProperty("payment")]
        public ZTiktokShopOrderListDataOrdersPaymentResponse_v202309 Payment { get; set; }
        [JsonProperty("recipient_address")]
        public ZTiktokShopOrderListDataOrdersRecipientAddressResponse_v202309 RecipientAddress { get; set; }
        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }
        [JsonProperty("cancellation_initiator")]
        public string CancellationInitiator { get; set; }
        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }
        [JsonProperty("rts_time")]
        public int RtsTime { get; set; }
        [JsonProperty("rts_sla_time")]
        public int RtsSlaTime { get; set; }
        [JsonProperty("tts_sla_time")]
        public int TtsSlaTime { get; set; }
        [JsonProperty("cancel_order_sla_time")]
        public int CancelOrderSlaTime { get; set; }
        [JsonProperty("update_time")]
        public int UpdateTime { get; set; }
        [JsonProperty("packages")]
        public List<ZTiktokShopOrderListDataOrdersPackagesResponse_v202309> Packages { get; set; }
        [JsonProperty("has_updated_recipient_address")]
        public Boolean HasUpdatedRecipientAddress { get; set; }
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("split_or_combine_tag")]
        public string SplitOrCombineTag { get; set; }
        [JsonProperty("fulfillment_type")]
        public string FulfillmentType { get; set; }
        [JsonProperty("seller_note")]
        public string SellerNote { get; set; }
        [JsonProperty("warehouse_id")]
        public string WarehouseID { get; set; }
        [JsonProperty("payment_method_name")]
        public string PaymentMethodName { get; set; }
        [JsonProperty("shipping_type")]
        public string ShippingType { get; set; }
        [JsonProperty("delivery_option_name")]
        public string DeliveryOptionName { get; set; }
        [JsonProperty("delivery_option_id")]
        public string DeliveryOptionID { get; set; }
        [JsonProperty("delivery_sla_time")]
        public int DeliverySlaTime { get; set; }
        [JsonProperty("is_cod")]
        public Boolean IsCOD{ get; set; }
        [JsonProperty("is_sample_order")]
        public Boolean IsSampleOrder { get; set; }
        [JsonProperty("line_items")]
        public List<ZTiktokShopOrderListDataOrdersLineItemsResponse_v202309> LineItems { get; set; }
        [JsonProperty("need_upload_invoice")]
        public string NeedUploadInvoice { get; set; }
        [JsonProperty("buyer_email")]
        public string BuyerEmail { get; set; }
        [JsonProperty("cpf")]
        public string CPF { get; set; }
    }
    public class ZTiktokShopOrderListDataOrdersPaymentResponse_v202309
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("sub_total")]
        public string SubTotal { get; set; }
        [JsonProperty("shipping_fee")]
        public string ShippingFee { get; set; }
        [JsonProperty("seller_discount")]
        public string SellerDiscount { get; set; }
        [JsonProperty("platform_discount")]
        public string PlatformDiscount { get; set; }
        [JsonProperty("total_amount")]
        public string TotalAmount { get; set; }
        [JsonProperty("original_total_product_price")]
        public string OriginalTotalProductPrice { get; set; }
        [JsonProperty("original_shipping_fee")]
        public string OriginalShippingFee { get; set; }
        [JsonProperty("shipping_fee_seller_discount")]
        public string ShippingFeeSellerDiscount { get; set; }
        [JsonProperty("shipping_fee_platform_discount")]
        public string ShippingFeePlatformDiscount { get; set; }
        [JsonProperty("tax")]
        public string Tax { get; set; }
        [JsonProperty("small_order_fee")]
        public string SmallOrderFee { get; set; }
        [JsonProperty("shipping_fee_tax")]
        public string ShippingFeeTax { get; set; }
        [JsonProperty("product_tax")]
        public string ProductTax { get; set; }
    }
    public class ZTiktokShopOrderListDataOrdersRecipientAddressResponse_v202309
    {
        [JsonProperty("full_address")]
        public string FullAddress { get; set; }
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
        [JsonProperty("address_detail")]
        public string AddressDetail { get; set; }
        [JsonProperty("region_code")]
        public string RegionCode { get; set; }
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }
        [JsonProperty("address_line4")]
        public string AddressLine4 { get; set; }
        [JsonProperty("district_info")]
        public List<ZTiktokShopOrderListDataOrdersDistrictInfoResponse_v202309> DistrictInfo { get; set; }
    }
    public class ZTiktokShopOrderListDataOrdersLineItemsResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("sku_id")]
        public string SKUID { get; set; }
        [JsonProperty("display_status")]
        public string DisplayStatus { get; set; }
        [JsonProperty("product_id")]
        public string ProductID { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("seller_sku")]
        public string SellerSKU { get; set; }
        [JsonProperty("sku_image")]
        public string SKUImage { get; set; }
        [JsonProperty("sku_type")]
        public string SKUType { get; set; }
        [JsonProperty("original_price")]
        public string OriginalPrice { get; set; }
        [JsonProperty("sale_price")]
        public string SalePrice { get; set; }
        [JsonProperty("platform_discount")]
        public string PlatformDiscount { get; set; }
        [JsonProperty("seller_discount")]
        public string SellerDiscount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }
        [JsonProperty("cancel_user")]
        public string CancelUser { get; set; }
        [JsonProperty("rts_time")]
        public int RtsTime { get; set; }
        [JsonProperty("package_status")]
        public string PackageStatus { get; set; }
        [JsonProperty("shipping_provider_id")]
        public string ShippingProviderID { get; set; }
        [JsonProperty("shipping_provider_name")]
        public string ShippingProviderName { get; set; }
        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }
        [JsonProperty("package_id")]
        public string PackageID { get; set; }
        [JsonProperty("is_gift")]
        public Boolean IsGift { get; set; }
        [JsonProperty("item_tax")]
        public List<ZTiktokShopOrderListDataOrdersLineItemsItemTaxResponse_v202309> ItemTax { get; set; }
        [JsonProperty("small_order_fee")]
        public string SmallOrderFee { get; set; }
    }
    public class ZTiktokShopOrderListDataOrdersDistrictInfoResponse_v202309
    {
        [JsonProperty("address_level_name")]
        public string AddressLevelName { get; set; }
        [JsonProperty("address_name")]
        public string AddressName { get; set; }
    }
    public class ZTiktokShopOrderListDataOrdersPackagesResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }
    public class ZTiktokShopOrderListDataOrdersLineItemsItemTaxResponse_v202309
    {
        [JsonProperty("tax_type")]
        public string TaxType { get; set; }
        [JsonProperty("tax_amount")]
        public string TaxAmount { get; set; }
    }
}
