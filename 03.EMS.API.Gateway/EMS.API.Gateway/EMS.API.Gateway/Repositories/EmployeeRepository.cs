using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public static class EmployeeRepository
    {
        public static ZResponse Login(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeLoginRequest>(data);
            return XHelper.Invoke("POST", "Employee/Login", req);
        }
        public static ZResponse LoginV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeLoginRequest>(data);
            return XHelper.Invoke("POST", "Employee/LoginV2", req);
        }
        public static ZResponse CountAccount(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeCountAccountRequest>(data);
            return XHelper.Invoke("POST", "Employee/CountAccount", req);
        }
        public static ZResponse ListAccount(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeListAcountRequest>(data);
            return XHelper.Invoke("POST", "Employee/ListAccount", req);
        }
        public static ZResponse AddNew(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeAddNewRequest>(data);
            return XHelper.Invoke("POST", "Employee/AddNew", req);
        }
        public static ZResponse ShopAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeShopAddRequest>(data);
            return XHelper.Invoke("POST", "Employee/ShopAdd", req);
        }
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeListRequest>(data);
            return XHelper.Invoke("POST", "Employee/List", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeEditRequest>(data);
            return XHelper.Invoke("POST", "Employee/Edit", req);
        }
        public static ZResponse ShopEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeShopEditRequest>(data);
            return XHelper.Invoke("POST", "Employee/ShopEdit", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("Employee/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }
        public static ZResponse GetByMobileNumber(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGetByMobileNumberRequest>(data);
            return XHelper.Invoke("GET", string.Format("Employee/GetByMobileNumber?MobileNumber={0}", req.MobileNumber), null);
        }
        public static ZResponse GetByGroupPermissionID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGetByGroupPermissionIDRequest>(data);
            return XHelper.Invoke("GET", "Employee/GetByGroupPermissionID?GroupPermissionID=" + req.GroupPermissionID + "&ShopID=" + req.ShopID, null);
        }
        public static ZResponse ChangeIsLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeChangeIsLockRequest>(data);
            return XHelper.Invoke("POST", "Employee/ChangeIsLock", req);
        }
        public static ZResponse ResetPassword(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeResetPasswordRequest>(data);
            return XHelper.Invoke("POST", "Employee/ResetPassword", req);
        }
        public static ZResponse ChangePassword(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeChangePasswordRequest>(data);
            return XHelper.Invoke("POST", "Employee/ChangePassword", req);
        }
        public static ZResponse ChangePasswordV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeChangePasswordRequest>(data);
            return XHelper.Invoke("POST", "Employee/ChangePasswordV2", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeRemoveRequest>(data);
            return XHelper.Invoke("POST", "Employee/Remove", req);
        }
        public static ZResponse TimeActiveAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeAddTimeActiveRequest>(data);
            return XHelper.Invoke("POST", "Employee/TimeActiveAdd", req);
        }
        public static ZResponse TimeActiveEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeEditTimeActiveRequest>(data);
            return XHelper.Invoke("POST", "Employee/TimeActiveEdit", req);
        }
        public static ZResponse TimeActiveGetByEmployeeID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGetTimeActiveRequest>(data);
            return XHelper.Invoke("POST", "Employee/TimeActiveGetByEmployeeID", req);
        }
        public static ZResponse CheckActive(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeCheckTimeActiveRequest>(data);
            return XHelper.Invoke("POST", "Employee/CheckActive", req);
        }
        //public static ZResponse GroupPermissionList(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZEmployeeGroupPermissionListRequest>(data);
        //    return XHelper.Invoke("POST", "Employee/GroupPermissionList", req);
        //}
        //public static ZResponse GroupPermissionAdd(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZEmployeeGroupPermissionAddRequest>(data);
        //    return XHelper.Invoke("POST", "Employee/GroupPermissionAdd", req);
        //}
        //public static ZResponse GroupPermissionEdit(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZEmployeeGroupPermissionEditRequest>(data);
        //    return XHelper.Invoke("POST", "Employee/GroupPermissionEdit", req);
        //}
        public static ZResponse GroupPermissionRemove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeGroupPermissionRemoveRequest>(data);
            return XHelper.Invoke("POST", "Employee/GroupPermissionRemove", req);
        }
        public static ZResponse CheckCondition(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeCheckConditionRequest>(data);
            return XHelper.Invoke("POST", "Employee/CheckCondition", req);
        }
        public static ZResponse ListEmployeeInPermission(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeListEmployeeInPermissionRequest>(data);
            return XHelper.Invoke("POST", "Employee/ListEmployeeInPermission", req);
        }
        public static ZResponse ListEmployeeByEmployeeGroupID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeListEmployeeByEmployeeGroupIDRequest>(data);
            return XHelper.Invoke("POST", "Employee/ListEmployeeByEmployeeGroupID", req);
        }
        public static ZResponse ChangeReadyReceiveRequestOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZEmployeeChangeReadyReceiveRequestOrderRequest>(data);
            return XHelper.Invoke("POST", "Employee/ChangeReadyReceiveRequestOrder", req);
        }
    }
}