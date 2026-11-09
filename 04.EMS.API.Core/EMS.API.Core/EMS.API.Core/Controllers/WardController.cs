using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web.Http;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using EMS.API.Core.Repositories;
using Newtonsoft.Json;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Ward")]
    public class WardController : ApiController
    {
        [Route("Gets")]
        public ZResponse Gets()
        {
            ZResponse res = new ZResponse();
            List<ZWardGetsResponse> ListItem = new List<ZWardGetsResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WARDS_PKG.GETS", new object[]
                {
                    0,                  //P_RETURN_CODE OUT NUMBER
                    0                   //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    int result = -1;
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while (dr.Read())
                                {
                                    ListItem.Add(new ZWardGetsResponse()
                                    {
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        ProvinceTitle = Convert.ToString(dr["PROVINCE_TITLE"]),
                                        ProvinceName = Convert.ToString(dr["PROVINCE_NAME"]),

                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        DistrictTitle = Convert.ToString(dr["DISTRICT_TITLE"]),
                                        DistrictName = Convert.ToString(dr["DISTRICT_NAME"]),

                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        WardTitle = Convert.ToString(dr["WARD_TITLE"]),
                                        WardName = Convert.ToString(dr["WARD_NAME"]),
                                    });
                                }
                            }

                            if (ListItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(ListItem);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WardController.Gets: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByDistrictID")]
        public ZResponse GetByDistrictID(int DistrictID, int Type = 0)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = WardRepository.GetByDistrictID(new ZWardGetByDistrictIDRequest() { DistrictID = DistrictID, Type = Type });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "WardController.GetByDistrictID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetWardIDByWardName")]
        public ZResponse GetWardIDByWardName(int DistrictID, string WardName)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WARDS_PKG.GET_WARD_ID_BY_WARD_NAME", new object[]
                {
                    0,
                    DistrictID,
                    WardName
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        res.Code = "00";
                        res.Message = "Thành công.";
                        res.Data = JsonConvert.SerializeObject(new { ID = result });
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WardController.GetWardIDByWardName: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchWard")]
        public ZResponse SearchWard(ZWardSearchRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZWardSearchResponse oItem = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WARDS_PKG.SEARCH_WARD", new object[]
                {
                    req.DistrictID,     //P_DISTRICT_ID     NUMBER
                    req.Name,           //P_WARD_NAME       VARCHAR
                    req.VMapCode,       //P_V_MAP_CODE      NUMBER
                    0,                  //P_RETURN_CODE OUT NUMBER
                    0                   //P_OUT_CURSOR  OUT T_CURSOR
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
                                while (dr.Read())
                                {
                                    oItem = new ZWardSearchResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString()
                                    };
                                }
                            }

                            if (oItem != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oItem);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WardController.SearchWard: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByVMapCode")]
        public ZResponse GetByVMapCode(int VMapCode)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZDistrictListResponse> oItem = new List<ZDistrictListResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WARDS_PKG.GET_BY_V_MAP_CODE", new object[]
                {
                    VMapCode,       //P_V_MAP_CODE NUMBER,
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
                                while (dr.Read())
                                {
                                    oItem.Add(new ZDistrictListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString()
                                    });
                                }
                            }

                            if (oItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(oItem);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "02";
                                    res.Message = "Mã VMapCode phải khác 0";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WardController.GetByVMapCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [HttpPost]
        [Route("GetByParentName")]
        public ZResponse GetByParentName(ZWardGetByParentNameRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZWardGetByParentNameResponse> model = new List<ZWardGetByParentNameResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_WARDS_PKG.GET_BY_PARENT_NAME", new object[]
                {
                    req.ProvinceName,
                    req.DistrictName,
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
                                while (dr.Read())
                                {
                                    model.Add(new ZWardGetByParentNameResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = dr["NAME"].ToString()
                                    });
                                }
                            }

                            if (model.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
                            }
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Tên Tỉnh/TP không được rỗng.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên Quận/Huyện không được rỗng.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "WardController.GetByParentName: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}