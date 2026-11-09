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
    [RoutePrefix("ExpectedDelivery")]
    public class ExpectedDeliveryController : ApiController
    {
        [Route("SearchDate")]
        public ZResponse SearchDate(ZExpectedDeliverySearchDateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_EXPECTED_DELIVERY_PKG.SEACH_DATE", new object[]
                {
                    0,
                    string.IsNullOrEmpty(req.Date) ? DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") : req.Date,
                    req.TransportServiceID,
                    req.SenderProvinceID,
                    req.SenderDistrictID,
                    req.ReceiverProvinceID,
                    req.ReceiverDistrictID,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = dbCommand.Parameters["P_OUT_EXPECTED_DELIVERY_DATE"].Value.ToString();
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Tư vấn cụ thể theo tuyến.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Quận/ Huyện người gửi không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Quận/ Huyện người nhận không tồn tại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ExpectedDeliveryController.SearchDate: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}