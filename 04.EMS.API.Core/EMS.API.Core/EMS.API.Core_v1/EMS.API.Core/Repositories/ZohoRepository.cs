using System;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using Newtonsoft.Json;

namespace EMS.API.Core.Repositories
{
    public class ZohoRepository
    {
        #region Gửi email
        public static ZResponse SendMail(ZZohoSendMailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "ZOHO_SEND_MAIL",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZohoRepository.SendMail: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion
    }
}