using EMS.Entity;
using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.API.PartnerGateway.Controllers
{
    [RoutePrefix("api/Helper")]
    public class HelpController : ApiController
    {
        #region PARTNER_ORDER
        [Route("PARTNER_ORDER_ADD")]
        public ZPartnerOrderAddResponse PARTNER_ORDER_ADD(ZPartnerOrderAddRequest req)
        {
            return new ZPartnerOrderAddResponse();
        }

        //[Route("PARTNER_ORDER_ADD_V2")]
        //public ZPartnerOrderAddV2Response PARTNER_ORDER_ADD_V2(ZPartnerOrderAddV2Request req)
        //{
        //    return new ZPartnerOrderAddV2Response();
        //}

        [Route("PARTNER_VIB_ORDER_ADD")]
        public ZPartnerVIBOrderAddResponse PARTNER_VIB_ORDER_ADD(ZPartnerVIBOrderAddRequest req)
        {
            return new ZPartnerVIBOrderAddResponse();
        }

        [Route("PARTNER_TEMU_ORDER_ADD")]
        public ZPartnerTemuOrderAddResponse PARTNER_TEMU_ORDER_ADD(ZPartnerTemuOrderAddRequest req)
        {
            return new ZPartnerTemuOrderAddResponse();
        }

        [Route("PARTNER_ORDER_CANCEL")]
        public string PARTNER_ORDER_CANCEL(ZPartnerOrderCancelRequest req)
        {
            return string.Empty;
        }

        [Route("PARTNER_ORDER_INFO")]
        public ZPartnerOrderInfoResponse PARTNER_ORDER_INFO(ZPartnerOrderInfoRequest req)
        {
            return new ZPartnerOrderInfoResponse();
        }

        [Route("PARTNER_ORDER_PRINT")]
        public ZPartnerOrderPrintResponse PARTNER_ORDER_PRINT(ZPartnerOrderPrintRequest req)
        {
            return new ZPartnerOrderPrintResponse();
        }
        [Route("PARTNER_ORDER_PRINT_V2")]
        public ZPartnerOrderPrintResponse PARTNER_ORDER_PRINT_V2(ZPartnerOrderPrintRequest req)
        {
            return new ZPartnerOrderPrintResponse();
        }

        [Route("PARTNER_ORDER_CREATE_SHIPPING_CODE")]
        public ZPartnerOrderCreateShippingCodeResponse PARTNER_ORDER_CREATE_SHIPPING_CODE(ZPartnerOrderCreateShippingCodeRequest req)
        {
            return new ZPartnerOrderCreateShippingCodeResponse();
        }
        #endregion

        #region PARTNER_TRANSPORT_SERVICE
        [Route("PARTNER_TRANSPORT_SERVICE_SEARCH_LIST")]
        public ZTransportServiceSearchListByShopIDResponse PARTNER_TRANSPORT_SERVICE_SEARCH_LIST(ZTransportServiceSearchListByShopIDRequest req)
        {
            return new ZTransportServiceSearchListByShopIDResponse();
        }
        #endregion

        #region PARTNER_TRANSPORT_POSTAGE
        [Route("PARTNER_TRANSPORT_POSTAGE_CHARGED")]
        public ZPartnerTransportPostageChargedResponse PARTNER_TRANSPORT_POSTAGE_CHARGED(ZPartnerTransportPostageChargedRequest req)
        {
            return new ZPartnerTransportPostageChargedResponse();
        }
        #endregion

        #region PARTNER_WAREHOUSE
        [Route("PARTNER_WAREHOUSE_SEARCH_LIST")]
        public List<ZPartnerWareHouseSearchListResponse> PARTNER_WAREHOUSE_SEARCH_LIST(ZPartnerWareHouseSearchListRequest req)
        {
            return new List<ZPartnerWareHouseSearchListResponse>();
        }

        [Route("PARTNER_WAREHOUSE_ADD")]
        public ZPartnerWareHouseAddResponse PARTNER_WAREHOUSE_ADD(ZPartnerWareHouseAddRequest req)
        {
            return new ZPartnerWareHouseAddResponse();
        }
        #endregion

        #region PARTNER_PRODUCT
        [Route("PARTNER_PRODUCT_SEARCH_LIST")]
        public List<ZPartnerProductSearchListResponse> PARTNER_PRODUCT_SEARCH_LIST(ZPartnerProductSearchListRequest req)
        {
            return new List<ZPartnerProductSearchListResponse>();
        }
        #endregion

        #region PARTNER_ADDRESS
        [Route("PARTNER_ADDRESS_SEARCH")]
        public ZEMSMapSearchDataResponse PARTNER_ADDRESS_SEARCH(ZEMSMapSearchRequest req)
        {
            return new ZEMSMapSearchDataResponse();
        }
        #endregion

        #region PARTNER_PROVINCE
        [Route("PARTNER_PROVINCE_SEARCH_LIST")]
        public ZProvinceListResponse PARTNER_PROVINCE_SEARCH_LIST(ZProvinceListRequest req)
        {
            return new ZProvinceListResponse();
        }
        #endregion

        #region PARTNER_DISTRICT
        [Route("PARTNER_DISTRICT_GET_BY_PROVINCE_ID")]
        public ZDistrictGetByProvinceIDResponse PARTNER_DISTRICT_GET_BY_PROVINCE_ID(ZDistrictGetByProvinceIDRequest req)
        {
            return new ZDistrictGetByProvinceIDResponse();
        }
        #endregion

        #region PARTNER_WARD
        [Route("PARTNER_WARD_GET_BY_DISTRICT_ID")]
        public ZWardGetByDistrictIDResponse PARTNER_WARD_GET_BY_DISTRICT_ID(ZWardGetByDistrictIDRequest req)
        {
            return new ZWardGetByDistrictIDResponse();
        }
        #endregion
    }
}