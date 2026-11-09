using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Repository
{
    public class CustomerGroupRepository
    {
        private static string CUSTOMER_GROUP_SEARCH_LIST = "CUSTOMER_GROUP_SEARCH_LIST";
        private static string CUSTOMER_GROUP_ADD = "CUSTOMER_GROUP_ADD";
        private static string CUSTOMER_GROUP_GET_BY_ID = "CUSTOMER_GROUP_GET_BY_ID";
        private static string CUSTOMER_GROUP_EDIT = "CUSTOMER_GROUP_EDIT";

        public static ZResponse SearchList(ZCustomerGroupSearchListRequest request)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = CUSTOMER_GROUP_SEARCH_LIST,
                    Data = JsonConvert.SerializeObject(request)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupRepository.SearchList: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddCustomerGroup(ZCustomerGroupAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = CUSTOMER_GROUP_ADD,
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupRepository.AddCustomerGroup: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetByID(long id)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = CUSTOMER_GROUP_GET_BY_ID,
                    Data = id.ToString(),
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupRepository.GetByID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditCustomerGroup(ZCustomerGroupEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = CUSTOMER_GROUP_EDIT,
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupRepository.EditCustomerGroup: " + ex.Message);
            }
            return res;
        }

        public static List<SelectListItem> ListItemCustomerGroup()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            try
            {
                var res = CustomerGroupRepository.SearchList(new ZCustomerGroupSearchListRequest(){});
                if (res.Code == "00")
                {
                    var list = JsonConvert.DeserializeObject<List<ZCustomerGroupSearchListResponse>>(res.Data);
                    foreach (var item in list)
                    {
                        listItem.Add(new SelectListItem()
                        {
                            Text = item.Name,
                            Value = item.Id.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerGroupRepository.ListItemCustomerGroup: " + ex.Message);
            }
            return listItem;
        }
    }
}