using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using System.Data.Common;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("CustomerTemp")]
    public class CustomerTempController : ApiController
    {
        [Route("AddTemp")]
        public ZResponse AddTemp(ZCustomerImportAddTempRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                DateTime dt = new DateTime();

                if (req.BirthDay != "")
                {
                    dt = Convert.ToDateTime(req.BirthDay);
                }
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_TEMP_PKG.ADD_TEMP", new object[]
                {
                    0,
                    req.FullName,
                    req.MobileNumber,
                    req.Email,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.BirthDay == "" ? "" : dt.ToString("dd/MM/yyyy"),
                    req.TaxCode,
                    req.Channel,
                    req.Note,
                    req.Sticker,
                    req.IsReplace,
                    req.ImportCode
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Số điện thoại đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerTempController.AddTemp: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddCommit")]
        public ZResponse AddCommit(ZCustomerImportAddCommitRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_CUSTOMER_TEMP_PKG.ADD_COMMIT", new object[]
                {
                    0,
                    req.ImportCode,
                    req.ShopID,
                    req.CreatedBy,
                    XHelper.DeviceName
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "CustomerTempController.AddCommit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}