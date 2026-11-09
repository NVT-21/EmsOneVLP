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
    [RoutePrefix("ProviderTemp")]
    public class ProviderTempController : ApiController
    {
        [Route("AddTemp")]
        public ZResponse AddTemp(ZProviderImportAddTempRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVIDER_TEMP_PKG.ADD_TEMP", new object[]
                {
                    0,
                    req.Code,
                    req.Name,
                    req.ShortName,
                    req.MobileNumber,
                    req.ProvinceID,
                    req.DistrictID,
                    req.WardID,
                    req.Address,
                    req.Email,
                    req.TaxCode,
                    req.Note,
                    req.IsLock,
                    req.ShopID,
                    req.ImportCode,
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
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            var checkError = Convert.ToString(dbCommand.Parameters["P_OUT_RETURN"].Value).Split(',');
                            var ErrorMessage = "";
                            for (var i = 0; i < checkError.Length; i++)
                            {
                                switch (Convert.ToInt32(checkError[i]))
                                {
                                    case -1:
                                        if (ErrorMessage == "")
                                        {
                                            ErrorMessage = "Mã nhà cung cấp đã tồn tại.";
                                        }
                                        else
                                        {
                                            ErrorMessage = ErrorMessage + ", " + "Mã nhà cung cấp đã tồn tại.";
                                        }
                                        break;
                                    case -2:
                                        if (ErrorMessage == "")
                                        {
                                            ErrorMessage = "Tên nhà cung cấp đã tồn tại.";
                                        }
                                        else
                                        {
                                            ErrorMessage = ErrorMessage + ", " + "Tên nhà cung cấp đã tồn tại.";
                                        }
                                        break;
                                    case -3:
                                        if (ErrorMessage == "")
                                        {
                                            ErrorMessage = "Số điện thoại đã tồn tại.";
                                        }
                                        else
                                        {
                                            ErrorMessage = ErrorMessage + ", " + "Số điện thoại đã tồn tại.";
                                        }
                                        break;
                                    default:
                                        if (ErrorMessage == "")
                                        {
                                            ErrorMessage = "Thất bại.";
                                        }
                                        else
                                        {
                                            ErrorMessage = ErrorMessage + ", " + "Thất bại.";
                                        }
                                        break;
                                }
                            }
                            res.Code = "04";
                            res.Message = ErrorMessage;
                            //switch (result)
                            //{
                            //    case -1:
                            //        res.Code = "01";
                            //        res.Message = "Mã nhà cung cấp đã tồn tại.";
                            //        break;
                            //    case -2:
                            //        res.Code = "02";
                            //        res.Message = "Tên nhà cung cấp đã tồn tại.";
                            //        break;
                            //    case -3:
                            //        res.Code = "03";
                            //        res.Message = "Số điện thoại đã tồn tại.";
                            //        break;
                            //    default:
                            //        res.Code = "99";
                            //        res.Message = "Thất bại";
                            //        break;
                            //}
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderTempController.AddTemp: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddCommit")]
        public ZResponse AddCommit(ZProviderImportAddCommitRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVIDER_TEMP_PKG.ADD_COMMIT", new object[]
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
                            res.Message = "Thêm mới thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã nhà cung cấp đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên nhà cung cấp đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProviderTempController.AddCommit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}