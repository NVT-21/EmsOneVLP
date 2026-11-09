using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class EMSRepository
    {
        public static ZResponse EMSMap(ZEMSMapSearchRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.EMSMapSearch_MCS("GET", $"/api/Map/VMap/SearchV2?text={req.AddressFull}", null);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSMap: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EMSLabel(ZPartnerOrderPrint_V2Request req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.EMSLabel("POST", "/api/Label/Generate", req);
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EMSRepository.EMSMap: " + ex.Message);
            }
            return res;
        }
    }
}