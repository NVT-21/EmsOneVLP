using EMS.API.Core.Repositories;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web.Http;
using System.Linq;
namespace EMS.API.Core.Controllers
{
    [RoutePrefix("District")]
    public class DistrictController : ApiController
    {
        [Route("GetByProvinceID")]
        public ZResponse GetByProvinceID(int ProvinceID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = DistrictRepository.GetByProvinceID(new ZDistrictGetByProvinceIDRequest() { ProvinceID = ProvinceID });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "DistrictController.GetByProvinceID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetDistrictIDByDistrictName")]
        public ZResponse GetDistrictIDByDistrictName(int ProvinceID, string DistrictName)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DISTRICTS_PKG.GET_DIS_ID_BY_DIS_NAME", new object[]
                {
                    0,
                    ProvinceID,
                    DistrictName
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DistrictController.GetDistrictIDByDistrictName: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchDistrict")]
        public ZResponse SearchDistrict(ZDistrictSearchRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZDistrictSearchResponse oItem = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DISTRICTS_PKG.SEARCH_DISTRICT", new object[]
                {
                    req.ProvinceID,     //P_PROVINCE_ID     NUMBER
                    req.Name,           //P_DISTRICT_NAME   VARCHAR
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
                                    oItem = new ZDistrictSearchResponse()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DistrictController.SearchDistrict: " + ex.Message);
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
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DISTRICTS_PKG.GET_BY_V_MAP_CODE", new object[]
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DistrictController.GetByVMapCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByProvinceName")]
        public ZResponse GetByProvinceName(string ProvinceName)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZDistrictGetByProvinceNameResponse> model = new List<ZDistrictGetByProvinceNameResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DISTRICTS_PKG.GET_BY_PROVINCE_NAME", new object[]
                {
                    ProvinceName,
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
                                    model.Add(new ZDistrictGetByProvinceNameResponse()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "DistrictController.GetByProvinceName: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("SearchAll")]
        //public ZResponse SearchAll()
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        List<Temp> oItem = new List<Temp>();
        //        List<string> ListTemp = new List<string>();
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_DEV_PKG.GET_ALL", new object[]
        //        {
        //            0,                  //P_RETURN_CODE OUT NUMBER
        //            0                   //P_OUT_CURSOR  OUT T_CURSOR
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
        //                if (result > 0)
        //                {
        //                    using (IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
        //                    {
        //                        while (dr.Read())
        //                        {
        //                            oItem.Add(new Temp()
        //                            {
        //                                DISTRICT_STATE = dr["DISTRICT_STATE"].ToString(),
        //                                DISTRICT_ID = dr["DISTRICT_ID"].ToString(),
        //                                DISTRICT_TITLE = dr["DISTRICT_TITLE"].ToString(),
        //                                DISTRICT_NAME = dr["DISTRICT_NAME"].ToString(),
        //                                WARD_STATE = dr["WARD_STATE"].ToString(),
        //                                WARD_ID = dr["WARD_ID"].ToString(),
        //                                WARD_TITLE = dr["WARD_TITLE"].ToString(),
        //                                WARD_NAME = dr["WARD_NAME"].ToString(),
        //                            });
        //                        }
        //                    }

        //                    var a = oItem.GroupBy(m => m.DISTRICT_ID);

        //                    foreach(var item in a)
        //                    {
        //                        if(item.Key == "5244" || item.Key == "9742")
        //                        {
        //                            var b = "";
        //                            var c = "";
        //                            var d = "";
        //                        }
        //                        if(item.Where(m => m.WARD_STATE == "A").Count() == 0)
        //                        {
        //                            ListTemp.Add(item.Key);
        //                        }
        //                    }

        //                    if (oItem != null)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(ListTemp);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
        //                    }
        //                }
        //                else
        //                {
        //                    res.Code = "99";
        //                    res.Message = "Thất bại.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "DistrictController.SearchDistrict: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
    }

    //public class Temp
    //{
    //    public string DISTRICT_STATE { get; set; }
    //    public string DISTRICT_ID { get; set; }
    //    public string DISTRICT_TITLE { get; set; }
    //    public string DISTRICT_NAME { get; set; }
    //    public string WARD_STATE { get; set; }
    //    public string WARD_ID { get; set; }
    //    public string WARD_TITLE { get; set; }
    //    public string WARD_NAME { get; set; }
    //}
}