using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class CustomerGroupRepository
    {
        public static ZResponse SearchListCustomerGroup(ZCustomerSearchGroupRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SEARCH_GROUP",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupRepository.SearchListCustomerGroup: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddNewCustomerGroup(ZCustomerAddNewGroupRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ADDNEW_GROUP",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupRepository.AddNewCustomerGroup: " + ex.Message);
            }
            return res;
        }
    }
}