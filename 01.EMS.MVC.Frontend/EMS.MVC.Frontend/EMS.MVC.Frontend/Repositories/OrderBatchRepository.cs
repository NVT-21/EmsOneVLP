using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Repositories
{
    public class OrderBatchRepository
    {
        public static ZResponse AddOrderBatch(ZOrderBatchAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_BATCH_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.AddOrderBatch: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditOrderBatchInfo(ZOrderBatchEditInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_BATCH_EDIT_INFO",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.EditOrderBatchInfo: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditOrderBatch(ZOrderBatchEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_BATCH_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.EditOrderBatch: " + ex.Message);
            }
            return res;
        }

        public static ZResponse CoppyOrderBatchInfo(ZOrderBatchCoppyInfoRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_BATCH_COPPY_INFO",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.CoppyOrderBatchInfo: " + ex.Message);
            }
            return res;
        }

        public static ZResponse DetailByBatchCode(ZOrderBatchDetailByBatchCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_BATCH_DETAIL_BY_BATCH_CODE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.DetailByBatchCode: " + ex.Message);
            }
            return res;
        }
    }
}