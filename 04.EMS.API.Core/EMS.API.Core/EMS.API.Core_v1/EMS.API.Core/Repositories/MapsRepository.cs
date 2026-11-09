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
using EMS.API.Core.Controllers;

namespace EMS.API.Core.Repositories
{
    public class MapsRepository
    {
        public static ZResponse GetByMBC2017Code(ZMapsGetByMBC2017CodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZMapsGetByMBC2017CodeResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_MAPS_PKG.GET_PWD_ID_BY_MBC_2017_CODE", new object[]
                {
                    Convert.ToInt32(req.ProvinceMBC2017Code),
                    Convert.ToInt32(req.DistrictMBC2017Code),
                    Convert.ToInt32(req.WardMBC2017Code),
                    0,                                          //P_RETURN_CODE OUT NUMBER
                    0                                           //P_OUT_CURSOR  OUT T_CURSOR
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
                                    model = new ZMapsGetByMBC2017CodeResponse()
                                    {
                                        ProvinceID = Convert.ToInt32(dr["PROVINCE_ID"]),
                                        ProvinceName = dr["PROVINCE_NAME"].ToString(),
                                        DistrictID = Convert.ToInt32(dr["DISTRICT_ID"]),
                                        DistrictName = dr["DISTRICT_NAME"].ToString(),
                                        WardID = Convert.ToInt32(dr["WARD_ID"]),
                                        WardName = dr["WARD_NAME"].ToString(),
                                        Street = req.Street
                                    };
                                }
                            }
                            if (model != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(model);
                                XHelper.LogToFile(LogFileType.MESSAGE, "MapsRepository.GetByMBC2017Code: " + JsonConvert.SerializeObject(req) + " || " + JsonConvert.SerializeObject(model));
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
                                res.Data = JsonConvert.SerializeObject(model);
                                XHelper.LogToFile(LogFileType.MESSAGE, "MapsRepository.GetByMBC2017Code: " + JsonConvert.SerializeObject(req) + " || " + JsonConvert.SerializeObject(model));
                            }
                        }
                        else
                        {
                            switch (result)
                            {
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "MapsRepository.GetByMBC2017Code: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}