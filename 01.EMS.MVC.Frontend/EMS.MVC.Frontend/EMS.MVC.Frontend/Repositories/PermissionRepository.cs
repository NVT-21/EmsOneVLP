using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
namespace EMS.MVC.Frontend.Repositories
{
    public class PermissionRepository
    {
        public static ZResponse SearchListGroupPermission(ZShopGroupPermissionListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GROUP_PERMISSION_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.SearchListGroupPermission: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListFunctional(ZFunctionalListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_FUNCTIONAL_LIST_FUNCTIONAL",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.SearchListFunctional: " + ex.Message);
            }
            return res;
        }

        public static ZResponse ChangeFunctional(ZFunctionalChangeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_FUNCTIONAL_CHANGE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.ChangeFunctional: " + ex.Message);
            }
            return res;
        }

        public static ZResponse CheckFunctional(ZFunctionalCheckRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_FUNCTIONAL_CHECK",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.CheckFunctional: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListMenu(ZFunctionalListMenuRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_FUNCTIONAL_LIST_MENU",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionRepository.SearchListMenu: " + ex.Message);
            }
            return res;
        }
    }
}