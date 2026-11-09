using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Repository
{
    public class NotifyControlRepository
    {
        public static ZResponse NotifyAdd(ZNotifyControlAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_CONTROL_ADD",
                    Data = JsonConvert.SerializeObject(new ZNotifyControlAddRequest()
                    {
                        NotifyType = "EMS",
                        NotifyName = req.NotifyName,
                        NotifyContent = req.NotifyContent,
                        NotifyRedirectUrl = req.NotifyRedirectUrl,
                        NotifyReceiverType = req.NotifyReceiverType,
                        NotifyReceiverGroupID = req.NotifyReceiverGroupID,
                        NotifyReceiverCRM = req.NotifyReceiverCRM,
                        NotifySendDate = req.NotifySendDate,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlRepository.NotifyAdd: " + ex.Message);
            }
            return res;
        }
        public static ZResponse NotifyList(NotifyControlSearchListModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_CONTROL_LIST",
                    Data = JsonConvert.SerializeObject(new ZNotifyControlListRequest()
                    {
                        FromDate = req.FromDate,
                        ToDate = req.ToDate,
                        IsLock = req.IsLock
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlRepository.NotifyList: " + ex.Message);
            }
            return res;
        }
        public static ZResponse NotifyChangeLock(NotifyControlChangeLockModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_CONTROL_CHANGE_LOCK",
                    Data = JsonConvert.SerializeObject(new ZNotifyControlChangeLockRequest()
                    {
                        NotifyID = req.NotifyID,
                        IsLock = req.IsLock,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlRepository.NotifyChangeLock: " + ex.Message);
            }
            return res;
        }
        public static ZResponse NotifyRemove(NotifyControlRemoveModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_CONTROL_REMOVE",
                    Data = JsonConvert.SerializeObject(new ZNotifyControlRemoveRequest()
                    {
                        NotifyID = req.NotifyID,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlRepository.NotifyRemove: " + ex.Message);
            }
            return res;
        }
        public static ZResponse NotifyGetByID(NotifyControlGetByIDModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_CONTROL_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(new ZNotifyControlGetByIDRequest()
                    {
                        NotifyID = req.NotifyID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlRepository.NotifyGetByID: " + ex.Message);
            }
            return res;
        }
        public static ZResponse NotifyEdit(ZNotifyControlEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "NOTIFY_CONTROL_EDIT",
                    Data = JsonConvert.SerializeObject(new ZNotifyControlEditRequest()
                    {
                        NotifyID = req.NotifyID,
                        NotifyType = "EMS",
                        NotifyName = req.NotifyName,
                        NotifyContent = req.NotifyContent,
                        NotifyRedirectUrl = req.NotifyRedirectUrl,
                        NotifyReceiverType = req.NotifyReceiverType,
                        NotifyReceiverGroupID = req.NotifyReceiverGroupID,
                        NotifyReceiverCRM = req.NotifyReceiverCRM,
                        NotifySendDate = req.NotifySendDate,
                        UpdatedBy = XHelper.Session.UserProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControlRepository.NotifyEdit: " + ex.Message);
            }
            return res;
        }
    }
}