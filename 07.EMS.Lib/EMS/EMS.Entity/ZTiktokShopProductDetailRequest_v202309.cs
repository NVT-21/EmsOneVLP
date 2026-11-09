using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopProductDetailRequest_v202309
    {
    }

    public class ZTiktokShopProductDetailResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopProductDetailDataResponse_v202309 Data { get; set; }
    }

    public class ZTiktokShopProductDetailDataResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("category_chains")]
        public List<ZTiktokShopProductDetailDataCategoryCategoryChainsResponse_v202309> CategoryChains { get; set; }

        [JsonProperty("brand")]
        public ZTiktokShopProductDetailDataBrandResponse_v202309 Brand { get; set; }

        [JsonProperty("main_images")]
        public List<ZTiktokShopProductDetailDataMainImagesResponse_v202309> MainImages { get; set; }

        [JsonProperty("video")]
        public ZTiktokShopProductDetailDataVideoResponse_v202309 Video { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("package_dimensions")]
        public ZTiktokShopProductDetailDataPackageDimensionResponse_v202309 PackageDimension { get; set; }

        [JsonProperty("package_weight")]
        public ZTiktokShopProductDetailDataPackageWeightResponse_v202309 PackageWeight { get; set; }

        [JsonProperty("skus")]
        public List<ZTiktokShopProductDetailDataSKUSResponse_v202309> SKUS { get; set; }

        [JsonProperty("certifications")]
        public List<ZTiktokShopProductDetailDataCertificationsResponse_v202309> Certifications { get; set; }

        [JsonProperty("size_chart")]
        public ZTiktokShopProductDetailDataSizeChartResponse_v202309 SizeChart { get; set; }

        [JsonProperty("is_cod_allowed")]
        public Boolean IsCODAllowed { get; set; }

        [JsonProperty("product_attributes")]
        public List<ZTiktokShopProductDetailDataProductAttributesResponse_v202309> ProductAttributes { get; set; }

        [JsonProperty("audit_failed_reasons")]
        public List<ZTiktokShopProductDetailDataAuditFailedReasonsResponse_v202309> AuditFailedReasons { get; set; }

        [JsonProperty("update_time")]
        public int UpdateTime { get; set; }

        [JsonProperty("create_time")]
        public int CreateTime { get; set; }

        [JsonProperty("delivery_options")]
        public List<ZTiktokShopProductDetailDataDeliveryOptionsResponse_v202309> DeliveryOptions { get; set; }

        [JsonProperty("external_product_id")]
        public string ExternalProductID { get; set; }
    }

    public class ZTiktokShopProductDetailDataBrandResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ZTiktokShopProductDetailDataCategoryCategoryChainsResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("parent_id")]
        public string ParentID { get; set; }

        [JsonProperty("local_name")]
        public string LocalName { get; set; }

        [JsonProperty("is_leaf")]
        public Boolean IsLeaf { get; set; }

    }

    public class ZTiktokShopProductDetailDataMainImagesResponse_v202309
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("thumb_urls")]
        public List<string> ThumbUrls { get; set; }

        [JsonProperty("uri")]
        public string URI { get; set; }

        [JsonProperty("urls")]
        public List<string> Urls { get; set; }
    }

    public class ZTiktokShopProductDetailDataVideoResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("cover_url")]
        public string CoverUrl { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }
    }

    public class ZTiktokShopProductDetailDataPackageDimensionResponse_v202309
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
    
    public class ZTiktokShopProductDetailDataPackageWeightResponse_v202309
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUSResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("seller_sku")]
        public string SellerSKU { get; set; }

        [JsonProperty("price")]
        public ZTiktokShopProductDetailDataSKUSPriceResponse_v202309 Price { get; set; }

        [JsonProperty("inventory")]
        public List<ZTiktokShopProductDetailDataSKUSInventoryResponse_v202309> Inventory { get; set; }

        [JsonProperty("identifier_code")]
        public ZTiktokShopProductDetailDataSKUSIdentifierCodeResponse_v202309 IdentifierCode { get; set; }

        [JsonProperty("sales_attributes")]
        public List<ZTiktokShopProductDetailDataSKUSSalesAttributeResponse_v202309> SalesAttributes { get; set; }

        [JsonProperty("external_sku_id")]
        public string ExternalSKUID { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUSPriceResponse_v202309
    {
        [JsonProperty("tax_exclusive_price")]
        public string TaxExclusivePrice { get; set; }

        [JsonProperty("sale_price")]
        public string SalePrice { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUSInventoryResponse_v202309
    {
        [JsonProperty("warehouse_id")]
        public string WarehouseID { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUSIdentifierCodeResponse_v202309
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUSSalesAttributeResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value_id")]
        public string ValueID { get; set; }

        [JsonProperty("value_name")]
        public string ValueName { get; set; }

        [JsonProperty("sku_img")]
        public ZTiktokShopProductDetailDataSKUSSalesAttributeSKUImgResponse_v202309 SKUImg { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUSSalesAttributeSKUImgResponse_v202309
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("thumb_urls")]
        public List<string> ThumbUrls { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("urls")]
        public List<string> Urls{ get; set; }
    }

    public class ZTiktokShopProductDetailDataCertificationsResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("files")]
        public List<ZTiktokShopProductDetailDataProductCertificationsFileResponse_v202309> Files { get; set; }

        [JsonProperty("images")]
        public List<ZTiktokShopProductDetailDataProductCertificationsImageResponse_v202309> Images { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductCertificationsFileResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("urls")]
        public List<string> Urls { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductCertificationsImageResponse_v202309
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("thumb_urls")]
        public List<string> ThumbUrls { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("urls")]
        public List<string> Urls { get; set; }
    }

    public class ZTiktokShopProductDetailDataSizeChartResponse_v202309
    {
        [JsonProperty("image")]
        public ZTiktokShopProductDetailDataSizeChartImageResponse_v202309 Image { get; set; }

        [JsonProperty("template")]
        public ZTiktokShopProductDetailDataSizeChartTemplateResponse_v202309 Template { get; set; }
    }

    public class ZTiktokShopProductDetailDataSizeChartImageResponse_v202309
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("thumb_urls")]
        public List<string> ThumbUrls { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("urls")]
        public List<string> Urls { get; set; }
    }

    public class ZTiktokShopProductDetailDataSizeChartTemplateResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductAttributesResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("values")]
        public List<ZTiktokShopProductDetailDataProductAttributesValuesResponse_v202309> Values { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductAttributesValuesResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ZTiktokShopProductDetailDataAuditFailedReasonsResponse_v202309
    {
        [JsonProperty("position")]
        public string Position { get; set; }
        
        [JsonProperty("reasons")]
        public List<string> Reasons { get; set; }

        [JsonProperty("suggestions")]
        public List<string> Suggestions { get; set; }
    }

    public class ZTiktokShopProductDetailDataDeliveryOptionsResponse_v202309
    {
        [JsonProperty("id")]
        public string DeliveryServiceID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_available")]
        public Boolean IsAvailable { get; set; }
    }

}
