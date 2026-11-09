using EMS.API.Gateway.Repositories;
using EMS.Entity;
using EMS.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web;
using System.Net.Http.Headers;

namespace EMS.API.Gateway.Controllers
{
    public class GatewayController : ApiController
    {
        [HttpPost]
        [Route("Execute")]
        public ZResponse Execute(ZRequest req)
        {
            

            ZResponse res = new ZResponse();
            //var header = HttpContext.Current.Request.Headers["DeviceName"];
            try
            {
                //string sign = Security.RSAEncrypt(req.Data, Security.PrivateKey);
                //string sign = Security.SignRSA(req.Data, Security.PrivateKey);
                //verify = Security.VerifyRSA(req.Data, req.Signature, Security.PublicKey);
                switch (req.Code)
                {
                    #region EMPLOYEE - Nhân viên
                    case "EMPLOYEE_LOGIN":
                        return EmployeeRepository.Login(req.Data);
                    case "EMPLOYEE_LOGIN_V2":
                        return EmployeeRepository.LoginV2(req.Data);
                    case "EMPLOYEE_COUNT_ACCOUNT":
                        return EmployeeRepository.CountAccount(req.Data);
                    case "EMPLOYEE_LIST_ACCOUNT":
                        return EmployeeRepository.ListAccount(req.Data);
                    case "EMPLOYEE_ADD_NEW":
                        return EmployeeRepository.AddNew(req.Data);
                    case "EMPLOYEE_SHOP_ADD":
                        return EmployeeRepository.ShopAdd(req.Data);
                    case "EMPLOYEE_LIST":
                        return EmployeeRepository.List(req.Data);
                    case "EMPLOYEE_EDIT":
                        return EmployeeRepository.Edit(req.Data);
                    case "EMPLOYEE_SHOP_EDIT":
                        return EmployeeRepository.ShopEdit(req.Data);
                    case "EMPLOYEE_GET_BY_ID":
                        return EmployeeRepository.GetByID(req.Data);
                    case "EMPLOYEE_GET_BY_MOBILE_NUMBER":
                        return EmployeeRepository.GetByMobileNumber(req.Data);
                    case "EMPLOYEE_GET_BY_GROUP_PERMISSION_ID":
                        return EmployeeRepository.GetByGroupPermissionID(req.Data);
                    case "EMPLOYEE_CHANGE_IS_LOCK":
                        return EmployeeRepository.ChangeIsLock(req.Data);
                    case "EMPLOYEE_RESET_PASSWORD":
                        return EmployeeRepository.ResetPassword(req.Data);
                    case "EMPLOYEE_CHANGE_PASSWORD":
                        return EmployeeRepository.ChangePassword(req.Data);
                    case "EMPLOYEE_CHANGE_PASSWORD_V2":
                        return EmployeeRepository.ChangePasswordV2(req.Data);
                    case "EMPLOYEE_REMOVE":
                        return EmployeeRepository.Remove(req.Data);
                    case "EMPLOYEE_TIME_ACTIVE_ADD":
                        return EmployeeRepository.TimeActiveAdd(req.Data);
                    case "EMPLOYEE_TIME_ACTIVE_EDIT":
                        return EmployeeRepository.TimeActiveEdit(req.Data);
                    case "EMPLOYEE_TIME_ACTIVE_GET_BY_EMPLOYEE_ID":
                        return EmployeeRepository.TimeActiveGetByEmployeeID(req.Data);
                    case "EMPLOYEE_CHECK_ACTIVE":
                        return EmployeeRepository.CheckActive(req.Data);
                    case "EMPLOYEE_CHECK_CONDITION":
                        return EmployeeRepository.CheckCondition(req.Data);
                    case "EMPLOYEE_LIST_EMPLOYEE_IN_PERMISSION":
                        return EmployeeRepository.ListEmployeeInPermission(req.Data);
                    case "EMPLOYEE_LIST_EMPLOYEE_BY_EMPLOYEE_GROUP_ID":
                        return EmployeeRepository.ListEmployeeByEmployeeGroupID(req.Data);
                    case "EMPLOYEE_CHANGE_READY_RECEIVE_REQUEST_ORDER":
                        return EmployeeRepository.ChangeReadyReceiveRequestOrder(req.Data);
                    #endregion

                    #region EMPLOYEE_GROUP - Nhóm nhân viên
                    case "EMPLOYEE_GROUP_LIST":
                        return EmployeeGroupRepository.List(req.Data);
                    case "EMPLOYEE_GROUP_ADD":
                        return EmployeeGroupRepository.Add(req.Data);
                    case "EMPLOYEE_GROUP_GET_BY_ID":
                        return EmployeeGroupRepository.GetByID(req.Data);
                    case "EMPLOYEE_GROUP_EDIT":
                        return EmployeeGroupRepository.Edit(req.Data);
                    case "EMPLOYEE_GROUP_CHANGE_IS_LOCK":
                        return EmployeeGroupRepository.ChangeIsLock(req.Data);
                    #endregion

                    #region USER - Tài khoản vận hành
                    case "USER_LOGIN":
                        return UserRepository.Login(req.Data);
                    case "USER_ADD":
                        return UserRepository.Add(req.Data);
                    case "USER_SEARCH_LIST":
                        return UserRepository.SearchList(req.Data);
                    case "USER_EDIT":
                        return UserRepository.Edit(req.Data);
                    case "USER_GET_BY_ID":
                        return UserRepository.GetByID(req.Data);
                    case "USER_CHANGE_TO_LOCKED":
                        return UserRepository.ChangeToLocked(req.Data);
                    case "USER_CHANGE_TO_UNLOCKED":
                        return UserRepository.ChangeToUnlocked(req.Data);
                    case "USER_RESET_PASSWORD":
                        return UserRepository.ResetPassword(req.Data);
                    case "USER_CHANGE_PASSWORD":
                        return UserRepository.ChangePassword(req.Data);
                    case "USER_CHECK_CONDITION":
                        return UserRepository.CheckCondition(req.Data);
                    case "USER_GET_BY_USERNAME":
                        return UserRepository.GetByUserName(req.Data);
                    #endregion

                    #region USER_GROUP - Nhóm tài khoản vận hành
                    case "USER_GROUP_SEARCH_LIST":
                        return UserGroupRepository.SearchList(req.Data);
                    #endregion

                    #region USER_IN_COLLECTION_POST_OFFICE - Tài khoản vận hành thuộc bưu cục thu gom
                    //case "USER_IN_COLLECTION_POST_OFFICE_SEARCH_LIST":
                    //    return UserInCollectionPostOfficeRepository.SearchList(req.Data);
                    case "USER_IN_COLLECTION_POST_OFFICE_SEARCH_LIST_BY_USER_ID":
                        return UserInCollectionPostOfficeRepository.SearchListByUserID(req.Data);
                    #endregion

                    #region CUSTOMER - Khách hàng
                    case "CUSTOMER_ADDNEW":
                        return CustomerRepository.AddNew(req.Data);
                    case "CUSTOMER_LIST":
                        return CustomerRepository.SearchList(req.Data);
                    case "CUSTOMER_SEARCH_LIST_V2":
                        return CustomerRepository.SearchListV2(req.Data);
                    case "CUSTOMER_EDIT":
                        return CustomerRepository.Edit(req.Data);
                    case "GET_BY_CUSTOMER_CODE":
                        return CustomerRepository.GetByCustomerCode(req.Data);
                    case "GET_BY_MOBILE_NUMBER":
                        return CustomerRepository.GetByMobileNumber(req.Data);
                    case "ADDNEW_GROUP":
                        return CustomerRepository.AddNewGroup(req.Data);
                    case "SEARCH_GROUP":
                        return CustomerRepository.SearchGroup(req.Data);
                    case "CUSTOMER_GET_BY_ID":
                        return CustomerRepository.GetByID(req.Data);
                    case "CUSTOMER_CHECK_BLACK_LIST":
                        return CustomerRepository.CheckBlackList(req.Data);
                    case "CUSTOMER_CHECK_ID_BY_MOBILE_NUMBER":
                        return CustomerRepository.CheckIDByMobileNumber(req.Data);
                    //case "CUSTOMER_GET_ID_BY_ORDER_CODE":
                    //    return CustomerRepository.GetCustomerIDByOrderCode(req.Data);
                    case "CUSTOMER_REMOVE_STICKER":
                        return CustomerRepository.RemoveSticker(req.Data);
                    case "CUSTOMER_IS_EXIST":
                        return CustomerRepository.IsExist(req.Data);
                    case "CUSTOMER_IMPORT_EXCEL":
                        return CustomerRepository.ImportExcel(req.Data);
                    #endregion

                    #region CUSTOMER_TEMP - Import Excel Khách hàng
                    case "CUSTOMER_TEMP_ADD_TEMP":
                        return CustomerTempRepository.AddTemp(req.Data);
                    case "CUSTOMER_TEMP_ADD_COMMIT":
                        return CustomerTempRepository.AddCommit(req.Data);
                    #endregion

                    #region CUSTOMER STICKER - Nhãn khách hàng
                    case "CUSTOMER_STICKER_LIST":
                        return CustomerStickerRepository.List(req.Data);
                    case "CUSTOMER_STICKER_ADD":
                        return CustomerStickerRepository.Add(req.Data);
                    case "CUSTOMER_STICKER_REMOVE":
                        return CustomerStickerRepository.Remove(req.Data);

                    #endregion

                    #region WAREHOUSE - Kho
                    case "WAREHOUSE_ADDNEW":
                        return WareHouseRepository.AddNew(req.Data);
                    case "WAREHOUSE_LIST":
                        return WareHouseRepository.SearchList(req.Data);
                    case "WAREHOUSE_LIST_PRODUCT_BY_WAREHOUSE_ID":
                        return WareHouseRepository.ListProductByWareHouseID(req.Data);
                    case "WAREHOUSE_GET_BY_ID":
                        return WareHouseRepository.GetByID(req.Data);
                    case "WAREHOUSE_LIST_PRODUCT":
                        return WareHouseRepository.ListProduct(req.Data);
                    case "WAREHOUSE_ADD_INVENTORY":
                        return WareHouseRepository.AddInventory(req.Data);
                    case "WAREHOUSE_EDIT":
                        return WareHouseRepository.Edit(req.Data);
                    //------------C------------->
                    case "WAREHOUSE_GET_PRODUCTS_BY_ID":
                        return WareHouseRepository.GetProductsByID(req.Data);
                    case "WAREHOUSE_IS_DEFAULT":
                        return WareHouseRepository.IsDefault(req.Data);
                    case "WAREHOUSE_GET_DEFAULT_WAREHOUSE":
                        return WareHouseRepository.GetDefaultWarehouse(req.Data);
                    case "WAREHOUSE_CHANGE_IS_LOCK":
                        return WareHouseRepository.ChangeIsLock(req.Data);
                    case "WAREHOUSE_GET_BY_CODE":
                        return WareHouseRepository.GetByCode(req.Data);
                    #endregion

                    #region WAREHOUSE_PRODUCT
                    case "WAREHOUSE_PRODUCT_UPDATE":
                        return WarehouseProductRepository.Update(req.Data);
                    case "WAREHOUSE_PRODUCT_GET_BY_VARIANT_ID":
                        return WarehouseProductRepository.GetByVariantID(req.Data);
                    #endregion

                    #region WAREHOUSE_IMPORT
                    case "WAREHOUSE_IMPORT_ADD":
                        return WarehouseImportRepositiry.Add(req.Data);
                    case "WAREHOUSE_IMPORT_ADD_DETAIL":
                        return WarehouseImportRepositiry.AddDetail(req.Data);
                    case "WAREHOUSE_IMPORT_GET_DATA_BY_SHOP_ID":
                        return WarehouseImportRepositiry.GetDataByShopID(req.Data);
                    case "WAREHOUSE_IMPORT_GET_DATA_BY_ID":
                        return WarehouseImportRepositiry.GetDataByID(req.Data);
                    case "WAREHOUSE_IMPORT_GET_DETAIL_BY_ID":
                        return WarehouseImportRepositiry.GetDetailByID(req.Data);
                    case "WAREHOUSE_IMPORT_REMOVE":
                        return WarehouseImportRepositiry.Remove(req.Data);
                    case "WAREHOUSE_IMPORT_REMOVE_DETAIL":
                        return WarehouseImportRepositiry.RemoveDetail(req.Data);
                    case "WAREHOUSE_IMPORT_COMPLETE":
                        return WarehouseImportRepositiry.Complete(req.Data);
                    #endregion

                    #region CORPORATION
                    case "CORPORATION_ADDNEW":
                        return CorporationRepositiry.AddNew(req.Data);
                    case "CORPORATION_LIST":
                        return CorporationRepositiry.List(req.Data);
                    case "CORPORATION_GET_BY_ID":
                        return CorporationRepositiry.GetByID(req.Data);
                    case "CORPORATION_EDIT":
                        return CorporationRepositiry.Edit(req.Data);
                    case "CORPORATION_REMOVE":
                        return CorporationRepositiry.RemoveCorporation(req.Data);
                    case "CORPORATION_GET_LIST_EMPLOYEE_BY_CORPORATION_ID":
                        return CorporationRepositiry.GetListEmployeeByCorporationID(req.Data);
                    case "CORPORATION_REMOVE_EMPLOYEE":
                        return CorporationRepositiry.RemoveEmployee(req.Data);
                    #endregion

                    #region SHOP
                    case "SHOP_ADD":
                        return ShopRepository.Add(req.Data);
                    case "SHOP_GET_BY_ID":
                        return ShopRepository.GetByID(req.Data);
                    case "SHOP_GET_HISTORY_BY_ID":
                        return ShopRepository.GetHistoryByID(req.Data);
                    case "SHOP_SEARCH_LIST":
                        return ShopRepository.SearchList(req.Data);
                    case "SHOP_REGISTER":
                        return ShopRepository.Register(req.Data);
                    case "SHOP_LOGIN":
                        return ShopRepository.Login(req.Data);
                    case "SHOP_APPROVED_SHOP":
                        return ShopRepository.ApprovedShop(req.Data);
                    case "SHOP_REJECT_SHOP":
                        return ShopRepository.RejectShop(req.Data);
                    //case "SHOP_UPDATE_CONFIRMATION_EMAIL":
                    //    return ShopRepository.UpdateConfirmationEmail(req.Data);
                    //case "SHOP_ADD_IMAGE":
                    //    return ShopRepository.AddImage(req.Data);
                    //case "SHOP_IMAGES_BY_SHOP_ID":
                    //    return ShopRepository.ImagesByShopID(req.Data);
                    //case "SHOP_IMAGE_BY_ID":
                    //    return ShopRepository.ImageDataByImageID(req.Data);
                    //case "SHOP_LOCK_SHOP":
                    //    return ShopRepository.LockShop(req.Data);
                    //case "SHOP_UNLOCK_SHOP":
                    //    return ShopRepository.UnlockShop(req.Data);
                    case "SHOP_CHANGE_IS_LOCK":
                        return ShopRepository.ChangeIsLock(req.Data);
                    case "SHOP_RESET_PASSWORD_SHOP":
                        return ShopRepository.ResetPasswordShop(req.Data);
                    case "SHOP_EDIT_SHOP":
                        return ShopRepository.EditShop(req.Data);
                    case "SHOP_EDIT":
                        return ShopRepository.Edit(req.Data);
                    case "SHOP_SALES_CHANNEL_LIST":
                        return ShopRepository.SalesChannelList(req.Data);
                    case "SHOP_SALES_CHANNEL_ADD":
                        return ShopRepository.SalesChannelAdd(req.Data);
                    case "SHOP_SALES_CHANNEL_REMOVE":
                        return ShopRepository.SalesChannelRemove(req.Data);
                    case "SHOP_CHANGE_PICK_UP_ADDRESS":
                        return ShopRepository.ChangePickUpAddress(req.Data);
                    case "SHOP_GROUP_PERMISSION_LIST":
                        return ShopRepository.GroupPermissionList(req.Data);
                    case "SHOP_GROUP_PERMISSION_CHANGE_IS_LOCK":
                        return ShopRepository.GroupPermissionChangeIsLock(req.Data);
                    case "SHOP_GROUP_PERMISSION_ADD":
                        return ShopRepository.GroupPermissionAdd(req.Data);
                    case "SHOP_GROUP_PERMISSION_GET_BY_ID":
                        return ShopRepository.GroupPermissionGetByID(req.Data);
                    case "SHOP_GROUP_PERMISSION_EDIT":
                        return ShopRepository.GroupPermissionEdit(req.Data);
                    case "SHOP_LEVEL_GET_BY_PARENT_ID":
                        return ShopRepository.LevelGetByParentID(req.Data);
                    case "SHOP_LEVEL_EDIT":
                        return ShopRepository.LevelEdit(req.Data);
                    case "SHOP_LEVEL_ADD":
                        return ShopRepository.LevelAdd(req.Data);
                    case "SHOP_UPDATE_PAYMENT_INFORMATION":
                        return ShopRepository.UpdatePaymentInformation(req.Data);
                    case "SHOP_UPDATE_AUTO_REQUEST_ORDER":
                        return ShopRepository.UpdateAutoRequestOrder(req.Data);
                    case "SHOP_GET_PICKUP_INFORMATION":
                        return ShopRepository.GetPickupInformation(req.Data);
                    case "SHOP_TRANSPORT_SERVICE_EDIT":
                        return TransportServiceRepository.TransportServiceEditByShop(req.Data);
                    case "SHOP_TRANSPORT_SERVICE_BY_SHOP_ID":
                        return TransportServiceRepository.SearchListByShopID(req.Data);
                    case "SHOP_GET_LIST_CRM_OR_PP_CODE":
                        return ShopRepository.GetListCrmOrPaypostCode(req.Data);
                    case "SHOP_CHANGE_LABEL_PRINT":
                        return ShopRepository.ChangeLabelPrint(req.Data);
                    case "SHOP_REMOVE":
                        return ShopRepository.RemoveShop(req.Data);
                    case "SHOP_LOCK":
                        return ShopRepository.LockShop(req.Data);
                    case "SHOP_GET_BY_CRM_OR_PAYPOST_CODE":
                        return ShopRepository.GetByCrmOrPaypostCode(req.Data);
                    case "SHOP_FORGET_PASSWORD_BY_EMAIL":
                        return ShopRepository.ForgetPasswordByEmail(req.Data);
                    case "SHOP_FORGET_PASSWORD_UPDATE":
                        return ShopRepository.ForgetPasswordUpdate(req.Data);
                    case "SHOP_SEND_EMAIL_REGISTER_SUCCESS":
                        return ShopRepository.SendEmailRegisterSuccess(req.Data);
                    #endregion

                    #region OTP
                    case "OTP_VERIFY_CODE":
                        return OtpRepository.VerifyCode(req.Data);
                    #endregion

                    #region PRODUCT_GROUP
                    case "PRODUCT_GROUP_LIST":
                        return ProductGroupRepository.List(req.Data);
                    case "PRODUCT_GROUP_ADD":
                        return ProductGroupRepository.Add(req.Data);
                    case "PRODUCT_GROUP_GET_BY_ID":
                        return ProductGroupRepository.GetByID(req.Data);
                    case "PRODUCT_GROUP_GET_BY_CODE":
                        return ProductGroupRepository.GetByCode(req.Data);
                    case "PRODUCT_GROUP_EDIT":
                        return ProductGroupRepository.Edit(req.Data);
                    case "PRODUCT_GROUP_SEARCH_LIST_PRODUCT":
                        return ProductGroupRepository.SearchListProduct(req.Data);
                    #endregion

                    #region PRODUCT
                    case "PRODUCT_ADD":
                        return ProductRepository.Add(req.Data);
                    case "PRODUCT_ADD_V2":
                        return ProductRepository.AddV2(req.Data);
                    case "PRODUCT_GET_BY_ID":
                        return ProductRepository.GetByID(req.Data);
                    case "PRODUCT_EDIT":
                        return ProductRepository.Edit(req.Data);
                    case "PRODUCT_LIST":
                        return ProductRepository.List(req.Data);
                    case "PRODUCT_REMOVE":
                        return ProductRepository.Remove(req.Data);
                    case "PRODUCT_DATA_IMAGES_BY_ID":
                        return ProductRepository.DataImagesByID(req.Data);
                    case "PRODUCT_EDIT_IMAGES":
                        return ProductRepository.EditImages(req.Data);
                    //case "PRODUCT_ADD_IMAGES":
                    //    return ProductRepository.AddImages(req.Data);
                    case "PRODUCT_UPDATE_AVATAR":
                        return ProductRepository.UpdateAvatar(req.Data);
                    case "PRODUCT_GET_BY_VARIANT_ID":
                        return ProductRepository.GetByVariantID(req.Data);
                    case "PRODUCT_UPDATE_PRODUCT_GROUP":
                        return ProductRepository.UpdateProductGroup(req.Data);
                    case "PRODUCT_IMPORT_EXCEL":
                        return ProductRepository.ImportExcel(req.Data);
                    #endregion

                    #region PRODUCT_VARIANT
                    case "PRODUCT_VARIANT_SEARCH_LIST":
                        return ProductVariantRepository.SearchList(req.Data);
                    case "PRODUCT_VARIANT_GET_BY_ID":
                        return ProductVariantRepository.GetByID(req.Data);
                    case "PRODUCT_VARIANT_GET_BY_PRODUCT_ID":
                        return ProductVariantRepository.GetByProductID(req.Data);
                    case "PRODUCT_VARIANT_GET_BY_IDS":
                        return ProductVariantRepository.GetByIDs(req.Data);
                    case "PRODUCT_VARIANT_GET_BY_SKU":
                        return ProductVariantRepository.GetBySKU(req.Data);
                    case "PRODUCT_VARIANT_GET_DETAIL_BY_ID":
                        return ProductVariantRepository.GetDetailByID(req.Data);
                    case "PRODUCT_VARIANT_DATA_BY_ID":
                        return ProductVariantRepository.DataByID(req.Data);
                    case "PRODUCT_VARIANT_GET_BY_SHOP_ID":
                        return ProductVariantRepository.GetByShopID(req.Data);
                    case "PRODUCT_VARIANT_UPDATE_IMAGE":
                        return ProductVariantRepository.UpdateImage(req.Data);
                    case "PRODUCT_VARIANT_ADD":
                        return ProductVariantRepository.Add(req.Data);
                    case "PRODUCT_VARIANT_EDIT":
                        return ProductVariantRepository.Edit(req.Data);
                    case "PRODUCT_VARIANT_REMOVE":
                        return ProductVariantRepository.Remove(req.Data);
                    case "PRODUCT_VARIANT_GET_BY_WAREHOUSE_ID":
                        return ProductVariantRepository.GetByWareHouseID(req.Data);
                    case "PRODUCT_VARIANT_CHECK_SKU":
                        return ProductVariantRepository.CheckSKU(req.Data);
                    #endregion

                    #region PRODUCT_OPTION
                    case "PRODUCT_OPTION_GET_NAME_BY_PRODUCT_ID":
                        return ProductOptionRepositiry.GetNameByProductID(req.Data);
                    case "PRODUCT_OPTION_GET_BY_PRODUCT_ID":
                        return ProductOptionRepositiry.GetByProductID(req.Data);
                    case "PRODUCT_OPTION_ADD":
                        return ProductOptionRepositiry.Add(req.Data);
                    case "PRODUCT_OPTION_REMOVE_BY_NAME":
                        return ProductOptionRepositiry.RemoveByName(req.Data);
                    #endregion

                    #region PRODUCT_UNIT
                    case "PRODUCT_UNIT_SEARCH_LIST":
                        return ProductUnitRepository.SearchList(req.Data);
                    case "PRODUCT_UNIT_ADD":
                        return ProductUnitRepository.AddUnit(req.Data);
                    #endregion

                    #region EMPLOYEE_LEVEL
                    case "EMPLOYEE_LEVEL_GETS":
                        return EmployeeLevelRepository.Gets();
                    #endregion

                    #region EMPLOYEE_FUNCTIONAL
                    case "EMPLOYEE_FUNCTIONAL_LIST_FUNCTIONAL":
                        return EmployeeFunctionalRepository.ListFunctional(req.Data);
                    case "EMPLOYEE_FUNCTIONAL_CHANGE":
                        return EmployeeFunctionalRepository.ChangeFunctional(req.Data);
                    case "EMPLOYEE_FUNCTIONAL_CHECK":
                        return EmployeeFunctionalRepository.CheckFunctional(req.Data);
                    case "EMPLOYEE_FUNCTIONAL_LIST_MENU":
                        return EmployeeFunctionalRepository.ListMenu(req.Data);
                    #endregion

                    #region PROVIDER
                    case "PROVIDER_LIST":
                        return ProviderRepository.List(req.Data);
                    case "PROVIDER_ADD":
                        return ProviderRepository.Add(req.Data);
                    case "PROVIDER_GET_BY_ID":
                        return ProviderRepository.GetByID(req.Data);
                    case "PROVIDER_EDIT":
                        return ProviderRepository.Edit(req.Data);
                    case "PROVIDER_CHANGE_IS_LOCK":
                        return ProviderRepository.ChangeIsLock(req.Data);
                    #endregion

                    #region PROVIDER_TEMP
                    case "PROVIDER_TEMP_ADD_TEMP":
                        return ProviderTempRepository.AddTemp(req.Data);
                    case "PROVIDER_TEMP_ADD_COMMIT":
                        return ProviderTempRepository.AddCommit(req.Data);
                    #endregion

                    #region PROMOTION
                    case "PROMOTION_ADD_NEW_CODE":
                        return PromotionRepository.AddNewPromotionCode(req.Data);
                    case "PROMOTION_ADD_NEW":
                        return PromotionRepository.AddNewPromotion(req.Data);
                    case "PROMOTION_GET_BY_CODE":
                        return PromotionRepository.GetByCode(req.Data);
                    case "PROMOTION_LIST":
                        return PromotionRepository.List(req.Data);
                    case "PROMOTION_IS_LOCK":
                        return PromotionRepository.IsLock(req.Data);
                    case "PROMOTION_REMOVE":
                        return PromotionRepository.PromotionRemove(req.Data);
                    #endregion

                    #region PROVINCES
                    case "PROVINCE_LIST":
                        return ProvincesRepository.List(req.Data);
                    case "PROVINCE_SEARCH":
                        return ProvincesRepository.SearchProvince(req.Data);
                    case "PROVINCE_GET_PRO_ID_BY_PRO_NAME":
                        return ProvincesRepository.GetProvinceIDByProvinceName(req.Data);
                    case "PROVINCE_IMPORT_EXCEL":
                        return ProvincesRepository.ProvinceImportExcel(req.Data);
                    #endregion

                    #region DISTRICT
                    case "DISTRICT_SEARCH":
                        return DistrictsRepository.SearchDistrict(req.Data);
                    case "DISTRICT_GET_BY_PROVINCE_ID":
                        return DistrictsRepository.GetByProvinceID(req.Data);
                    case "DISTRICT_GET_DIS_ID_BY_DIS_NAME":
                        return DistrictsRepository.GetDistrictIDByDistrictName(req.Data);
                    case "DISTRICT_GET_BY_V_MAP_CODE":
                        return DistrictsRepository.GetByVMapCode(req.Data);
                    case "DISTRICT_GET_BY_PROVINCE_NAME":
                        return DistrictsRepository.GetByProvinceName(req.Data);
                    #endregion

                    #region WARD
                    case "WARD_GETS":
                        return WardsRepository.Gets();
                    case "WARD_SEARCH":
                        return WardsRepository.SearchWard(req.Data);
                    case "WARD_GET_BY_DISTRICT_ID":
                        return WardsRepository.GetByDistrictID(req.Data);
                    case "WARD_GET_WARD_ID_BY_WARD_NAME":
                        return WardsRepository.GetWardIDByWardName(req.Data);
                    case "WARD_GET_BY_V_MAP_CODE":
                        return WardsRepository.GetByVMapCode(req.Data);
                    case "WARD_GET_BY_PARENT_NAME":
                        return WardsRepository.GetByParentName(req.Data);
                    #endregion

                    #region FUNCTIONAL
                    case "FUNCTIONAL_LIST":
                        return FunctionalRepository.List(req.Data);
                    #endregion

                    #region EXCEL
                    case "IMPORT_EXCEL_ADD":
                        return ExcelRepository.AddImportExcel(req.Data);
                    #endregion

                    #region Option
                    case "OPTION_ADD":
                        return OptionRepositiry.Add(req.Data);
                    case "OPTION_REMOVE_BY_ID":
                        return OptionRepositiry.RemoveByID(req.Data);
                    case "OPTION_GET_DATA_BY_SHOP_ID":
                        return OptionRepositiry.GetOptionByShopID(req.Data);
                    case "OPTION_GET_DATA_BY_PARENT_ID":
                        return OptionRepositiry.GetDataByParentID(req.Data);
                    case "OPTION_DATA_BY_VARIANT_ID":
                        return OptionRepositiry.DataByVariantID(req.Data);
                    #endregion

                    #region ORDER
                    case "ORDER_SEARCH_PRODUCT":
                        return OrderRepository.SearchProduct(req.Data);
                    //case "ORDER_ADD":
                    //    return OrderRepository.Add(req.Data);
                    //case "ORDER_ADD_AND_SEND_TRANSPORT":
                    //    return OrderRepository.AddOrderAndSendTransport(req.Data);
                    case "ORDER_ADD":
                        return OrderRepository.AddV2(req.Data);
                    case "ORDER_ADD_AND_SEND_TRANSPORT":
                        return OrderRepository.AddOrderAndSendTransportV2(req.Data);
                    case "ORDER_EDIT":
                        return OrderRepository.OrderEdit(req.Data);
                    case "ORDER_SEND_TRANSPORT":
                        return OrderRepository.OrderSendTransport(req.Data);
                    //case "ORDER_SEND_TRANSPORT_WEB":
                    //    return OrderRepository.OrderSendTransportWEB(req.Data);
                    case "ORDER_CANCEL_TRANSPORT":
                        return OrderRepository.OrderCancelTransport(req.Data);
                    case "RETURN_ORDER_ADD":
                        return OrderRepository.ReturnOrderAdd(req.Data);
                    case "ORDER_SEARCH":
                        return OrderRepository.FESearchList(req.Data);
                    case "ORDER_PAGINATION_SEARCH":
                        return OrderRepository.FEPaginationSearchList(req.Data);
                    case "ORDER_PAGINATION_EXPAND_SEARCH":
                        return OrderRepository.FEPaginationExpandSearchList(req.Data);
                    case "ORDER_BE_SEARCH_LIST":
                        return OrderRepository.BESearchList(req.Data);
                    case "ORDER_IMPORT_SEARCH":
                        return OrderRepository.OrderImportSearchList(req.Data);
                    //case "ORDER_CHANGE_STATUS":
                    //    return OrderRepository.ChangeStatus(req.Data);
                    case "ORDER_CANCEL_STATUS":
                        return OrderRepository.CancelStatus(req.Data);
                    case "ORDER_NEW_STATUS":
                        return OrderRepository.NewStatus(req.Data);
                    case "ORDER_GET_BY_ID":
                        return OrderRepository.GetByID(req.Data);
                    case "ORDER_GET_BY_SHIPPING_CODE":
                        return OrderRepository.GetByShippingCode(req.Data);
                    case "ORDER_DETAIL_BY_ORDER_ID":
                        return OrderRepository.DetailByOrderID(req.Data);
                    case "ORDER_DETAIL_BY_SHIPPING_CODE":
                        return OrderRepository.DetailByShippingCode(req.Data);
                    case "ORDER_GET_BY_ORDER_CODE_HM":
                        return OrderRepository.DetailByOrderCodeHM(req.Data);

                    case "ORDER_COPPY":
                        return OrderRepository.CoppyOrder(req.Data);
                    //case "ORDER_SEARCH_INFO_SEND_REQUEST_TRANSPORT":
                    //    return OrderRepository.SearchInfoSendRequestTransport(req.Data);
                    //case "ORDER_CANCEL_REQUEST_TRANSPORT":
                    //    return OrderRepository.CancelRequestTransport(req.Data);
                    //case "CHECK_ORDER_TRANSPORT":
                    //    return OrderRepository.CheckOrderTransport(req.Data);
                    case "ORDER_SUCCESS_HISTORY":
                        return OrderRepository.SuccessHistory(req.Data);
                    case "ORDER_PRINT":
                        return OrderRepository.Print(req.Data);
                    case "ORDER_PRINT_SINGLE":
                        return OrderRepository.PrintSingle(req.Data);
                    case "ORDER_SEARCH_HISTORY":
                        return OrderRepository.SearchHistory(req.Data);
                    //case "ORDER_CHECK_ORDER_CODE":
                    //    return OrderRepository.CheckOrderCode(req.Data);
                    case "ORDER_UPDATE_PRINT_STATUS":
                        return OrderRepository.UpdatePrintStatus(req.Data);
                    case "ORDER_EDIT_RECEIVER_ADDRESS":
                        return OrderRepository.EditReceiverAddress(req.Data);
                    case "ORDER_GET_EXCEL_LIST_BY_SHOP_ID":
                        return OrderRepository.SearchListExcelImport(req.Data);
                    #endregion

                    #region ORDER_TEMP
                    case "ORDER_TEMP_ADD_TEMP":
                        return OrderTempRepositiry.AddTemp(req.Data);
                    case "ORDER_TEMP_ADD_COMMIT":
                        return OrderTempRepositiry.AddCommit(req.Data);
                    #endregion

                    #region ORDER_REPORT
                    case "ORDER_REPORT_LIST":
                        return OrderReportRepository.ListOrderReport(req.Data);
                    case "ORDER_REPORT_DETAIL_LIST":
                        return OrderReportRepository.ListOrderDetailReport(req.Data);
                    #endregion

                    #region REQUEST_ORDER
                    case "REQUEST_ORDER_LIST":
                        return RequestOrderRepository.ListRequestOrder(req.Data);
                    case "REQUEST_ORDER_GET_BY_ID":
                        return RequestOrderRepository.GetByID(req.Data);
                    case "REQUEST_ORDER_ADD":
                        return RequestOrderRepository.AddNew(req.Data);
                    case "REQUEST_ORDER_ADD_V2":
                        return RequestOrderRepository.AddNewV2(req.Data);
                    //case "ADDNEW_REQUEST_ACCEPT":
                    //    return RequestOrderRepository.Accept(req.Data);
                    case "REQUEST_ORDER_LIST_DIVISION":
                        return RequestOrderRepository.ListDivision(req.Data);
                    case "REQUEST_ORDER_EXCEL_DIVISION":
                        return RequestOrderRepository.ExcelDivision(req.Data);
                    case "REQUEST_ORDER_CHANGE_STATUS":
                        return RequestOrderRepository.ChangeStatus(req.Data);
                    case "REQUEST_ORDER_CHANGE_REASON":
                        return RequestOrderRepository.ChangeReason(req.Data);
                    case "REQUEST_ORDER_LIST_DIVISION_AGAIN":
                        return RequestOrderRepository.ListDivisionAgain(req.Data);
                    case "REQUEST_ORDER_UPDATE":
                        return RequestOrderRepository.Update(req.Data);
                    #endregion

                    #region REQUEST_ORDER_TEMP
                    case "REQUEST_ORDER_TEMP_ADD_TEMP":
                        return RequestOrderTempRepository.AddTemp(req.Data);
                    case "REQUEST_ORDER_TEMP_ADD_COMMIT":
                        return RequestOrderTempRepository.AddCommit(req.Data);
                    #endregion

                    #region FACEBOOK
                    case "FACEBOOK_LOGIN":
                        return FacebookRepository.FacebookLogin(req.Data);
                    case "FACEBOOK_PAGE_CONNECT_GET_BY_ID":
                        return FacebookRepository.GetPageConnectByShopID(req.Data);
                    case "FACEBOOK_PAGE_CONNECT_ADD":
                        return FacebookRepository.AddConnectPage(req.Data);
                    case "FACEBOOK_PAGE_CONNECT_ADD_V2":
                        return FacebookRepository.AddConnectPage_V2(req.Data);
                    case "FACEBOOK_CONVERSATION_ASSIGN_LIST":
                        return FacebookRepository.ListConversationAssign(req.Data);
                    case "FACEBOOK_CONVERSATION_ASSIGN_ADD":
                        return FacebookRepository.AddConversationAssign(req.Data);
                    case "FACEBOOK_CONVERSATION_ASSIGN_EDIT":
                        return FacebookRepository.EditConversationAssign(req.Data);
                    case "FACEBOOK_CONVERSATION_ASSIGN_DELETE":
                        return FacebookRepository.DeleteConversationAssign(req.Data);
                    case "FACEBOOK_FEED_LIST":
                        return FacebookRepository.ListFeed(req.Data);
                    case "FACEBOOK_COMMENT_LIST":
                        return FacebookRepository.ListComment(req.Data);
                    case "FACEBOOK_ADDCONVER_TO_CONVERSATION_ASSIGN":
                        return FacebookRepository.AddAssignToConversation(req.Data);
                    case "FACEBOOK_DELETE_ASSIGN":
                        return FacebookRepository.DeleteAssign(req.Data);
                    case "FACEBOOK_MASK_AS_VIEWED":
                        return FacebookRepository.MaskAsViewed(req.Data);
                    case "FACEBOOK_MASK_AS_READ":
                        return FacebookRepository.MaskAsRead(req.Data);
                    case "FACEBOOK_MASK_AS_VIEWED_DETAIL":
                        return FacebookRepository.MaskAsViewedDetail(req.Data);
                    case "FACEBOOK_MASK_AS_READ_DETAIL":
                        return FacebookRepository.MaskAsReadDetail(req.Data);
                    case "FACEBOOK_CUSTOMER_CHECK_INFO":
                        return FacebookRepository.CheckCustomerInfo(req.Data);
                    case "FACEBOOK_CONVERSATION_SPLIT":
                        return FacebookRepository.SplitConversation(req.Data);
                    case "FACEBOOK_CONVERSATION_SPLIT_COUNT":
                        return FacebookRepository.CountConversationSplit(req.Data);
                    case "FACEBOOK_CONVERSATION_SPLIT_MULTI":
                        return FacebookRepository.SplitConversationMulti(req.Data);
                    case "FACEBOOK_PAGE_AUTOLIKE":
                        return FacebookRepository.PageAutoLike(req.Data);
                    case "FACEBOOK_PAGE_AUTOLIKE_CHANGE_STATUS":
                        return FacebookRepository.ChangeStatusPageAutoLike(req.Data);
                    case "FACEBOOK_PAGE_AUTO_HIDDEN":
                        return FacebookRepository.PageAutoHidden(req.Data);
                    case "FACEBOOK_PAGE_AUTO_HIDDEN_EDIT":
                        return FacebookRepository.EditPageHidden(req.Data);
                    case "FACEBOOK_PAGE_AUTO_HIDDEN_CHANGE":
                        return FacebookRepository.ChangeStatusPageAutoHidden(req.Data);
                    case "FACEBOOK_PAGE_AUTO_HIDDEN_GET_BYID":
                        return FacebookRepository.GetPageAutoHiddenByID(req.Data);
                    case "FACEBOOK_WORKING_TIME_EDIT":
                        return FacebookRepository.EditWorkingTime(req.Data);
                    case "FACEBOOK_WORKING_TIME_DELETE":
                        return FacebookRepository.DeleteWorkingTime(req.Data);
                    case "FACEBOOK_WORKING_TIME_LIST":
                        return FacebookRepository.ListWorkingTime(req.Data);
                    case "FACEBOOK_WORKING_TIME_GET_BY_ID":
                        return FacebookRepository.GetWorkingTimeByID(req.Data);
                    case "FACEBOOK_WORKING_TIME_DETAIL_ADD":
                        return FacebookRepository.AddWorkingTimeDetail(req.Data);
                    case "FACEBOOK_WORKING_TIME_DETAIL_EDIT":
                        return FacebookRepository.EditWorkingTimeDetail(req.Data);
                    case "FACEBOOK_WORKING_TIME_DETAIL_DELETE":
                        return FacebookRepository.DeleteWorkingTimeDetail(req.Data);
                    case "FACEBOOK_WORKING_TIME_DETAIL":
                        return FacebookRepository.WorkingTimeDetail(req.Data);
                    case "FACEBOOK_MESSAGES_CAMPAIGN_ADD":
                        return FacebookRepository.AddMessagesCampaign(req.Data);
                    case "FACEBOOK_MESSAGES_CAMPAIGN_LIST":
                        return FacebookRepository.ListMessagesCampaign(req.Data);
                    case "FACEBOOK_MASS_MESSAGES_ADD":
                        return FacebookRepository.AddSendMassMessages(req.Data);
                    case "FACEBOOK_MASS_MESSAGES_LIST":
                        return FacebookRepository.ListSendMassMessages(req.Data);
                    case "FACEBOOK_PAGE_FEEDBACK_AUTO_ADD":
                        return FacebookRepository.AddPageFeedbackAuto(req.Data);
                    case "FACEBOOK_FEEDBACK_AUTO_LIST":
                        return FacebookRepository.ListFeedbackAuto(req.Data);
                    case "FACEBOOK_FEEDBACK_AUTO_CHANGE":
                        return FacebookRepository.ChangeFeedbackAuto(req.Data);
                    case "FACEBOOK_FEEDBACK_AUTO_DELETE":
                        return FacebookRepository.DeleteFeedbackAuto(req.Data);
                    case "FACEBOOK_PAGE_CONNECT_RESET":
                        return FacebookRepository.ResetConnectPage(req.Data);
                    #endregion

                    #region FACEBOOK SYNC DATA
                    case "FACEBOOK_FEED_SYNC_ADD":
                        return FeedRepository.AddSyncFeed(req.Data);
                    case "FACEBOOK_COMMENT_SYNC_ADD":
                        return FeedRepository.AddSyncComment(req.Data);
                    case "FACEBOOK_CONVERSATION_SYNC_ADD":
                        return FacebookRepository.AddXMLConversation(req.Data);
                    case "FACEBOOK_CONVERSATION_DETAIL_SYNC_ADD":
                        return FacebookRepository.AddXMLConversationDetail(req.Data);
                    #endregion

                    #region DELIVERY_NOTE
                    case "DELIVERY_NOTE_LIST":
                        return DeliveryNoteRepository.List(req.Data);
                    case "DELIVERY_NOTE_GET_BY_ID":
                        return DeliveryNoteRepository.GetByID(req.Data);
                    case "DELIVERY_NOTE_GET_BY_ORDER_ID":
                        return DeliveryNoteRepository.GetDeliveryNoteByOrderID(req.Data);
                    case "DELIVERY_NOTE_ADD":
                        return DeliveryNoteRepository.Add(req.Data);
                    case "DELIVERY_NOTE_CHANGE_STATUS":
                        return DeliveryNoteRepository.ChangeStatus(req.Data);
                    case "DELIVERY_NOTE_COMPLETED_STATUS":
                        return DeliveryNoteRepository.CompletedStatus(req.Data);

                    #endregion

                    #region STATUS_PROCESS
                    case "STATUS_PROCESS_LIST":
                        return StatusProcessRepository.ListStatusProcess(req.Data);
                    case "STATUS_PROCESS_ADD":
                        return StatusProcessRepository.AddStatusProcess(req.Data);
                    case "STATUS_PROCESS_CHANGE_IS_LOCK":
                        return StatusProcessRepository.ChangeIsLockStatusProcess(req.Data);
                    case "STATUS_PROCESS_REMOVE":
                        return StatusProcessRepository.RemoveStatusProcess(req.Data);
                    #endregion

                    #region TRANSPORT_REGION
                    case "TRANSPORT_REGION_SEARCH_LIST":
                        return TransportRegionRepository.SearchList(req.Data);
                    case "TRANSPORT_REGION_ADD":
                        return TransportRegionRepository.Add(req.Data);
                    case "TRANSPORT_REGION_GET_BY_PARENT_ID":
                        return TransportRegionRepository.GetByParentID(req.Data);
                    #endregion

                    #region TRANSPORT_REGION_GROUP
                    case "TRANSPORT_REGION_GROUP_SEARCH_LIST":
                        return TransportRegionGroupRepository.SearchList(req.Data);
                    case "TRANSPORT_REGION_GROUP_ADD":
                        return TransportRegionGroupRepository.Add(req.Data);
                    #endregion

                    #region TRANSPORT_POSTAGE_V2
                    case "TRANSPORT_POSTAGE_SEARCH_LIST":
                        return TransportPostageRepository.SearchList(req.Data);
                    case "TRANSPORT_POSTAGE_MAIN_SERVICE_BASIC_ADD":
                        return TransportPostageRepository.MainServiceBasicAdd(req.Data);
                    case "TRANSPORT_POSTAGE_MAIN_SERVICE_BASIC_EDIT":
                        return TransportPostageRepository.MainServiceBasicEdit(req.Data);
                    case "TRANSPORT_POSTAGE_MAIN_SERVICE_FEE_EDIT":
                        return TransportPostageRepository.MainServiceFeeEdit(req.Data);
                    case "TRANSPORT_POSTAGE_MAIN_SERVICE_SURCHARGE_EDIT":
                        return TransportPostageRepository.MainServiceSurchargeEdit(req.Data);
                    case "TRANSPORT_POSTAGE_EXTRA_SERVICE_BASIC_ADD":
                        return TransportPostageRepository.ExtraServiceBasicAdd(req.Data);
                    case "TRANSPORT_POSTAGE_EXTRA_SERVICE_BASIC_EDIT":
                        return TransportPostageRepository.ExtraServiceBasicEdit(req.Data);


                    case "TRANSPORT_POSTAGE_GET_BY_TRANSPORT_POSTAGE_ID":
                        return TransportPostageRepository.GetByTransportPostageID(req.Data);




                    case "TRANSPORT_POSTAGE_CHANGE_IS_LOCK":
                        return TransportPostageRepository.ChangeIsLock(req.Data);
                    case "TRANSPORT_POSTAGE_CONFIRM":
                        return TransportPostageRepository.Confirm(req.Data);
                    case "TRANSPORT_POSTAGE_UNCONFIRM":
                        return TransportPostageRepository.UnConfirm(req.Data);
                    case "TRANSPORT_POSTAGE_SEARCH_HISTORY":
                        return TransportPostageRepository.SearchHistory(req.Data);
                    #endregion

                    #region TRANSPORT_POSTAGE



                    //case "TRANSPORT_POSTAGE_SURCHARGE_BY_ID":
                    //    return TransportRepository.PostageSurchargeByID(req.Data);

                    //case "TRANSPORT_POSTAGE_MAIN_FEE_BY_ID":
                    //    return TransportRepository.PostageMainFeeByID(req.Data);
                    case "TRANSPORT_POSTAGE_RANGE_ADD":
                        return TransportRepository.PostageRangeAdd(req.Data);
                    case "TRANSPORT_POSTAGE_RANGE_SEARCH":
                        return TransportRepository.PostageRangeSearch(req.Data);
                    case "TRANSPORT_POSTAGE_WEIGHT_ADD":
                        return TransportRepository.PostageWeightAdd(req.Data);
                    case "TRANSPORT_POSTAGE_WEIGHT_SEARCH":
                        return TransportRepository.PostageWeightSearch(req.Data);
                   
                    
                    
                    //-----------
                    case "TRANSPORT_POSTAGE_EXTRA_SERVICE_BY_ID":
                        return TransportRepository.PostageExtraServiceByID(req.Data);
                    case "TRANSPORT_POSTAGE_CHARGED":
                        return TransportRepository.Charged(req.Data);
                    //----------
                    
                    #endregion

                    #region TRANSPORT_POSTAGE_DETAIL
                    case "TRANSPORT_POSTAGE_DETAIL_ADD":
                        return TransportPostageDetailRepository.Add(req.Data);
                    case "TRANSPORT_POSTAGE_DETAIL_EDIT":
                        return TransportPostageDetailRepository.Edit(req.Data);
                    case "TRANSPORT_POSTAGE_DETAIL_REMOVE":
                        return TransportPostageDetailRepository.Remove(req.Data);
                    case "TRANSPORT_POSTAGE_DETAIL_GET_BY_TRANSPORT_POSTAGE_ID":
                        return TransportPostageDetailRepository.GetByTransportPostageID(req.Data);
                    #endregion

                    #region TRANSPORT_SERVICE
                    case CommandCodeConst.TRANSPORT_SERVICE_LIST:
                        return TransportServiceRepository.SearchList(req.Data);
                    case CommandCodeConst.TRANSPORT_SERVICE_ADD:
                        return TransportServiceRepository.Add(req.Data);
                    //case "TRANSPORT_SERVICE_CHECK":
                    //    return TransportRepository.ServiceCheck(req.Data);
                    case "TRANSPORT_SERVICE_EDIT":
                        return TransportServiceRepository.TransportServiceEdit(req.Data);
                    case "TRANSPORT_SERVICE_GET_BY_ID":
                        return TransportServiceRepository.GetByID(req.Data);
                    case "TRANSPORT_SERVICE_LOCK":
                        return TransportServiceRepository.TransportServiceLock(req.Data);
                    case "TRANSPORT_SERVICE_UNLOCK":
                        return TransportServiceRepository.TransportServiceUnlock(req.Data);
                    case "TRANSPORT_SERVICE_CHECK_BY_CODE":
                        return TransportServiceRepository.TransportServiceCheckByCode(req.Data);
                    #endregion

                    #region PERMISSION
                    case "PERMISSION_LIST":
                        return PermissionRepository.ListPermission(req.Data);
                    //case "PERMISSION_LIST_FUNCTIONAL_GROUP":
                    //    return PermissionRepository.ListFunctionalGroup(req.Data);
                    case "PERMISSION_CHANGE":
                        return PermissionRepository.ChangePermission(req.Data);
                    case "PERMISSION_LIST_FUNCTIONAL":
                        return PermissionRepository.ListFunctional(req.Data);
                    //case "PERMISSION_MENU_LEFT":
                    //    return PermissionRepository.MenuLeft(req.Data);
                    case "PERMISSION_LIST_MENU":
                        return PermissionRepository.ListMenu(req.Data);
                    case "PERMISSION_LIST_ALLOW":
                        return PermissionRepository.ListAllow(req.Data);
                    #endregion

                    #region TRANSPORT
                    //case "TRANSPORT_LIST_ORDER":
                    //    return TransportRepository.List(req.Data);
                    //case "TRANSPORT_INFO_SHOP_GET":
                    //    return TransportRepository.GetInfoShop(req.Data);
                    //case "TRANSPORT_INFO_SHOP_ORDER_DETAIL_GET":
                    //    return TransportRepository.GetInfoShopOrderDetail(req.Data);
                    //case "TRANSPORT_ORDER_UPDATE_STATUS":
                    //    return TransportRepository.UpdateStatusTransportOrder(req.Data);
                    //case "TRANSPORT_EMS_BARCODE_GET":
                    //    return TransportRepository.GetEmsBarCode(req.Data);
                    //case "TRANSPORT_EMS_BARCODE_CANCEL_GET":
                    //    return TransportRepository.GetEmsBarCodeCancel(req.Data);
                    #endregion

                    #region STATISTICAL
                    case "STATISTICAL_DASHBOARD":
                        return StatisticaltRepository.StatisticalDashboard(req.Data);
                    //case "STATISTICAL_TRANSPORT":
                    //    return StatisticaltRepository.StatisticalTransport(req.Data);
                    //case "STATISTICAL_ORDER":
                    //    return StatisticaltRepository.StatisticalOrder(req.Data);
                    //case "STATISTICAL_CUSTOMER":
                    //    return StatisticaltRepository.StatisticalCustomer(req.Data);
                    //case "STATISTICAL_REQUEST_ORDER":
                    //    return StatisticaltRepository.StatisticalRequestOrder(req.Data);
                    //case "STATISTICAL_CASHFLOW":
                    //    return StatisticaltRepository.StatisticalCashflow(req.Data);
                    case "STATISTICAL":
                        return StatisticaltRepository.StatisticalDashboardV2(req.Data);
                    #endregion

                    #region RECONCILIATION
                    case "RECONCILIATION_FEE_LIST":
                        return ReconciliationRepository.ListTransportFee(req.Data);
                    case "RECONCILIATION_PERIOD_FEE_LIST":
                        return ReconciliationRepository.ListTransportFeePeriod(req.Data);
                    case "RECONCILIATION_TRADE_DISCOUNTS_FEE_LIST":
                        return ReconciliationRepository.ListTransportFeeTradeDiscounts(req.Data);
                    case "RECONCILIATION_COD_LIST":
                        return ReconciliationRepository.ListCOD(req.Data);
                    case "RECONCILIATION_PERIOD_COD_LIST":
                        return ReconciliationRepository.ListCODPeriod(req.Data);
                    case "RECONCILIATION_TRANSPORT_FEE_AGREE_UPDATE":
                        return ReconciliationRepository.UpdateAgreeTransportFee(req.Data);
                    case "RECONCILIATION_COD_AGREE_UPDATE":
                        return ReconciliationRepository.UpdateAgreeCOD(req.Data);
                    case "CONTROL_PERIOD_LIST":
                        return ReconciliationRepository.ListControlPeriod(req.Data);
                    case "CONTROL_PERIOD_DATE_ID":
                        return ReconciliationRepository.ControlPeriodDateID(req.Data);
                    case "RECONCILIATION_COD_FOLLOW_LIST":
                        return ReconciliationRepository.ListCODFollow(req.Data);
                    case "RECONCILIATION_FEE_FOLLOW_LIST":
                        return ReconciliationRepository.ListFeeFollow(req.Data);
                    case "RECONCILIATION_FEE_REPORT_LIST":
                        return ReconciliationRepository.ListFeeReport(req.Data);
                    case "RECONCILIATION_FEE_POSTOFFICE_REPORT_LIST":
                        return ReconciliationRepository.ListFeePostOfficeReport(req.Data);
                    case "RECONCILIATION_ADJUSTING_JOURNAL_ENTRIES_LIST":
                        return ReconciliationRepository.ListFeeAdjustingJournalEntries(req.Data);
                    case "RECONCILIATION_DETAIL_EDITED_ITEMCODE_LIST":
                        return ReconciliationRepository.ListDetailEditedItemCode(req.Data);
                    case "RECONCILIATION_POSTAGE_REFUND":
                        return ReconciliationRepository.ListPostageRefund(req.Data);
                    case "RECONCILIATION_DEBT_NOTICES":
                        return ReconciliationRepository.ListDebtNotices(req.Data);
                    case "RECONCILIATION_SUMMARY_DEBT_NOTICES":
                        return ReconciliationRepository.ListSummaryOfDebtNotices(req.Data);
                    #endregion

                    #region RECONCILIATION NOTIFY
                    case "RECONCILIATION_NOTIFY_LIST":
                        return ReconciliationNotifyRepository.ListNotify(req.Data);
                    case "RECONCILIATION_NOTIFY_UPDATE":
                        return ReconciliationNotifyRepository.UpdateNotify(req.Data);
                    #endregion

                    #region PAYMENT COD
                    case "PAYMENT_COD_LIST":
                        return PaymentRepository.ListPaymentCOD(req.Data);
                    case "PAYMENT_COD_UPDATE":
                        return PaymentRepository.UpdatePaymentCOD(req.Data);
                    #endregion

                    #region PREPAID_FREIGHT_PAYMENT
                    case "PREPAID_FREIGHT_PAYMENT_LIST":
                        return PrepaidFreightPaymentRepository.ListPrepaidFreightPayment(req.Data);
                    #endregion

                    #region REQUEST_SUPPORT
                    case "REQUEST_SUPPORT_LIST":
                        return RequestSupportRepository.FESearchList(req.Data);
                    case "REQUEST_SUPPORT_BE_SEARCH_LIST":
                        return RequestSupportRepository.BESearchList(req.Data);
                    case "REQUEST_SUPPORT_ADD":
                        return RequestSupportRepository.Add(req.Data);
                    //case "REQUEST_SUPPORT_SEARCH_INFO_SEND_REQUEST_SUPPORT":
                    //    return RequestSupportRepository.SearchInfoSendRequestSupport(req.Data);
                    case "REQUEST_SUPPORT_UPDATE_STATUS":
                        return RequestSupportRepository.UpdateStatus(req.Data);
                    case "REQUEST_SUPPORT_GET_BY_ID":
                        return RequestSupportRepository.GetByID(req.Data);
                    case "REQUEST_SUPPORT_DETAIL":
                        return RequestSupportRepository.Detail(req.Data);
                    //case "REQUEST_SUPPORT_SEARCH_HISTORY":
                    //    return RequestSupportRepository.SearchHistory(req.Data);
                    case "REQUEST_SUPPORT_ACCEPTED_STATUS":
                        return RequestSupportRepository.AcceptedStatus(req.Data);
                    case "REQUEST_SUPPORT_UNACCEPTED_STATUS":
                        return RequestSupportRepository.UnAcceptedStatus(req.Data);
                    case "REQUEST_SUPPORT_PROCESS_STATUS":
                        return RequestSupportRepository.ProcessStatus(req.Data);
                    case "REQUEST_SUPPORT_CLOSE_STATUS":
                        return RequestSupportRepository.CloseStatus(req.Data);
                    case "REQUEST_SUPPORT_RE_OPEN_STATUS":
                        return RequestSupportRepository.ReOpenStatus(req.Data);
                    case "REQUEST_SUPPORT_EXIST_STATUS":
                        return RequestSupportRepository.ExistStatus(req.Data);
                    case "REQUEST_SUPPORT_NOTIFY_LIST":
                        return RequestSupportRepository.NotifyList(req.Data);
                    case "REQUEST_SUPPORT_UPDATE_CLICK_NOTIFY":
                        return RequestSupportRepository.UpdateClickNotify(req.Data);
                    //case "REQUEST_SUPPORT_LOCK_ACCEPTED_STATUS":
                    //    return RequestSupportRepository.LockAcceptedStatus(req.Data);
                    //case "REQUEST_SUPPORT_UNLOCK_ACCEPTED_STATUS":
                    //    return RequestSupportRepository.UnLockAcceptedStatus(req.Data);
                    case "REQUEST_SUPPORT_SEND":
                        return RequestSupportRepository.SendRequestSupport(req.Data);
                    case "REQUEST_SUPPORT_ADD_PROCESS_RESULT":
                        return RequestSupportRepository.AddProcessResult(req.Data);
                    #endregion

                    #region REQUEST_SUPPORT_GROUP
                    case "REQUEST_SUPPORT_GROUP_LIST":
                        return RequestSupportGroupRepository.List(req.Data);
                    #endregion

                    #region INVENTORY
                    case "INVENTORY_ADD":
                        return InventoryRepository.Add(req.Data);
                    case "INVENTORY_ADD_V2":
                        return InventoryRepository.AddV2(req.Data);
                    case "INVENTORY_EDIT":
                        return InventoryRepository.Edit(req.Data);
                    case "INVENTORY_SEARCH_LIST":
                        return InventoryRepository.SearchList(req.Data);
                    case "INVENTORY_GET_BY_ID":
                        return InventoryRepository.GetByID(req.Data);
                    case "INVENTORY_UPDATE_STATUS":
                        return InventoryRepository.UpdateStatus(req.Data);
                    case "INVENTORY_CONFIRM":
                        return InventoryRepository.Confirm(req.Data);
                    case "INVENTORY_REJECT":
                        return InventoryRepository.Reject(req.Data);
                    case "INVENTORY_CANCEL":
                        return InventoryRepository.Cancel(req.Data);
                    case "INVENTORY_DELETE":
                        return InventoryRepository.Delete(req.Data);
                    case "INVENTORY_GET_PRODUCT_VARIANT_BY_ID":
                        return InventoryRepository.GetProductVariantByID(req.Data);
                    #endregion

                    #region INVENTORY_DETAIL
                    case "INVENTORY_DETAIL_ADD":
                        return InventoryDetailRepository.Add(req.Data);
                    case "INVENTORY_DETAIL_EDIT":
                        return InventoryDetailRepository.Edit(req.Data);
                    case "INVENTORY_DETAIL_REMOVE":
                        return InventoryDetailRepository.Remove(req.Data);
                    case "INVENTORY_DETAIL_GET_BY_INVENTORY_ID":
                        return InventoryDetailRepository.GetByInventoryID(req.Data);
                    #endregion

                    #region STOCK_TAKE
                    case "STOCK_TAKE_LIST":
                        return StockTakeRepository.List(req.Data);
                    case "STOCK_TAKE_ADD":
                        return StockTakeRepository.Add(req.Data);
                    case "STOCK_TAKE_EDIT":
                        return StockTakeRepository.Edit(req.Data);
                    case "STOCK_TAKE_GET_BY_ID":
                        return StockTakeRepository.GetByID(req.Data);
                    //case "STOCK_TAKE_DETAIL":
                    //    return StockTakeRepository.Detail(req.Data);
                    case "STOCK_TAKE_ACCEPT":
                        return StockTakeRepository.Accept(req.Data);
                    case "STOCK_TAKE_REMOVE":
                        return StockTakeRepository.Remove(req.Data);
                    case "STOCK_TAKE_REJECT":
                        return StockTakeRepository.Reject(req.Data);
                    #endregion

                    #region PERIOD
                    case "PERIOD_ADD":
                        return PeriodRepository.Add(req.Data);
                    case "PERIOD_REMOVE":
                        return PeriodRepository.Remove(req.Data);
                    case "PERIOD_LIST":
                        return PeriodRepository.List(req.Data);
                    #endregion

                    #region TRANSPORT_SERVICE_ROLE
                    case "TRANSPORT_SERVICE_ROLE_LIST":
                        return TransportServiceRoleRepository.List(req.Data);
                    #endregion

                    #region CONNECT_PAGE
                    case "CONNECT_PAGE_ADD":
                        return ConnectPageRepository.AddConnectPage(req.Data);
                    case "CONNECT_PAGE_SEARCH_LIST":
                        return ConnectPageRepository.SearchListConnectPage(req.Data);
                    case "CONNECT_PAGE_RESET":
                        return ConnectPageRepository.ResetConnectPage(req.Data);
                    case "CONNECT_PAGE_COUNT_FOLLOW":
                        return ConnectPageRepository.CountFollow(req.Data);
                    case "CONNECT_PAGE_DELETE":
                        return ConnectPageRepository.DeleteConnectPage(req.Data);
                    #endregion

                    #region CONVERSION
                    case "CONVERSION_SEARCH_LIST":
                        return ConversionRepository.SearchListConversion(req.Data);
                    #endregion

                    #region MESSAGE
                    case "MESSAGE_SEARCH_LIST":
                        return MessageRepository.SearchListMessage(req.Data);
                    case "MESSAGE_SYNC_ADD":
                        return MessageRepository.AddSyncMessage(req.Data);
                    #endregion

                    #region ZALO
                    case "ZALO_CONNECT":
                        return ZaloRepository.Connect(req.Data);
                    case "ZALO_SEARCH_LIST_OA":
                        return ZaloRepository.SearchListOA(req.Data);
                    //case "ZALO_INFO":
                    //    return ZaloRepository.Info(req.Data);
                    //case "ZALO_UPDATE_TOKEN":
                    //    return ZaloRepository.UpdateToken(req.Data);
                    //case "ZALO_UPDATE_INFO_OA":
                    //    return ZaloRepository.UpdateInfoOA(req.Data);
                    case "ZALO_GET_BY_OA_ID":
                        return ZaloRepository.GetByOAID(req.Data);
                    #endregion

                    #region ZALO_CONVERSION
                    case "ZALO_CONVERSION_SEARCH_LIST":
                        return ZaloConversionRepository.SearchList(req.Data);
                    #endregion

                    #region ZALO_FOLLOW
                    case "ZALO_FOLLOW":
                        return ZaloFollowRepository.Follow(req.Data);
                    case "ZALO_UNFOLLOW":
                        return ZaloFollowRepository.UnFollow(req.Data);
                    case "ZALO_FOLLOW_SEARCH_LIST":
                        return ZaloFollowRepository.SearchList(req.Data);
                    case "ZALO_FOLLOW_SUBMIT_INFO":
                        return ZaloFollowRepository.UserSubmitInfo(req.Data);
                    #endregion

                    #region ZALO_MESSAGE
                    case "ZALO_MESSAGE_SEARCH_LIST":
                        return ZaloMessageRepository.SearchList(req.Data);
                    case "ZALO_USER_SEND_MESSAGE_TO_OA":
                        return ZaloMessageRepository.UserSendMessageToOA(req.Data);
                    case "ZALO_USER_RECEIVED_MESSAGE":
                        return ZaloMessageRepository.UserReceivedMessage(req.Data);
                    case "ZALO_USER_SEEN_MESSAGE":
                        return ZaloMessageRepository.UserSeenMessage(req.Data);
                    case "ZALO_OA_SEND_TEXT_TO_USER":
                        return ZaloMessageRepository.OASendTextToUser(req.Data);
                    #endregion

                    #region AUTO_SEND_MESSAGE
                    case "AUTO_SEND_MESSAGE_SEARCH_LIST":
                        return AutoSendMessageRepository.SearchList(req.Data);
                    case "AUTO_SEND_MESSAGE_ADD":
                        return AutoSendMessageRepository.AddAutoSendMessage(req.Data);
                    #endregion

                    #region USER_CHANNEL
                    case "USER_CHANNEL_SEARCH_BY_CHANNEL_USER_ID":
                        return UserChannelRepository.SearchByChannelUserID(req.Data);
                    case "USER_CHANNEL_WEB_UPDATE_INFORMATION":
                        return UserChannelRepository.WebUpdateInformation(req.Data);
                    case "USER_CHANNEL_SEARCH_LIST":
                        return UserChannelRepository.SearchList(req.Data);
                    #endregion

                    #region MAPS
                    case "MAPS_GET_BY_V_MAP_CODE":
                        return MapsRepository.GetByVMapCode(req.Data);
                    case "MAPS_GET_BY_MBC_2017_CODE":
                        return MapsRepository.GetByMBC2017Code(req.Data);
                    case "MAPS_GET_BY_MBC_CODE":
                        return MapsRepository.GetByMBCCode(req.Data);
                    case "MAPS_GET_BY_PWD_NAME":
                        return MapsRepository.GetByPWDName(req.Data);
                    #endregion

                    #region FEED
                    case "FEED_SEARCH_LIST":
                        return FeedRepository.SearchList(req.Data);
                    case "FEED_DELETE":
                        return FeedRepository.DeleteFeed(req.Data);
                    #endregion

                    #region COMMENT
                    case "COMMENT_SEARCH_LIST":
                        return CommentRepository.SearchList(req.Data);
                    case "COMMENT_DELETE":
                        return CommentRepository.DeleteComment(req.Data);
                    case "COMMENT_UPDATE_LIKE":
                        return CommentRepository.UpdateLikeComment(req.Data);
                    case "COMMENT_UPDATE_HIDDEN":
                        return CommentRepository.UpdateHiddenComment(req.Data);
                    #endregion

                    #region PICK_UP_ADDRESS
                    case "PICK_UP_ADDRESS_SEARCH_LIST":
                        return PickUpAddressRepository.SearchList(req.Data);
                    case "PICK_UP_ADDRESS_ADD":
                        return PickUpAddressRepository.Add(req.Data);
                    case "PICK_UP_ADDRESS_GET_BY_ID":
                        return PickUpAddressRepository.GetByID(req.Data);
                    case "PICK_UP_ADDRESS_UPDATE":
                        return PickUpAddressRepository.Update(req.Data);
                    case "PICK_UP_ADDRESS_REMOVE":
                        return PickUpAddressRepository.Remove(req.Data);
                    #endregion

                    #region LABEL_PRINT
                    case "LABEL_PRINT_SEARCH_LIST":
                        return LabelPrintRepository.SearchList(req.Data);
                    case "LABEL_PRINT_ADD":
                        return LabelPrintRepository.Add(req.Data);
                    case "LABEL_PRINT_GET_BY_ID":
                        return LabelPrintRepository.GetByID(req.Data);
                    case "LABEL_PRINT_UPDATE":
                        return LabelPrintRepository.Update(req.Data);
                    case "LABEL_PRINT_REMOVE":
                        return LabelPrintRepository.Remove(req.Data);
                    case "LABEL_PRINT_DEFAULT_ACCORDING_TO_PICK_UP":
                        return LabelPrintRepository.DefaultAccordingToPickUpAddress(req.Data);
                    #endregion

                    #region TEMPLATE_PRINT
                    case "TEMPLATE_PRINT_SEARCH_LIST":
                        return TemplatePrintRepository.SearchList(req.Data);
                    case "TEMPLATE_PRINT_UPDATE_ROLE":
                        return TemplatePrintRepository.UpdateRole(req.Data);

                    #endregion

                    #region PARTNER - ĐỐI TÁC
                    case "PARTNER_SEARCH_LIST":
                        return PartnerRepository.SearchList(req.Data);
                    #endregion

                    #region PARTNER_WEBHOOK
                    case "PARTNER_WEBHOOK_REGISTER_ADD":
                        return PartnerWebhookRepository.AddPartnerRegister(req.Data);
                    case "PARTNER_WEBHOOK_REGISTER_EDIT":
                        return PartnerWebhookRepository.EditPartnerRegister(req.Data);
                    case "PARTNER_WEBHOOK_REGISTER_DELETE":
                        return PartnerWebhookRepository.DeletePartnerRegister(req.Data);
                    case "PARTNER_WEBHOOK_REGISTER_LIST":
                        return PartnerWebhookRepository.ListPartnerRegister(req.Data);
                    case "PARTNER_WEBHOOK_REGISTER_BY_ID":
                        return PartnerWebhookRepository.GetPartnerRegisterByID(req.Data);

                    case "PARTNER_WEBHOOK_HISTORY_STATUS_LIST":
                        return PartnerWebhookRepository.ListHistoryStatus(req.Data);
                    case "PARTNER_WEBHOOK_HISTORY_STATUS_UPDATE":
                        return PartnerWebhookRepository.UpdateHistoryStatus(req.Data);
                    #endregion

                    #region INVENTORY_OUTPUT
                    case "INVENTORY_OUTPUT_SEARCH_LIST":
                        return InventoryOutputRepository.SearchList(req.Data);
                    case "INVENTORY_OUTPUT_ADD":
                        return InventoryOutputRepository.Add(req.Data);
                    case "INVENTORY_OUTPUT_GET_BY_ID":
                        return InventoryOutputRepository.GetByID(req.Data);
                    case "INVENTORY_OUTPUT_EDIT":
                        return InventoryOutputRepository.Edit(req.Data);
                    case "INVENTORY_OUTPUT_PROCESS":
                        return InventoryOutputRepository.Process(req.Data);
                    case "INVENTORY_OUTPUT_ACCEPT":
                        return InventoryOutputRepository.Accept(req.Data);
                    case "INVENTORY_OUTPUT_REJECT":
                        return InventoryOutputRepository.Reject(req.Data);
                    case "INVENTORY_OUTPUT_INFO":
                        return InventoryOutputRepository.Info(req.Data);
                    #endregion

                    #region INVENTORY_IMPORT
                    case "INVENTORY_IMPORT_SEARCH_LIST":
                        return InventoryImportRepository.SearchList(req.Data);
                    case "INVENTORY_IMPORT_ADD":
                        return InventoryImportRepository.Add(req.Data);
                    case "INVENTORY_IMPORT_GET_BY_ID":
                        return InventoryImportRepository.GetByID(req.Data);
                    case "INVENTORY_IMPORT_EDIT":
                        return InventoryImportRepository.Edit(req.Data);
                    case "INVENTORY_IMPORT_ACCEPT":
                        return InventoryImportRepository.Accept(req.Data);
                    case "INVENTORY_IMPORT_REJECT":
                        return InventoryImportRepository.Reject(req.Data);
                    case "INVENTORY_IMPORT_REMOVE":
                        return InventoryImportRepository.Remove(req.Data);
                    #endregion

                    #region COLLECTION_POST_OFFICE - Bưu cục thu gom
                    case CommandCodeConst.COLLECTION_POST_OFFICE_SEARCH_LIST:
                        return CollectionPostOfficeRepository.SearchList(req.Data);
                    case "COLLECTION_POST_OFFICE_ADD":
                        return CollectionPostOfficeRepository.Add(req.Data);
                    case "COLLECTION_POST_OFFICE_EDIT":
                        return CollectionPostOfficeRepository.Edit(req.Data);
                    case "COLLECTION_POST_OFFICE_GET_BY_ID":
                        return CollectionPostOfficeRepository.GetByID(req.Data);
                    case "COLLECTION_POST_OFFICE_SEARCH_LIST_BY_PARENT_ID":
                        return CollectionPostOfficeRepository.SearchListByParentID(req.Data);
                    case CommandCodeConst.COLLECTION_POST_OFFICE_SEARCH_LIST_RECENT:
                        return CollectionPostOfficeRepository.SearchListRecent(req.Data);
                    #endregion

                    #region REPORT
                    case "REPORT_COD_MONEY":
                        return ReportRepository.CODMoney(req.Data);
                    #endregion

                    #region GRAB
                    case "GRAB_DELIVERY_ADD_ORDER":
                        return GrabOrderRepository.GrabDeliveryAddOrder(req.Data);
                    case "GRAB_DELIVERY_CANCEL_ORDER":
                        return GrabOrderRepository.GrabDeliveryCancelOrder(req.Data);
                    case "GRAB_DELIVERY_ORDER_SEARCH":
                        return GrabOrderRepository.GrabDeliveryListOrder(req.Data);
                    #endregion

                    #region TIKTOK
                    case "TIK_PRODUCT_LIST":
                        return TiktokRepository.ListProduct(req.Data);
                    case "TIK_PRODUCT_PROCESS_LIST":
                        return TiktokRepository.ListProductProcess(req.Data);
                    case "TIK_PRODUCT_NOT_LINKED_YET_LIST":
                        return TiktokRepository.ListProductNotLinkedYet(req.Data);
                    case "TIK_PRODUCT_UPDATE_CONNECTION":
                        return TiktokRepository.ProductUpdateConnection(req.Data);
                    case "TIK_PRODUCT_CONFIG":
                        return TiktokRepository.ConfigProductByPageID(req.Data);
                    case "TIK_PRODUCT_CONFIG_CHANGE":
                        return TiktokRepository.ConfigProductChange(req.Data);
                    #endregion

                    #region ORDER_BATCH
                    case CommandCodeConst.ORDER_BATCH_ADD:
                        return OrderBatchRepository.Add(req.Data);
                    case CommandCodeConst.ORDER_BATCH_EDIT_INFO:
                        return OrderBatchRepository.EditInfo(req.Data);
                    case CommandCodeConst.ORDER_BATCH_EDIT:
                        return OrderBatchRepository.Edit(req.Data);
                    case CommandCodeConst.ORDER_BATCH_COPPY_INFO:
                        return OrderBatchRepository.CoppyInfo(req.Data);
                    case CommandCodeConst.ORDER_BATCH_DETAIL_BY_BATCH_CODE:
                        return OrderBatchRepository.DetailByBatchCode(req.Data);
                    #endregion

                    #region CONVERSATION_STICKER
                    case "CONVERSATION_STICKER_LIST":
                        return ConversationStickerRepository.SearchList(req.Data);
                    case "CONVERSATION_STICKER_BY_ID":
                        return ConversationStickerRepository.GetByID(req.Data);
                    case "CONVERSATION_STICKER_ADD":
                        return ConversationStickerRepository.Add(req.Data);
                    case "CONVERSATION_STICKER_EDIT":
                        return ConversationStickerRepository.Edit(req.Data);
                    case "CONVERSATION_STICKER_DELETE":
                        return ConversationStickerRepository.Delete(req.Data);
                    case "C_STICKER_OF_C_LIST":
                        return ConversationStickerRepository.SearchListCStickerOfC(req.Data);
                    case "C_STICKER_OF_C_UPDATE":
                        return ConversationStickerRepository.UpdateCStickerOfC(req.Data);
                    #endregion

                    #region NOTIFY
                    case "NOTIFY_LIST":
                        return NotifyRepository.NotifyList(req.Data);
                    case "NOTIFY_UPDATE":
                        return NotifyRepository.NotifyUpdate(req.Data);
                    case "NOTIFY_REMOVE":
                        return NotifyRepository.NotifyRemove(req.Data);
                    case "NOTIFY_GET_BY_ID":
                        return NotifyRepository.NotifyGetByID(req.Data);
                    #endregion

                    #region NOTIFY_CONTROL
                    case "NOTIFY_CONTROL_ADD":
                        return NotifyControlRepository.NotifyAdd(req.Data);
                    case "NOTIFY_CONTROL_LIST":
                        return NotifyControlRepository.NotifyList(req.Data);
                    case "NOTIFY_CONTROL_CHANGE_LOCK":
                        return NotifyControlRepository.NotifyChangeLock(req.Data);
                    case "NOTIFY_CONTROL_REMOVE":
                        return NotifyControlRepository.NotifyRemove(req.Data);
                    case "NOTIFY_CONTROL_GET_BY_ID":
                        return NotifyControlRepository.NotifyGetByID(req.Data);
                    case "NOTIFY_CONTROL_EDIT":
                        return NotifyControlRepository.NotifyEdit(req.Data);
                    #endregion

                    #region CUSTOMER_GROUP
                    case "CUSTOMER_GROUP_SEARCH_LIST":
                        return CustomerGroupRepository.SearchList(req.Data);
                    case "CUSTOMER_GROUP_ADD":
                        return CustomerGroupRepository.Add(req.Data);
                    case "CUSTOMER_GROUP_EDIT":
                        return CustomerGroupRepository.Edit(req.Data);
                    case "CUSTOMER_GROUP_GET_BY_ID":
                        return CustomerGroupRepository.GetByID(req.Data);
                    #endregion

                    #region EXPECTED_DELIVERY - Ngày dự kiến giao hàng
                    case CommandCodeConst.EXPECTED_DELIVERY_SEARCH_DATE:
                        return ExpectedDeliveryRepository.SearchDate(req.Data);
                    #endregion

                    #region TEST
                    case "ORDER_PAGINATION":
                        return OrderRepository.Pagination(req.Data);
                    #endregion

                    default:
                        res.Code = "01";
                        res.Message = "Không tìm thấy thông tin mã hàm.";
                        break;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GatewayController.Execute: " + ex.Message + "|| request: "  + JsonConvert.SerializeObject(req));
            }
            return res;
        }

        [HttpPost]
        [Route("ExecuteFile")]
        public ZResponse ExecuteFile()
        {
            ZResponse res = new ZResponse();
            try
            {
                var request = HttpContext.Current.Request;
                if(request == null)
                {
                    res.Code = "99";
                    res.Message = "Request không đc phép null";
                }

                if(request.Files.Count == 0)
                {
                    res.Code = "99";
                    res.Message = "Không có file upload";
                    
                }


                var file = request.Files["file"] ?? request.Files[0];
                if(file == null || file.ContentLength <= 0)
                {
                    res.Code = "99";
                    res.Message = "File không hợp lệ";
                }

                var code = request.Form["Code"];
                if(string.IsNullOrWhiteSpace(code))
                {
                    res.Code = "01";
                    res.Message = "Không tìm thấy thông tin mã hàm.";
                }
                var data = request.Form["Data"];
                switch(code)
                {
                    case "ORDER_IMPORT_EXCEL":
                        return OrderRepository.OrderImportExcel(file, code, data);
                    default:
                        res.Code = "01";
                        res.Message = "Không tìm thấy thông tin mã hàm.";
                        break;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GatewayController.ExecuteFile: " + ex.Message);
            }
            return res;
        }
    }
}
