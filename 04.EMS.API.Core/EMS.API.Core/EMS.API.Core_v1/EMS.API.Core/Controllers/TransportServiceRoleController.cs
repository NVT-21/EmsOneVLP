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
    [RoutePrefix("TransportServiceRole")]
    public class TransportServiceRoleController : ApiController
    {
        [Route("List")]
        public ZResponse List(ZTransportServiceRoleListRequest req)
        {
            int result = -1;
            List<ZTransportServiceRoleListResponse> lst = new List<ZTransportServiceRoleListResponse>();
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_SERVICE_ROLE_PKG.LIST_TRANSPORT_SER_ROLE", new object[]
                {
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
                                    lst.Add(new ZTransportServiceRoleListResponse()
                                    {
                                        TransportServiceRoleID = Convert.ToInt32(dr["TRANSPORT_SERVICE_ROLE_ID"]),
                                        TransportServiceID = Convert.ToInt32(dr["TRANSPORT_SERVICE_ID"]),
                                        TransportServiceIndex = Convert.ToInt32(dr["TRANSPORT_SERVICE_INDEX"]),
                                        TransportServiceLevel = Convert.ToInt32(dr["TRANSPORT_SERVICE_LEVEL"]),
                                        IsShow = Convert.ToString(dr["IS_SHOW"]),
                                        IsChoose = Convert.ToString(dr["IS_CHOOSE"]),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRoleController.List " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}