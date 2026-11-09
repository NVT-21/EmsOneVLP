using EMS.API.Gateway.Repositories;
using EMS.Entity;
using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.API.Gateway.Controllers
{
    [RoutePrefix("api/Helper")]
    public class HelperController : ApiController
    {
        #region CUSTOMER
        [Route(CommandCodeConst.CUSTOMER_ADDNEW)]
        public string CUSTOMER_ADDNEW(ZCustomerAddNewRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.GET_BY_MOBILE_NUMBER)]
        public ZCustomerGetByMobileNumberResponse GET_BY_MOBILE_NUMBER(ZCustomerGetByMobileNumberRequest req)
        {
            return new ZCustomerGetByMobileNumberResponse();
        }

        [Route(CommandCodeConst.CUSTOMER_STICKER_LIST)]
        public List<ZCustomerStickerListResponse> CUSTOMER_STICKER_LIST(ZCustomerStickerListRequest req)
        {
            return new List<ZCustomerStickerListResponse>();
        }

        #endregion

        #region SHOP
        [Route(CommandCodeConst.SHOP_REGISTER)]
        public string SHOP_REGISTER(ZShopRegisterRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.SHOP_GET_BY_ID)]
        public ZShopGetByIDResponse SHOP_GET_BY_ID(ZShopGetByIDRequest req)
        {
            return new ZShopGetByIDResponse();
        }

        [Route(CommandCodeConst.SHOP_GET_HISTORY_BY_ID)]
        public ZShopGetHistoryByIDResponse SHOP_GET_HISTORY_BY_ID(ZShopGetHistoryByIDRequest req)
        {
            return new ZShopGetHistoryByIDResponse();
        }

        [Route(CommandCodeConst.SHOP_CHANGE_PICK_UP_ADDRESS)]
        public string SHOP_CHANGE_PICK_UP_ADDRESS(ZShopChangePickUpAddressRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.SHOP_GET_PICKUP_INFORMATION)]
        public ZShopGetPickupInformationResponse SHOP_GET_PICKUP_INFORMATION(ZShopGetPickupInformationRequest req)
        {
            return new ZShopGetPickupInformationResponse();
        }

        [Route(CommandCodeConst.SHOP_GROUP_PERMISSION_LIST)]
        public List<ZShopGroupPermissionListResponse> SHOP_GROUP_PERMISSION_LIST(ZShopGroupPermissionListRequest req)
        {
            return new List<ZShopGroupPermissionListResponse>();
        }

        [Route(CommandCodeConst.SHOP_TRANSPORT_SERVICE_BY_SHOP_ID)]
        public List<ZShopTransportServiceByShopIDResponse> SHOP_TRANSPORT_SERVICE_BY_SHOP_ID(ZShopTransportServiceByShopIDRequest req)
        {
            return new List<ZShopTransportServiceByShopIDResponse>();
        }

        [Route(CommandCodeConst.SHOP_REMOVE)]
        public string SHOP_REMOVE(ZShopRemoveRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.SHOP_LOCK)]
        public string SHOP_LOCK(ZShopLockRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.SHOP_GET_LIST_CRM_OR_PP_CODE)]
        public List<ZShopGetListCrmOrPaypostCodeResponse> SHOP_GET_LIST_CRM_OR_PP_CODE(ZShopGetListCrmOrPaypostCodeRequest req)
        {
            return new List<ZShopGetListCrmOrPaypostCodeResponse>();
        }

        [Route(CommandCodeConst.SHOP_FORGET_PASSWORD_BY_EMAIL)]
        public string SHOP_FORGET_PASSWORD_BY_EMAIL(ZShopForgetPasswordByEmailRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.SHOP_FORGET_PASSWORD_UPDATE)]
        public string SHOP_FORGET_PASSWORD_UPDATE(ZShopForgetPasswordUpdateRequest req)
        {
            return string.Empty;
        }
        #endregion

        #region EMPLOYEE
        [Route(CommandCodeConst.EMPLOYEE_LOGIN)]
        public ZEmployeeLoginResponse EMPLOYEE_LOGIN(ZEmployeeLoginRequest req)
        {
            return new ZEmployeeLoginResponse();
        }

        [Route(CommandCodeConst.EMPLOYEE_LIST)]
        public List<ZEmployeeListResponse> EMPLOYEE_LIST(ZEmployeeListRequest req)
        {
            return new List<ZEmployeeListResponse>();
        }

        [Route(CommandCodeConst.EMPLOYEE_SHOP_ADD)]
        public string EMPLOYEE_SHOP_ADD(ZEmployeeShopAddRequest req)
        {
            return string.Empty;
        }

        //[Route(CommandCodeConst.EMPLOYEE_COUNT_ACCOUNT)]
        //public ZEmployeeCountAccountResponse EMPLOYEE_COUNT_ACCOUNT(ZEmployeeCountAccountRequest req)
        //{
        //    return new ZEmployeeCountAccountResponse();
        //}
        //[Route(CommandCodeConst.EMPLOYEE_LIST_ACCOUNT)]
        //public ZEmployeeListAcountResponse EMPLOYEE_LIST_ACCOUNT(ZEmployeeListAcountRequest req)
        //{
        //    return new ZEmployeeListAcountResponse();
        //}

        [Route(CommandCodeConst.EMPLOYEE_CHANGE_PASSWORD)]
        public string EMPLOYEE_CHANGE_PASSWORD(ZEmployeeChangePasswordRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.EMPLOYEE_CHANGE_IS_LOCK)]
        public string EMPLOYEE_CHANGE_IS_LOCK(ZEmployeeChangeIsLockRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.EMPLOYEE_REMOVE)]
        public string EMPLOYEE_REMOVE(ZEmployeeRemoveRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.EMPLOYEE_CHECK_CONDITION)]
        public string EMPLOYEE_CHECK_CONDITION(ZEmployeeCheckConditionRequest req)
        {
            return string.Empty;
        }

        #endregion

        #region PROVINCE
        [Route(CommandCodeConst.PROVINCE_LIST)]
        public List<ZProvinceListResponse> PROVINCE_LIST(ZProvinceListRequest req)
        {
            return new List<ZProvinceListResponse>();
        }
        #endregion

        #region DISTRICT
        [Route(CommandCodeConst.DISTRICT_GET_BY_PROVINCE_ID)]
        public List<ZDistrictListResponse> DISTRICT_GET_BY_PROVINCE_ID(ZDistrictListRequest req)
        {
            return new List<ZDistrictListResponse>();
        }
        #endregion

        #region WARD
        [Route(CommandCodeConst.WARD_GETS)]
        public List<ZWardGetsResponse> WARD_GETS()
        {
            return new List<ZWardGetsResponse>();
        }
        [Route(CommandCodeConst.WARD_GET_BY_DISTRICT_ID)]
        public List<ZWardListResponse> WARD_GET_BY_DISTRICT_ID(ZWardListRequest req)
        {
            return new List<ZWardListResponse>();
        }
        #endregion

        #region ORDER
        [Route(CommandCodeConst.ORDER_SEARCH)]
        public List<ZOrderFESearchListResponse> ORDER_SEARCH(ZOrderFESearchListRequest req)
        {
            return new List<ZOrderFESearchListResponse>();
        }

        [Route(CommandCodeConst.ORDER_SEARCH_PRODUCT)]
        public List<ZOrderSearchProductResponse> ORDER_SEARCH_PRODUCT(ZProductListRequest req)
        {
            return new List<ZOrderSearchProductResponse>();
        }

        [Route(CommandCodeConst.ORDER_ADD)]
        public string ORDER_ADD(ZOrderAddRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.ORDER_ADD_AND_SEND_TRANSPORT)]
        public string ORDER_ADD_AND_SEND_TRANSPORT(ZOrderAddRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.ORDER_SEND_TRANSPORT)]
        public string ORDER_SEND_TRANSPORT(ZOrderSendTransportRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.ORDER_CANCEL_TRANSPORT)]
        public string ORDER_CANCEL_TRANSPORT(ZOrderCancelTransportRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.ORDER_GET_BY_ID)]
        public ZOrderGetByIDResponse ORDER_GET_BY_ID(ZOrderGetByIDRequest req)
        {
            return new ZOrderGetByIDResponse();
        }

        //[Route(CommandCodeConst.ORDER_SEARCH_INFO_SEND_REQUEST_TRANSPORT)]
        //public ZOrderSendTransportInfoResponse ORDER_SEARCH_INFO_SEND_REQUEST_TRANSPORT(ZOrderSendTransportInfoRequest req)
        //{
        //    return new ZOrderSearchInfoSendRequestTransportResponse();
        //}

        [Route(CommandCodeConst.ORDER_GET_BY_SHIPPING_CODE)]
        public ZOrderGetByShippingCodeResponse ORDER_GET_BY_SHIPPING_CODE(ZOrderGetByShippingCodeRequest req)
        {
            return new ZOrderGetByShippingCodeResponse();
        }

        [Route(CommandCodeConst.ORDER_DETAIL_BY_SHIPPING_CODE)]
        public ZOrderDetailByShippingCodeResponse ORDER_DETAIL_BY_SHIPPING_CODE(ZOrderDetailByShippingCodeRequest req)
        {
            return new ZOrderDetailByShippingCodeResponse();
        }
        #endregion

        #region PROVIDER
        [Route(CommandCodeConst.PROVIDER_LIST)]
        public List<ZProviderSearchListResponse> PROVIDER_LIST(ZProviderSearchListRequest req)
        {
            return new List<ZProviderSearchListResponse>();
        }

        [Route(CommandCodeConst.PROVIDER_ADD)]
        public string PROVIDER_LIST(ZProviderAddRequest req)
        {
            return string.Empty;
        }


        [Route(CommandCodeConst.PROVIDER_GET_BY_ID)]
        public ZProviderGetByIDResponse PROVIDER_LIST(ZProviderGetByIDRequest req)
        {
            return new ZProviderGetByIDResponse();
        }

        [Route(CommandCodeConst.PROVIDER_EDIT)]
        public string PROVIDER_EDIT(ZProviderEditRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.PROVIDER_CHANGE_IS_LOCK)]
        public string PROVIDER_CHANGE_IS_LOCK(ZProviderChangeIsLockRequest req)
        {
            return string.Empty;
        }

        #endregion

        #region WAREHOUSE
        [Route(CommandCodeConst.WAREHOUSE_LIST)]
        public List<ZWareHouseSearchListResponse> WAREHOUSE_LIST(ZWareHouseSearchListRequest req)
        {
            return new List<ZWareHouseSearchListResponse>();
        }

        [Route(CommandCodeConst.WAREHOUSE_ADDNEW)]
        public string WAREHOUSE_ADDNEW(ZWareHouseAddNewRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.WAREHOUSE_EDIT)]
        public string WAREHOUSE_EDIT(ZWareHouseEditRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.WAREHOUSE_GET_BY_ID)]
        public ZWareHouseGetByIDResponse WAREHOUSE_GET_BY_ID(ZWareHouseGetByIDRequest req)
        {
            return new ZWareHouseGetByIDResponse();
        }

        [Route(CommandCodeConst.WAREHOUSE_CHANGE_IS_LOCK)]
        public string WAREHOUSE_CHANGE_IS_LOCK(ZWareHouseChangeIsLockRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.WAREHOUSE_IS_DEFAULT)]
        public string WAREHOUSE_IS_DEFAULT(ZWarehouseIsDefaultRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.WAREHOUSE_GET_DEFAULT_WAREHOUSE)]
        public ZWarehouseGetDefaultWarehouseResponse WAREHOUSE_GET_DEFAULT_WAREHOUSE(ZWarehouseGetDefaultWarehouseRequest req)
        {
            return new ZWarehouseGetDefaultWarehouseResponse();
        }

        #endregion

        #region STATISTICAL
        [Route(CommandCodeConst.STATISTICAL_TRANSPORT)]
        public List<ZStatisticalSearchTransportResponse> STATISTICAL_TRANSPORT(ZStatisticalSearchTransportRequest req)
        {
            return new List<ZStatisticalSearchTransportResponse>();
        }

        [Route(CommandCodeConst.STATISTICAL_ORDER)]
        public List<ZStatisticalSearchOrderResponse> STATISTICAL_ORDER(ZStatisticalSearchOrderRequest req)
        {
            return new List<ZStatisticalSearchOrderResponse>();
        }

        [Route(CommandCodeConst.STATISTICAL_CUSTOMER)]
        public List<ZStatisticalSearchCustomerResponse> STATISTICAL_CUSTOMER(ZStatisticalSearchCustomerRequest req)
        {
            return new List<ZStatisticalSearchCustomerResponse>();
        }

        [Route(CommandCodeConst.STATISTICAL_CASHFLOW)]
        public List<ZStatisticalSearchCashflowResponse> STATISTICAL_CASHFLOW(ZStatisticalSearchCashflowRequest req)
        {
            return new List<ZStatisticalSearchCashflowResponse>();
        }

        [Route(CommandCodeConst.STATISTICAL_REQUEST_ORDER)]
        public List<ZRequestOrderDashboardResponse> STATISTICAL_REQUEST_ORDER(ZRequestOrderDashboardRequest req)
        {
            return new List<ZRequestOrderDashboardResponse>();
        }
        #endregion

        #region REQUEST_ORDER
        [Route(CommandCodeConst.REQUEST_ORDER_LIST)]
        public List<ZRequestOrderListResponse> REQUEST_ORDER_LIST(ZRequestOrderListRequest req)
        {
            return new List<ZRequestOrderListResponse>();
        }

        [Route(CommandCodeConst.REQUEST_ORDER_ADD)]
        public string REQUEST_ORDER_ADD(ZRequestOrderAddRequest req)
        {
            return string.Empty;
        }
        #endregion

        #region REQUEST_SUPPORT
        [Route(CommandCodeConst.REQUEST_SUPPORT_LIST)]
        public List<ZRequestSupportFESearchListResponse> REQUEST_SUPPORT_LIST(ZRequestSupportFESearchListRequest req)
        {
            return new List<ZRequestSupportFESearchListResponse>();
        }

        [Route(CommandCodeConst.REQUEST_SUPPORT_ADD)]
        public string REQUEST_SUPPORT_ADD(ZRequestSupportAddRequest req)
        {
            return string.Empty;
        }

        //[Route(CommandCodeConst.REQUEST_SUPPORT_SEARCH_INFO_SEND_REQUEST_SUPPORT)]
        //public ZRequestSupportSearchInfoSendRequestSupportResponse REQUEST_SUPPORT_SEARCH_INFO_SEND_REQUEST_SUPPORT(ZRequestSupportSearchInfoSendRequestSupportRequest req)
        //{
        //    return new ZRequestSupportSearchInfoSendRequestSupportResponse();
        //}
        #endregion

        #region REQUEST_SUPPORT_GROUP
        [Route(CommandCodeConst.REQUEST_SUPPORT_GROUP_LIST)]
        public List<ZRequestSupportGroupListResponse> REQUEST_SUPPORT_GROUP_LIST(ZRequestSupportGroupListRequest req)
        {
            return new List<ZRequestSupportGroupListResponse>();
        }
        #endregion

        #region PRODUCT
        [Route(CommandCodeConst.PRODUCT_LIST)]
        public List<ZProductListResponse> PRODUCT_LIST(ZProductListRequest req)
        {
            return new List<ZProductListResponse>();
        }

        [Route(CommandCodeConst.PRODUCT_ADD)]
        public string PRODUCT_ADD(ZProductAddRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.PRODUCT_GET_BY_ID)]
        public ZProductGetByIDResponse PRODUCT_GET_BY_ID(ZProductGetByIDRequest req)
        {
            return new ZProductGetByIDResponse();
        }

        [Route(CommandCodeConst.PRODUCT_EDIT)]
        public string PRODUCT_EDIT(ZProductEditRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.PRODUCT_DATA_IMAGES_BY_ID)]
        public ZProductDataImagesByIDResponse PRODUCT_DATA_IMAGES_BY_ID(ZProductDataImagesByIDRequest req)
        {
            return new ZProductDataImagesByIDResponse();
        }

        [Route(CommandCodeConst.PRODUCT_EDIT_IMAGES)]
        public string PRODUCT_EDIT_IMAGES(ZProductEditImagesRequest req)
        {
            return string.Empty;
        }


        #endregion

        #region PRODUCT_VARIANT
        [Route(CommandCodeConst.PRODUCT_VARIANT_GET_BY_SKU)]
        public ZProductVariantGetBySKUResponse PRODUCT_VARIANT_GET_BY_SKU(ZProductVariantGetBySKURequest req)
        {
            return new ZProductVariantGetBySKUResponse();
        }

        [Route(CommandCodeConst.PRODUCT_VARIANT_GET_BY_ID)]
        public ZProductVariantGetByIDResponse PRODUCT_VARIANT_GET_BY_ID(ZProductVariantGetByIDRequest req)
        {
            return new ZProductVariantGetByIDResponse();
        }


        [Route(CommandCodeConst.PRODUCT_VARIANT_GET_BY_PRODUCT_ID)]
        public List<ZProductVariantGetByProductIDResponse> PRODUCT_VARIANT_GET_BY_ID(ZProductVariantGetByProductIDRequest req)
        {
            return new List<ZProductVariantGetByProductIDResponse>();
        }

        [Route(CommandCodeConst.PRODUCT_VARIANT_ADD)]
        public string PRODUCT_VARIANT_ADD(ZProductVariantAddRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.PRODUCT_VARIANT_EDIT)]
        public string PRODUCT_VARIANT_EDIT(ZProductVariantEditRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.PRODUCT_VARIANT_REMOVE)]
        public string PRODUCT_VARIANT_REMOVE(ZProductVariantRemoveRequest req)
        {
            return string.Empty;
        }
        
        #endregion

        #region PRODUCT_OPTION
        [Route(CommandCodeConst.PRODUCT_OPTION_GET_BY_PRODUCT_ID)]
        public ZProductOptionGetByProductIDResponse PRODUCT_OPTION_GET_BY_PRODUCT_ID(ZProductOptionGetByProductIDRequest req)
        {
            return new ZProductOptionGetByProductIDResponse();
        }

        [Route(CommandCodeConst.PRODUCT_OPTION_ADD)]
        public string PRODUCT_OPTION_ADD(ZProductOptionAddRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.PRODUCT_OPTION_REMOVE_BY_NAME)]
        public string PRODUCT_OPTION_REMOVE_BY_NAME(ZProductOptionRemoveByNameRequest req)
        {
            return string.Empty;
        }
        
        #endregion

        #region PRODUCT_GROUP
        [Route(CommandCodeConst.PRODUCT_GROUP_LIST)]
        public List<ZProductGroupListResponse> PRODUCT_GROUP_LIST(ZProductGroupListRequest req)
        {
            return new List<ZProductGroupListResponse>();
        }

        [Route(CommandCodeConst.PRODUCT_GROUP_ADD)]
        public string PRODUCT_GROUP_ADD(ZProductGroupAddRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.PRODUCT_GROUP_GET_BY_ID)]
        public ZProductGroupGetByIDResponse PRODUCT_GROUP_GET_BY_ID(ZProductGroupGetByIDRequest req)
        {
            return new ZProductGroupGetByIDResponse();
        }

        [Route(CommandCodeConst.PRODUCT_GROUP_EDIT)]
        public string PRODUCT_GROUP_EDIT(ZProductGroupEditRequest req)
        {
            return string.Empty;
        }


        #endregion

        #region PRODUCT_UNIT
        [Route(CommandCodeConst.PRODUCT_UNIT_SEARCH_LIST)]
        public List<ZProductUnitSearchListResponse> PRODUCT_UNIT_SEARCH_LIST(ZProductUnitSearchListRequest req)
        {
            return new List<ZProductUnitSearchListResponse>();
        }

        [Route(CommandCodeConst.PRODUCT_UNIT_ADD)]
        public string PRODUCT_UNIT_ADD(ZProductUnitAddRequest req)
        {
            return string.Empty;
        }
        #endregion

        #region TRANSPORT_POSTAGE
        [Route(CommandCodeConst.TRANSPORT_POSTAGE_CHARGED)]
        public ZTransportPostageChargedResponse TRANSPORT_POSTAGE_CHARGED(ZTransportPostageChargedRequest req)
        {
            return new ZTransportPostageChargedResponse();
        }
        #endregion

        #region TRANSPORT_EMS
        [Route(CommandCodeConst.TRANSPORT_EMS_BARCODE_GET)]
        public string TRANSPORT_EMS_BARCODE_GET(ZTransportGetEmsBarCodeRequest req)
        {
            return string.Empty;
        }
        #endregion

        #region TRANSPORT_ORDER
        [Route(CommandCodeConst.TRANSPORT_ORDER_UPDATE_STATUS)]
        public string TRANSPORT_ORDER_UPDATE_STATUS(ZTransportUpdateOrderRequest req)
        {
            return string.Empty;
        }
        #endregion

        #region INVENTORY
        [Route(CommandCodeConst.INVENTORY_CONFIRM)]
        public string INVENTORY_CONFIRM(ZInventoryConfirmRequest req)
        {
            return string.Empty;
        }
        #endregion

        #region COLLECTION_POST_OFFICE
        [Route(CommandCodeConst.COLLECTION_POST_OFFICE_SEARCH_LIST)]
        public List<ZCollectionPostOfficeSearchListResponse> COLLECTION_POST_OFFICE_SEARCH_LIST(ZCollectionPostOfficeSearchListRequest req)
        {
            return new List<ZCollectionPostOfficeSearchListResponse>();
        }
        #endregion

        #region OTP
        [Route(CommandCodeConst.OTP_VERIFY_CODE)]
        public ZOtpVerifyCodeResponse OTP_VERIFY_CODE(ZOtpVerifyCodeRequest req)
        {
            return new ZOtpVerifyCodeResponse();
        }
        #endregion

        #region ORDER_BATCH
        [Route(CommandCodeConst.ORDER_BATCH_ADD)]
        public string ORDER_BATCH_ADD(ZOrderBatchAddRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.ORDER_BATCH_EDIT_INFO)]
        public ZOrderBatchEditInfoResponse ORDER_BATCH_EDIT_INFO(ZOrderBatchEditInfoRequest req)
        {
            return new ZOrderBatchEditInfoResponse();
        }

        [Route(CommandCodeConst.ORDER_BATCH_EDIT)]
        public string ORDER_BATCH_EDIT(ZOrderBatchEditRequest req)
        {
            return string.Empty;
        }

        [Route(CommandCodeConst.ORDER_BATCH_COPPY_INFO)]
        public ZOrderBatchCoppyInfoResponse ORDER_BATCH_COPPY_INFO(ZOrderBatchCoppyInfoRequest req)
        {
            return new ZOrderBatchCoppyInfoResponse();
        }

        [Route(CommandCodeConst.ORDER_BATCH_DETAIL_BY_BATCH_CODE)]
        public ZOrderBatchDetailByBatchCodeResponse ORDER_BATCH_DETAIL_BY_BATCH_CODE(ZOrderBatchDetailByBatchCodeRequest req)
        {
            return new ZOrderBatchDetailByBatchCodeResponse();
        }
        #endregion

        
    }
}
