using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Otp")]
    public class OtpController : ApiController
    {
        [Route("VerifyCode")]
        public ZResponse VerifyCode(ZOtpVerifyCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_OTP_PKG.VERIFY_CODE", new object[]
                {
                    0,
                    req.OtpID,
                    req.OtpCode,
                    0,
                    0,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[00].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                            res.Data = JsonConvert.SerializeObject(new ZOtpVerifyCodeResponse()
                            {
                                EmployeeID = Convert.ToInt32(dbCommand.Parameters[03].Value),
                                MobileNumber = dbCommand.Parameters[04].Value.ToString(),
                                OtpCode = dbCommand.Parameters[05].Value.ToString()
                            });
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã xác thực không đúng.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mã xác thực đã được sử dụng.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Mã xác thực đã bị hủy bỏ.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Mã xác thực hết hiệu lực.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Tài khoản không tồn tại.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "OtpController.VerifyCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}