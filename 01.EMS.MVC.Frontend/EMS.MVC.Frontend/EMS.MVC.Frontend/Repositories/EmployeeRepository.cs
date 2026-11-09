using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class EmployeeRepository
    {
        public static ZResponse SearchListEmployee(ZEmployeeListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeRepository.SearchListEmployee: " + ex.Message);
            }
            return res;
        }
        public static ZResponse CheckEmployeeCondition(ZEmployeeCheckConditionRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_CHECK_CONDITION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeRepository.CheckEmployeeCondition: " + ex.Message);
            }
            return res;
        }
        public static ZResponse SearchByMobileNumber(ZEmployeeGetByMobileNumberRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_GET_BY_MOBILE_NUMBER",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeRepository.SearchByMobileNumber: " + ex.Message);
            }
            return res;
        }
    }
}