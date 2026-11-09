using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Http;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using EMS.API.Core.Repositories;
using Newtonsoft.Json;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Province")]
    public class ProvinceController : ApiController
    {
        [Route("List")]
        public ZResponse List(ZProvinceListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ProvinceRepository.List(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProvinceController.List: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetProvinceIDByProvinceName")]
        public ZResponse GetProvinceIDByProvinceName(string ProvinceName)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVINCES_PKG.GET_PRO_ID_BY_PRO_NAME", new object[]
                {
                    0,
                    ProvinceName
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProvinceController.GetProvinceIDByProvinceName: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("SearchProvince")]
        public ZResponse SearchProvince(ZProvinceSearchRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZProvinceSearchResponse oItem = null;
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_PROVINCES_PKG.SEARCH_PROVINCE", new object[]
                {
                    req.Name,           //P_PROVINCE_NAME   VARCHAR
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
                                    oItem = new ZProvinceSearchResponse()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ProvinceController.SearchProvince: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}