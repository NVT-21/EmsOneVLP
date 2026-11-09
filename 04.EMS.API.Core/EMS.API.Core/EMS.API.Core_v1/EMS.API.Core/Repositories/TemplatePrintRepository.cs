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

namespace EMS.API.Core.Repositories
{
    public class TemplatePrintRepository
    {
        public static ZResponse SearchList(ZTemplatePrintSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZTemplatePrintSearchListResponse> ListItem = new List<ZTemplatePrintSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TEMPLATE_PRINT_PKG.SEARCH_TEMPLATE_PRINT", new object[]
                {
                    req.PrintType,
                    req.TemplateType,
                    req.ShopID,
                    0,
                    0
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
                                    ListItem.Add(new ZTemplatePrintSearchListResponse()
                                    {
                                        TemplatePrintID = Convert.ToInt32(dr["TEMPLATE_PRINT_ID"]),
                                        Title = Convert.ToString(dr["TITLE"]),
                                        TemplatePrintParent = Convert.ToInt32(dr["TEMPLATE_PRINT_PARENT"]),
                                        PrintType = Convert.ToString(dr["PRINT_TYPE"]),
                                        QuantityChild = Convert.ToInt32(dr["QUANTITY_CHILD"]),
                                        IsChoose = Convert.ToString(dr["IS_CHOOSE"]),
                                    });
                                }
                            }
                            if (ListItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(ListItem);
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
                            res.Message = "Thất bại.";
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TemplatePrintRepository.SearchList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}