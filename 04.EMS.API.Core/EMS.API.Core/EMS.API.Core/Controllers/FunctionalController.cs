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

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("Functional")]
    public class FunctionalController : ApiController
    {
        //[Route("List")]
        //public ZResponse List(ZFunctionalGroupListRequest req)
        //{
        //    int result = -1;
        //    List<ZFunctionalGroupListResponse> oItem = new List<ZFunctionalGroupListResponse>();
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_ROLE_FUNCTIONAL_PKG.LIST", new object[]
        //        {
        //            req.GroupPermissionID,
        //            req.IsShop,
        //            0,
        //            0
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
        //                            oItem.Add(new ZFunctionalGroupListResponse()
        //                            {
        //                                ID = string.IsNullOrEmpty(dr["FUNCTIONAL_ID"].ToString()) ? 0 : Convert.ToInt32(dr["FUNCTIONAL_ID"]),
        //                                Code = Convert.ToString(dr["FUNCTIONAL_CODE"]),
        //                                Name = Convert.ToString(dr["FUNCTIONAL_NAME"]),
        //                                FunctionalGroup = new FunctionalGroup()
        //                                {
        //                                    ID = string.IsNullOrEmpty(dr["FUNCTIONAL_GROUP_ID"].ToString()) ? 0 : Convert.ToInt32(dr["FUNCTIONAL_GROUP_ID"]),
        //                                    Code = Convert.ToString(dr["FUNCTIONAL_GROUP_CODE"]),
        //                                    Name = Convert.ToString(dr["FUNCTIONAL_GROUP_NAME"]),
        //                                },
        //                                Value = string.IsNullOrEmpty(dr["VALUE"].ToString()) ? 0 : Convert.ToInt32(dr["VALUE"]),
        //                            });
        //                        }
        //                    }
        //                    if (oItem != null)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Thành công.";
        //                        res.Data = JsonConvert.SerializeObject(oItem);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
        //                        res.Data = null;
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
        //                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "FunctionalController.List " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
    }
}