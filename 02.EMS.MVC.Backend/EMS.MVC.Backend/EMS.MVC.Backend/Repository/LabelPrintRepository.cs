using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Repository
{
    public class LabelPrintRepository
    {
        public static ZResponse SearchList(ZLabelPrintSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "LABEL_PRINT_SEARCH_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "LabelPrintRepository.SearchList: " + ex.Message);
            }
            return res;
        }
        public static ZResponse Add(ZLabelPrintAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "LABEL_PRINT_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "LabelPrintRepository.Add: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GetByID(ZLabelPrintGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "LABEL_PRINT_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "LabelPrintRepository.GetByID: " + ex.Message);
            }
            return res;
        }
        public static ZResponse Update(ZLabelPrintUpdateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "LABEL_PRINT_UPDATE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "LabelPrintRepository.Update: " + ex.Message);
            }
            return res;
        }
        public static ZResponse Remove(ZLabelPrintRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "LABEL_PRINT_REMOVE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "LabelPrintRepository.Remove: " + ex.Message);
            }
            return res;
        }
        public static ZResponse DefaultAccordingToPickUpAddress(ZLabelPrintDefaultAccordingToPickUpAddressRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "LABEL_PRINT_DEFAULT_ACCORDING_TO_PICK_UP",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "LabelPrintRepository.DefaultAccordingToPickUpAddress: " + ex.Message);
            }
            return res;
        }
    }
}