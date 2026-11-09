using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public static class ShopRepository
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopAddRequest>(data);
            return XHelper.Invoke("POST", "Shop/Add", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("Shop/GetByID?ID={0}", req.ID), null);
        }
        public static ZResponse GetHistoryByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGetHistoryByIDRequest>(data);
            return XHelper.Invoke("POST", "Shop/GetHistoryByID", req);
        }
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopSearchListRequest>(data);
            if(XHelper.IsSqlInjection(req.Status) || XHelper.IsSqlInjection(req.MobileNumber) || XHelper.IsSqlInjection(req.CustomerCode) || XHelper.IsSqlInjection(req.CrmOrPaypostCode) || XHelper.IsSqlInjection(req.IsLock) || XHelper.IsSqlInjection(req.DateType))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Shop/SearchList", req);
        }
        public static ZResponse Register(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopRegisterRequest>(data);
            return XHelper.Invoke("POST", "Shop/Register", req);
        }
        public static ZResponse Login(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopLoginRequest>(data);
            return XHelper.Invoke("POST", "Shop/Login", req);
        }
        public static ZResponse ApprovedShop(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopApprovedShopRequest>(data);
            return XHelper.Invoke("POST", "Shop/ApprovedShop", req);
        }
        public static ZResponse RejectShop(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopRejectShopRequest>(data);
            return XHelper.Invoke("POST", "Shop/RejectShop", req);
        }
        //public static ZResponse UpdateConfirmationEmail(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZShopUpdateConfirmationEmailRequest>(data);
        //    return XHelper.Invoke("POST", "Shop/UpdateConfirmationEmail", req);
        //}
        //public static ZResponse AddImage(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZShopAddImageRequest>(data);
        //    return XHelper.Invoke("POST", "Shop/AddImage", req);
        //}
        public static ZResponse ImagesByShopID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopImagesByShopIDRequest>(data);
            return XHelper.Invoke("POST", "Shop/ImagesByShopID", req);
        }
        //public static ZResponse ImageDataByImageID(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZShopImageDataByImageIDRequest>(data);
        //    return XHelper.Invoke("POST", "Shop/ImageDataByImageID", req);
        //}
        //public static ZResponse LockShop(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZShopLockShopRequest>(data);
        //    return XHelper.Invoke("POST", "Shop/LockShop", req);
        //}
        //public static ZResponse UnlockShop(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZShopUnlockShopRequest>(data);
        //    return XHelper.Invoke("POST", "Shop/UnlockShop", req);
        //}
        public static ZResponse ChangeIsLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopChangeIsLockRequest>(data);
            return XHelper.Invoke("POST", "Shop/ChangeIsLock", req);
        }
        public static ZResponse ResetPasswordShop(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopResetPasswordShopRequest>(data);
            return XHelper.Invoke("POST", "Shop/ResetPasswordShop", req);
        }
        public static ZResponse EditShop(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopEditShopRequest>(data);
            return XHelper.Invoke("POST", "Shop/EditShop", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopEditRequest>(data);
            return XHelper.Invoke("POST", "Shop/Edit", req);
        }
        public static ZResponse SalesChannelList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopSalesChannelListRequest>(data);
            return XHelper.Invoke("POST", "Shop/SalesChannelList", req);
        }
        public static ZResponse SalesChannelAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopSalesChannelAddRequest>(data);
            return XHelper.Invoke("POST", "Shop/SalesChannelAdd", req);
        }
        public static ZResponse SalesChannelRemove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopSalesChannelRemoveRequest>(data);
            return XHelper.Invoke("POST", "Shop/SalesChannelRemove", req);
        }
        public static ZResponse ChangePickUpAddress(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopChangePickUpAddressRequest>(data);
            return XHelper.Invoke("POST", "Shop/ChangePickUpAddress", req);
        }
        public static ZResponse GroupPermissionList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGroupPermissionListRequest>(data);
            return XHelper.Invoke("POST", "Shop/GroupPermissionList", req);
        }
        public static ZResponse GroupPermissionChangeIsLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGroupPermissionChangeIsLockRequest>(data);
            return XHelper.Invoke("POST", "Shop/GroupPermissionChangeIsLock", req);
        }
        public static ZResponse GroupPermissionAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGroupPermissionAddRequest>(data);
            return XHelper.Invoke("POST", "Shop/GroupPermissionAdd", req);
        }
        public static ZResponse GroupPermissionGetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGroupPermissionGetByIDRequest>(data);
            return XHelper.Invoke("POST", "Shop/GroupPermissionGetByID", req);
        }
        public static ZResponse GroupPermissionEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGroupPermissionEditRequest>(data);
            return XHelper.Invoke("POST", "Shop/GroupPermissionEdit", req);
        }
        public static ZResponse LevelGetByParentID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopLevelGetByParentIDRequest>(data);
            return XHelper.Invoke("POST", "Shop/LevelGetByParentID", req);
        }
        public static ZResponse LevelEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopLevelEditRequest>(data);
            return XHelper.Invoke("POST", "Shop/LevelEdit", req);
        }
        public static ZResponse LevelAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopLevelAddRequest>(data);
            return XHelper.Invoke("POST", "Shop/LevelAdd", req);
        }
        public static ZResponse UpdatePaymentInformation(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopUpdatePaymentInformationRequest>(data);
            return XHelper.Invoke("POST", "Shop/UpdatePaymentInformation", req);
        }
        public static ZResponse UpdateAutoRequestOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopUpdateAutoRequestOrderRequest>(data);
            return XHelper.Invoke("POST", "Shop/UpdateAutoRequestOrder", req);
        }
        public static ZResponse GetPickupInformation(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGetPickupInformationRequest>(data);
            return XHelper.Invoke("GET", "Shop/GetPickupInformation?ID=" + req.ID, null);
        }
        public static ZResponse TransportServiceByShopID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopTransportServiceByShopIDRequest>(data);
            return XHelper.Invoke("POST", "Shop/TransportServiceByShopID", req);
        }
        public static ZResponse GetListCrmOrPaypostCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGetListCrmOrPaypostCodeRequest>(data);
            return XHelper.Invoke("GET", "Shop/GetListCrmOrPaypostCode?ShopID=" + req.ShopID, null);
        }
        public static ZResponse ChangeLabelPrint(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopChangeLabelPrintRequest>(data);
            return XHelper.Invoke("POST", "Shop/ChangeLabelPrint", req);
        }
        public static ZResponse RemoveShop(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopRemoveRequest>(data);
            return XHelper.Invoke("POST", "Shop/RemoveShop", req);
        }
        public static ZResponse LockShop(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopLockRequest>(data);
            return XHelper.Invoke("POST", "Shop/LockShop", req);
        }
        public static ZResponse GetByCrmOrPaypostCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopGetByCrmOrPaypostCodeRequest>(data);
            return XHelper.Invoke("GET", string.Format("Shop/GetByCrmOrPaypostCode?CrmOrPaypostCode={0}", req.CrmOrPaypostCode), null);
        }
        public static ZResponse ForgetPasswordByEmail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopForgetPasswordByEmailRequest>(data);
            return XHelper.Invoke("POST", "Shop/ForgetPasswordByEmail", req);
        }
        public static ZResponse ForgetPasswordUpdate(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopForgetPasswordUpdateRequest>(data);
            return XHelper.Invoke("POST", "Shop/ForgetPasswordUpdate", req);
        }

        public static ZResponse SendEmailRegisterSuccess(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopSearchInfoSendEmailRequest>(data);
            return XHelper.Invoke("POST", "Shop/SendEmailRegisterSuccess", req);
        }
    }
}