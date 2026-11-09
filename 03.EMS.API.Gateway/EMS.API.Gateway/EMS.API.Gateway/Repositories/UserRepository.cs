using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class UserRepository
    {
        public static ZResponse Login(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserLoginRequest>(data);
            return XHelper.Invoke("POST", "User/Login", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserAddRequest>(data);
            return XHelper.Invoke("POST", "User/Add", req);
        }
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserSearchListRequest>(data);
            if(XHelper.IsSqlInjection(req.Email) || XHelper.IsSqlInjection(req.MobileNumber) || XHelper.IsSqlInjection(req.UserGroupCode))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "User/SearchList", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserEditRequest>(data);
            return XHelper.Invoke("POST", "User/Edit", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("User/GetByID?UserID={0}", req.UserID), null);
        }
        public static ZResponse ChangeToLocked(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserChangeToLockedRequest>(data);
            return XHelper.Invoke("POST", "User/ChangeToLocked", req);
        }
        public static ZResponse ChangeToUnlocked(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserChangeToUnlockedRequest>(data);
            return XHelper.Invoke("POST", "User/ChangeToUnlocked", req);
        }
        public static ZResponse ResetPassword(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserResetPasswordRequest>(data);
            return XHelper.Invoke("POST", "User/ResetPassword", req);
        }
        public static ZResponse ChangePassword(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserChangePasswordRequest>(data);
            return XHelper.Invoke("POST", "User/ChangePassword", req);
        }
        public static ZResponse CheckCondition(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserCheckConditionRequest>(data);
            return XHelper.Invoke("POST", "User/CheckCondition", req);
        }
        public static ZResponse GetByUserName(string data)
        {
            var req = JsonConvert.DeserializeObject<ZUserGetByUserNameRequest>(data);
            return XHelper.Invoke("GET", string.Format("User/GetByUserName?UserName={0}", req.UserName), null);
        }
    }
}