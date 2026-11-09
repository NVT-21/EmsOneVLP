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
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("RequestSupportGroup")]
    public class RequestSupportGroupController : ApiController
    {
        [Route("List")]
        public ZResponse List(ZRequestSupportGroupListRequest req)
        {
            int result = -1;
            List<ZRequestSupportGroupListResponse> lst = new List<ZRequestSupportGroupListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_GROUP_PKG.LIST", new object[]
                {
                    req.ParentID,
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
                                    lst.Add(new ZRequestSupportGroupListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Name = Convert.ToString(dr["Name"]),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        CmsReasonForComplaint = Convert.ToString(dr["CMS_REASON_FOR_COMPLAINT"]),
                                    });
                                }
                            }
                            if (lst.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(lst);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportGroup.List " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}