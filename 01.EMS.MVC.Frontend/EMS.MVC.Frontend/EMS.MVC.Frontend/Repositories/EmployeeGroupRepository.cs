using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class EmployeeGroupRepository
    {
        public static ZResponse SearchListEmployeeGroup(ZEmployeeGroupListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_GROUP_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeGroupRepository.SearchListEmployeeGroup: " + ex.Message);
            }
            return res;
        }
        public static ZResponse AddEmployeeGroup(ZEmployeeGroupAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_GROUP_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeGroupRepository.AddEmployeeGroup: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchEmployeeGroupByEmployeeGroupID(ZEmployeeGroupGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_GROUP_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeGroupRepository.SearchEmployeeGroupByEmployeeGroupID: " + ex.Message);
            }
            return res;
        }
        public static ZResponse EditEmployeeGroup(ZEmployeeGroupEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_GROUP_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeGroupRepository.EditEmployeeGroup: " + ex.Message);
            }
            return res;
        }
        public static ZResponse ChangeIsLock(ZEmployeeGroupChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_GROUP_CHANGE_IS_LOCK",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeGroupRepository.ChangeIsLock: " + ex.Message);
            }
            return res;
        }
    }
}