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
    [RoutePrefix("RequestSupportTemp")]
    public class RequestSupportTempController : ApiController
    {
        [Route("AddRequestSupport")]
        public ZResponse Add(ZRequestSupportAddRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_TEMP_PKG.ADD_REQUEST_SUPPORT", new object[]
                {
                    0,
                    req.EShippingCode,
                    req.RequestGroup,
                    req.RequestType,
                    req.Description,
                    req.Note,
                    req.UpdateInformation,
                    req.Document,
                    "Cao", //req.Priority,
                    "Web", //req.Origin,
                    "Bưu chính chuyển phát", //req.ServiceGroup,
                    "Trong nước", //req.Scope,
                    req.ShopID,
                    req.CreatedBy,
                    0                           // P_OUT_CODE OUT VARCHAR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Tạo yêu cầu hỗ trợ thành công.";
                            //res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_CURRENT_ORDER_STATUS"].Value);
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã bưu gửi không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lưu lịch sử thất bại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Đơn hàng không có dịch vụ thu hộ - chi hộ.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Mã đơn hàng không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    //res.Message = "Trạng thái bưu gửi không cho phép thay đổi COD.";
                                    res.Message = "Loại yêu cầu không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Nhóm yêu cầu không tồn tại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Đơn hàng ở trạng thái " + XData.GetNameOrderStatus(Convert.ToString(dbCommand.Parameters["P_OUT_CURRENT_ORDER_STATUS"].Value)) + " nên không thể tạo yêu cầu";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Đơn hàng đã phát đủ số lần quy định, vui lòng liên hệ nhân viên CSKH để được hỗ trợ";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}