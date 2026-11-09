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

namespace EMS.API.Core.Repositories
{
    public class CountryRepository
    {
        public static ZResponse List()
        {
            ZResponse res = new ZResponse();
            try
            {
               

                int result = -1;
                List<ZCountryListResponse> listItem = new List<ZCountryListResponse>();
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_COUNTRYS_PKG.LIST", new object[]
                {
                    0,                  //P_RETURN_CODE OUT NUMBER
                    0                   //P_OUT_CURSOR  OUT T_CURSOR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if(result > 0)
                        {
                            using(IDataReader dr = (IDataReader)dbCommand.Parameters["P_OUT_CURSOR"].Value)
                            {
                                while(dr.Read())
                                {
                                    listItem.Add(new ZCountryListResponse()
                                    {
                                        ID = dr["ID"].ToString(),
                                        Name = dr["NAME"].ToString(),
                                    });
                                }
                            }

                            if(listItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(listItem);
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "CountryRepository.List: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}