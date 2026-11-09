using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;

namespace EMS.MVC.Backend.Repository
{
    public class PeriodRepository
    {
        public static ZResponse SearchListPeriod(ZPeriodListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PERIOD_LIST",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PeriodRepository.SearchListPeriod: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddPeriod(ZPeriodAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PERIOD_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PeriodRepository.AddPeriod: " + ex.Message);
            }
            return res;
        }

        public static ZResponse RemovePeriod(ZPeriodRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "PERIOD_REMOVE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PeriodRepository.RemovePeriod: " + ex.Message);
            }
            return res;
        }
    }
}