using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class AddressRepository
    {
        public static ZResponse SearchAddress(string data)
        {
            ZResponse res = new ZResponse();
            try
            {
                var req = JsonConvert.DeserializeObject<ZEMSMapSearchRequest>(data);
                if (string.IsNullOrEmpty(req.AddressFull))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập địa chỉ";
                    return res;
                }

                if (string.IsNullOrEmpty(req.AddressFull.Trim()))
                {
                    res.Code = "01";
                    res.Message = "Vui lòng nhập địa chỉ";
                    return res;
                }

                var resEMSMapSearch = EMSRepository.EMSMap(new ZEMSMapSearchRequest() { AddressFull = req.AddressFull });
                if (resEMSMapSearch.Code == "00")
                {
                    var ResponseData = JsonConvert.DeserializeObject<ZEMSMapSearchResponse>(resEMSMapSearch.Data);
                    switch (ResponseData.Code)
                    {
                        case "1":
                            res.Code = "00";
                            break;
                        default:
                            res.Code = ResponseData.Code;
                            break;
                    }
                    res.Data = JsonConvert.SerializeObject(ResponseData.data);
                    res.Message = ResponseData.Message;
                }
                else
                {
                    res.Code = resEMSMapSearch.Code;
                    res.Message = resEMSMapSearch.Message;
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("AddressRepository.SearchAddress: req={0}||res={1}", data, JsonConvert.SerializeObject(resEMSMapSearch)));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "AddressRepository.SearchAddress: " + ex.Message);
            }
            return res;
        }
    }
}