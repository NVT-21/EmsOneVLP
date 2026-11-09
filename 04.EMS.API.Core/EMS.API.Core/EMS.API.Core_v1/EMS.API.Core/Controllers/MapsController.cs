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
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Maps")]
    public class MapsController : ApiController
    {
        [Route("GetByVMapCode")]
        [HttpPost]
        public ZResponse GetByVMapCode(ZMapsGetByVMapCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZMapsGetByVMapCodeResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MAPS_PKG.GET_BY_V_MAP_CODE", new object[]
                {
                    req.ProvinceVMapCode,
                    req.DistrictVMapCode,
                    req.WardVMapCode,
                    0,              //P_RETURN_CODE OUT NUMBER
                    0               //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                if (dr.Read())
                                {
                                    model = new ZMapsGetByVMapCodeResponse()
                                    {
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        ProvinceName = dr["PROVINCE_NAME"].ToString(),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        DistrictName = dr["DISTRICT_NAME"].ToString(),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        WardName = dr["WARD_NAME"].ToString(),
                                        Street = req.StreetVMapCode
                                    };
                                }
                            }
                            if(model != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã VMapCode của Tỉnh/Thành phố phải khác 0";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã VMapCode của Quận/Huyện phải khác 0";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mã VMapCode Phường/Xã phải khác 0";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "MapsController.GetByVMapCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByPWDName")]
        [HttpPost]
        public ZResponse GetByPWDName(ZMapsGetByPWDNameRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZMapsGetByPWDNameResponse model = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MAPS_PKG.GET_PWD_ID_BY_NAME", new object[]
                {
                    req.ProvinceName,
                    req.DistrictName,
                    req.WardName,
                    0,              //P_RETURN_CODE OUT NUMBER
                    0               //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                if (dr.Read())
                                {
                                    model = new ZMapsGetByPWDNameResponse()
                                    {
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        WardID = Convert.ToInt32(dr["WARD_ID"])
                                    };
                                }
                            }
                            if(model != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Tỉnh/Thành phố không tồn tại";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Quận/Huyện không tồn tại";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Phường/Xã không tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "MapsController.GetByPWDName: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByMBC2017Code")]
        [HttpPost]
        public ZResponse GetByMBC2017Code(ZMapsGetByMBC2017CodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = MapsRepository.GetByMBC2017Code(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "MapsController.GetByMBC2017Code: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}
