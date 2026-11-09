using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Log")]
    public class LogController : ApiController
    {
        [Route("WriteLog")]
        public ZResponse WriteLog(object req)
        {
            ZResponse res = new ZResponse();
            try
            {
                XHelper.LogToFile(LogFileType.TRACE, "LogController.WriteLog" + JsonConvert.SerializeObject(req));
                res.Code = "00";
                res.Message = "Ghi log thành công";
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "LogController.WriteLog" + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}