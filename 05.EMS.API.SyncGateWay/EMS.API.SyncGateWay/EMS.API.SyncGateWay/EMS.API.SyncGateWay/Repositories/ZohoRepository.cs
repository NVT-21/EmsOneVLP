using EMS.API.GateWay;
using EMS.API.GateWay.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.API.SyncGateWay.Repositories
{
    public class ZohoRepository
    {
        #region Zoho
        public static ZResponse GetAccessTokenZoho()
        {
            return APILibraryRepository.CallApiTokenRefreshZohoCRM();
        }
        public static ZResponse GetZohoCRMLeadsPhone(string data)
        {
            var req = JsonConvert.DeserializeObject<ZApiZohoCRMLeadsRequest>(data);
            return APILibraryRepository.CallApiGetZohoCRMLeads(req.zohoToken, req.phone);
        }
        public static ZResponse GetZohoCRMLeadsEmail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZApiZohoCRMLeadsRequest>(data);
            return APILibraryRepository.CallApiGetZohoCRMLeadsEmail(req.zohoToken, req.phone);
        }
        public static ZResponse CreateLeadsZohoCRM(string data)
        {
            return APILibraryRepository.CallApiCreateLeadsZohoCRM(data);
        }
        public static ZResponse SendMail(string data)
        {
            var res = APILibraryRepository.SendMail(data);
            if(res.Code == "00")
            {
                var ResponseData = JsonConvert.DeserializeObject<ZZohoSendMailResponse>(res.Data);
                if(ResponseData.message == "OK")
                {
                    res.Code = "00";
                    res.Message = "Gửi email thành công";
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Gửi email thất bại";
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ZohoRepository.SendMail: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
                }
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ZohoRepository.SendMail: request={0}||response={1}", data, JsonConvert.SerializeObject(res)));
            }
            return res;
        }
        #endregion
    }
}