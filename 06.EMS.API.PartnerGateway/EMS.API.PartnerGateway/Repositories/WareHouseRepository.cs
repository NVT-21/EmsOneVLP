using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class WareHouseRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerWareHouseSearchListRequest>(data);
            return XHelper.Invoke("POST", "Partner/WareHouseSearchList", req);
        }

        public static ZResponse Add(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZPartnerWareHouseAddRequest>(data);
                ZPartnerWareHouseAddRequest handleRequest = new ZPartnerWareHouseAddRequest()
                {
                    CrmOrPaypostCode = req.CrmOrPaypostCode.Trim(),
                    WareHouseCode = req.WareHouseCode.Trim(),
                    WareHouseName = req.WareHouseName.Trim(),
                    FullName = req.FullName.Trim(),
                    MobileNumber = req.MobileNumber.Trim(),
                    ProvinceID = req.ProvinceID,
                    DistrictID = req.DistrictID,
                    WardID = req.WardID,
                    Street = req.Street.Trim(),
                    IsDefault = req.IsDefault.Trim(),
                };
                res = XHelper.Invoke("POST", "Partner/WareHouseAdd", handleRequest);
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "WareHouseRepository.Add: " + ex.Message);
            }
            return res;
        }
    }
}