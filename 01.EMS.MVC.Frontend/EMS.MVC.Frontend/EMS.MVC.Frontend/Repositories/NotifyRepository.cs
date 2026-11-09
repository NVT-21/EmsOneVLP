using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Repositories
{
    public class NotifyRepository
    {
        public static ZResponse NotifyList(NotifyListModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_LIST",
                    Data = JsonConvert.SerializeObject(
                        new ZNotifyListRequest()
                        {
                            ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                            RowLimit = req.RowLimit,
                            FromDate = req.FromDate,
                            ToDate = req.ToDate,
                            NotifyType = req.NotifyType,
                            IsCheck = req.IsCheck
                        }
                    )
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyRepository.NotifyList: " + ex.Message);
            }
            return res;
        }
        public static ZResponse NotifyUpdate(NotifyUpdateModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_UPDATE",
                    Data = JsonConvert.SerializeObject(new ZNotifyUpdateRequest()
                    {
                        StrID = req.StrID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyRepository.NotifyUpdate: " + ex.Message);
            }
            return res;
        }
        public static ZResponse NotifyRemove(NotifyUpdateModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_REMOVE",
                    Data = JsonConvert.SerializeObject(new ZNotifyUpdateRequest()
                    {
                        ID = req.ID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyRepository.NotifyRemove: " + ex.Message);
            }
            return res;
        }
        public static ZResponse NotifyGetByID(NotifyGetByIDModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(new ZNotifyGetByIDRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        ID = req.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyRepository.NotifyGetByID: " + ex.Message);
            }
            return res;
        }
    }
}