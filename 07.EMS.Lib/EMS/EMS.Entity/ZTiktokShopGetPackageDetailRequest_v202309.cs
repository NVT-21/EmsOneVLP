using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopGetPackageDetailRequest_v202309
    {
        public string package_id { get; set; }
    }
    public class ZTiktokShopGetPackageDetailResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopGetPackageDetailDataResponse_v202309 Data { get; set; }
    }
    public class ZTiktokShopGetPackageDetailDataResponse_v202309
    {
        [JsonProperty("package_id")]
        public string PackageID { get; set; }
        [JsonProperty("orders")]
        public List<ZTiktokShopGetPackageDetailDataOrdersResponse_v202309> Orders { get; set; }
        [JsonProperty("PackageStatus")]
        public string PackageStatus { get; set; }
        [JsonProperty("split_and_combine_tag")]
        public string SplitAndCombineTag { get; set; }
        [JsonProperty("has_multi_skus")]
        public Boolean HasMultiSKUS { get; set; }
        [JsonProperty("note_tag")]
        public string NoteTag { get; set; }
        [JsonProperty("shipping_provider_name")]
        public string ShippingProviderName { get; set; }
        [JsonProperty("shipping_provider_id")]
        public string ShippingProviderID { get; set; }
        [JsonProperty("shipping_type")]
        public string ShippingType { get; set; }
        [JsonProperty("delivery_option_name")]
        public string DeliveryOptionName { get; set; }
        [JsonProperty("delivery_option_id")]
        public string DeliveryOptionID { get; set; }
        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; set; }
        [JsonProperty("handover_method")]
        public string HandoverMethod { get; set; }
        [JsonProperty("pickup_slot")]
        public ZTiktokShopGetPackageDetailDataPickupSlotResponse_v202309 PickupSlot { get; set; }
        [JsonProperty("create_time")]
        public int CreateTime { get; set; }
        [JsonProperty("update_time")]
        public int UpdateTime { get; set; }
        [JsonProperty("order_line_item_ids")]
        public List<string> OrderLineItemIDS { get; set; }
        [JsonProperty("recipient_address")]
        public ZTiktokShopGetPackageDetailDataRecipientAddressResponse_v202309 RecipientAddress { get; set; }
        [JsonProperty("sender_address")]
        public ZTiktokShopGetPackageDetailDataRecipientAddressResponse_v202309 SenderAddress { get; set; }
        [JsonProperty("weight")]
        public ZTiktokShopGetPackageDetailDataWeightResponse_v202309 Weight { get; set; }
        [JsonProperty("dimension")]
        public ZTiktokShopGetPackageDetailDataDimensionResponse_v202309 Dimension { get; set; }
    }
    public class ZTiktokShopGetPackageDetailDataOrdersResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("skus")]
        public List<ZTiktokShopGetPackageDetailDataOrdersSKUSResponse_v202309> SKUS { get; set; }
    }
    public class ZTiktokShopGetPackageDetailDataOrdersSKUSResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
    public class ZTiktokShopGetPackageDetailDataPickupSlotResponse_v202309
    {
        [JsonProperty("start_time")]
        public int StartTime { get; set; }
        [JsonProperty("EndTime")]
        public int EndTime { get; set; }
    }
    public class ZTiktokShopGetPackageDetailDataRecipientAddressResponse_v202309
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
    }
    public class ZTiktokShopGetPackageDetailDataWeightResponse_v202309
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
    public class ZTiktokShopGetPackageDetailDataDimensionResponse_v202309
    {
        [JsonProperty("length")]
        public string Length { get; set; }
        [JsonProperty("width")]
        public string Width { get; set; }
        [JsonProperty("height")]
        public string Height { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
}
