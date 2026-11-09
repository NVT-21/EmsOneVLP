using EMS.API.GateWay;
using EMS.API.GateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;

namespace EMS.API.SyncGateWay.Repositories
{
    public class PaypostRepository
    {
        public static ZResponse EditCODMoney(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPaypostEditCODMoneyRequest>(data);
            var res = APILibraryRepository.CallApiEditCODMoney("POST", "CODMoney/EditCOD", req);
            if(res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZPaypostEditCODMoneyResponse>(res.Data);
                res.Code = ResponseData.Code;
                res.Message = ResponseData.Message;
                res.Data = ResponseData.Data;
            }
            else
            {
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("PaypostRepository.EditCODMoney: req={0}||res={1}", data, JsonConvert.SerializeObject(res)));
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
            }
            return res;
        }
    }
}