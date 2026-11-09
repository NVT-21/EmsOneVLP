using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopProductDetailRequest
    {
    }

    public class ZTiktokShopProductDetailResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopProductDetailDataResponse Data { get; set; }
    }

    public class ZTiktokShopProductDetailDataResponse
    {
        [JsonProperty("brand")]
        public ZTiktokShopProductDetailDataBrandResponse Brand { get; set; }

        [JsonProperty("category_list")]
        public List<ZTiktokShopProductDetailDataCategoryListResponse> CategoryList { get; set; }

        [JsonProperty("create_time")]
        public long CreateTime { get; set; }

        [JsonProperty("delivery_services")]
        public List<ZTiktokShopProductDetailDataDeliveryServiceResponse> DeliveryServices { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("exemption_of_identifier_code")]
        public ZTiktokShopProductDetailDataExemptionOfIdentifierCodeResponse ExemptionOfIdentifierCode { get; set; }

        [JsonProperty("images")]
        public List<ZTiktokShopProductDetailDataImageResponse> Images { get; set; }

        [JsonProperty("is_cod_open")]
        public Boolean IsCodOpen { get; set; }

        [JsonProperty("package_dimension_unit")]
        public string PackageDimensionUnit { get; set; }

        [JsonProperty("package_height")]
        public string PackageHeight { get; set; }

        [JsonProperty("package_length")]
        public string PackageLength { get; set; }

        [JsonProperty("package_weight")]
        public string PackageWeight { get; set; }

        [JsonProperty("package_width")]
        public string PackageWidth { get; set; }

        [JsonProperty("product_attributes")]
        public List<ZTiktokShopProductDetailDataProductAttributeResponse> ProductAttributes { get; set; }

        [JsonProperty("product_certifications")]
        public List<ZTiktokShopProductDetailDataProductCertificationResponse> ProductCertifications { get; set; }

        [JsonProperty("product_id")]
        public string ProductID { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("product_status")]
        public int ProductStatus { get; set; }

        [JsonProperty("qc_reasons")]
        public List<ZTiktokShopProductDetailDataQCReasonResponse> QCReasons { get; set; }

        [JsonProperty("size_chart")]
        public ZTiktokShopProductDetailDataSizeChartResponse SizeChart { get; set; }

        [JsonProperty("skus")]
        public List<ZTiktokShopProductDetailDataSKUResponse> SKUS { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }

        [JsonProperty("video")]
        public ZTiktokShopProductDetailDataVideoResponse Video { get; set; }

        [JsonProperty("warranty_period")]
        public ZTiktokShopProductDetailDataWarrantyPeriodResponse WarrantyPeriod { get; set; }

        [JsonProperty("warranty_policy")]
        public string WarrantyPolicy { get; set; }
    }

    public class ZTiktokShopProductDetailDataBrandResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class ZTiktokShopProductDetailDataCategoryListResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("is_leaf")]
        public Boolean IsLeaf { get; set; }

        [JsonProperty("local_display_name")]
        public string LocalDisplayName { get; set; }

        [JsonProperty("parent_id")]
        public string ParentID { get; set; }
    }

    public class ZTiktokShopProductDetailDataDeliveryServiceResponse
    {
        [JsonProperty("delivery_option_name")]
        public string DeliveryOptionName { get; set; }

        [JsonProperty("delivery_service_id")]
        public long DeliveryServiceID { get; set; }

        [JsonProperty("delivery_service_status")]
        public Boolean DeliveryServiceStatus { get; set; }

    }

    public class ZTiktokShopProductDetailDataExemptionOfIdentifierCodeResponse
    {
        [JsonProperty("exemption_reason")]
        public string ExemptionReason { get; set; }
    }

    public class ZTiktokShopProductDetailDataImageResponse
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("thumb_url_list")]
        public List<string> ThumbUrlList { get; set; }

        [JsonProperty("url_list")]
        public List<string> UrlList { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductAttributeResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("values")]
        public List<ZTiktokShopProductDetailDataProductAttributeValueResponse> Values { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductAttributeValueResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductCertificationResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("files")]
        public List<ZTiktokShopProductDetailDataProductCertificationFileResponse> Files { get; set; }

        [JsonProperty("images")]
        public List<ZTiktokShopProductDetailDataProductCertificationImageResponse> Images { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductCertificationFileResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("list")]
        public List<string> List { get; set; }
    }

    public class ZTiktokShopProductDetailDataProductCertificationImageResponse
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("thumb_url_list")]
        public string ThumbUrlList { get; set; }

        [JsonProperty("url_list")]
        public List<string> UrlList { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }

    public class ZTiktokShopProductDetailDataQCReasonResponse
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("sub_reasons")]
        public List<string> SubReasons { get; set; }
    }

    public class ZTiktokShopProductDetailDataSizeChartResponse
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("thumb_url_list")]
        public string ThumbUrlList { get; set; }

        [JsonProperty("url_list")]
        public List<string> UrlList { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("price")]
        public ZTiktokShopProductDetailDataSKUPriceResponse Price { get; set; }

        [JsonProperty("product_identifier_code")]
        public ZTiktokShopProductDetailDataSKUProductIdentifierCodeResponse ProductIdentifierCode { get; set; }

        [JsonProperty("sales_attributes")]
        public List<ZTiktokShopProductDetailDataSKUSalesAttributeResponse> SalesAttributes { get; set; }

        [JsonProperty("seller_sku")]
        public string SellerSKU { get; set; }

        [JsonProperty("stock_infos")]
        public List<ZTiktokShopProductDetailDataSKUStockInfoResponse> StockInfos { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUPriceResponse
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("original_price")]
        public string OriginalPrice { get; set; }

        [JsonProperty("price_include_vat")]
        public string PriceIncludeVat { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUProductIdentifierCodeResponse
    {
        [JsonProperty("identifier_code")]
        public string IdentifierCode { get; set; }

        [JsonProperty("identifier_code_type")]
        public string IdentifierCodeType { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUSalesAttributeResponse
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
        public ZTiktokShopProductDetailDataSKUSalesAttributeSKUImgResponse SKUImg { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUSalesAttributeSKUImgResponse
    {
        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("thumb_url_list")]
        public List<string> ThumbUrlList { get; set; }

        [JsonProperty("url_list")]
        public List<string> UrlList { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }
    }

    public class ZTiktokShopProductDetailDataSKUStockInfoResponse
    {
        [JsonProperty("available_stock")]
        public int AvailableStock { get; set; }

        [JsonProperty("warehouse_id")]
        public string WarehouseID { get; set; }

    }

    public class ZTiktokShopProductDetailDataVideoResponse
    {
        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("post_url")]
        public string PostUrl { get; set; }

        [JsonProperty("video_infos")]
        public List<ZTiktokShopProductDetailDataVideoVideoInfoResponse> VideoInfos { get; set; }

    }

    public class ZTiktokShopProductDetailDataVideoVideoInfoResponse
    {
        [JsonProperty("backup_url")]
        public string BackupUrl { get; set; }

        [JsonProperty("bitrate")]
        public string Bitrate { get; set; }

        [JsonProperty("file_hash")]
        public string FileHash { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("main_url")]
        public string MainUrl { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("url_expire")]
        public string UrlExpire { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }
    }

    public class ZTiktokShopProductDetailDataWarrantyPeriodResponse
    {
        [JsonProperty("warranty_description")]
        public string WarrantyDescription { get; set; }

        [JsonProperty("warranty_id")]
        public string WarrantyID { get; set; }

    }
}