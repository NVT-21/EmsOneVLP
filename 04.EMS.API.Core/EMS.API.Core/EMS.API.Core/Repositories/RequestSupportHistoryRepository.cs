using System;
using System.Collections.Generic;
using EMS.Library;
using EMS.Entity;
using EMS.Core;
using System.Data.Common;
using System.Data;
using Newtonsoft.Json;

namespace EMS.API.Core.Repositories
{
    public class RequestSupportHistoryRepository
    {
        public static ZResponse SearchListUpdateInformation(ZRequestSupportHistorySearchListUpdateInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZRequestSupportHistorySearchListUpdateInformationResponse> list = new List<ZRequestSupportHistorySearchListUpdateInformationResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQ_SUP_HISTORY_PKG.SEARCH_LIST_UPDATE_INFORMATION", new object[]
                {
                    req.EShippingCode,  //P_ESHIPPING_CODE  VARCHAR
                    req.ShopID,         //P_SHOP_ID         NUMBER
                    0,                  //P_RETURN_CODE     OUT NUMBER
                    0                   //P_OUT_CURSOR      OUT T_CURSOR
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
                                    list.Add(new ZRequestSupportHistorySearchListUpdateInformationResponse()
                                    {
                                        RequestSupportType = new ZBaseResponse() 
                                        {
                                            ID = string.IsNullOrEmpty(dr["REQUEST_SUPPORT_TYPE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["REQUEST_SUPPORT_TYPE_ID"].ToString()),
                                            Name = dr["REQUEST_SUPPORT_TYPE_NAME"].ToString()
                                        },
                                        OldUpdateInformation = dr["OLD_UPDATE_INFORMATION"].ToString(),
                                        NewUpdateInformation = dr["NEW_UPDATE_INFORMATION"].ToString(),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                    });
                                }
                            }
                            if (list.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(list);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                            }

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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportHistoryRepository.SearchListUpdateInformation: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}