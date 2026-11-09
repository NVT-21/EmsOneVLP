using EMS.API.Core.Repositories;
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
using System.Web;
using System.Web.Http;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("TransportRegion")]
    public class TransportRegionController : ApiController
    {
        [Route("SearchList")]
        public ZResponse SearchList(ZTransportRegionSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZTransportRegionSearchListResponse> model = new List<ZTransportRegionSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_REGION_PKG.SEARCH_LIST", new object[]
                {
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0                    //P_OUT_CURSOR  OUT T_CURSOR
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
                                    model.Add(new ZTransportRegionSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        Name = dr["NAME"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        ProvincesID = dr["PROVINCES_ID"].ToString(),
                                        Description = dr["DESCRIPTION"].ToString(),
                                        CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
                                        CreatedDate = dr["CREATED_DATE"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRegionController.SearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZTransportRegionAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_REGION_PKG.ADD", new object[]
                {
                   0,
                   req.Code,            //P_CODE               VARCHAR,
                   req.Name,            //P_NAME               VARCHAR,
                   req.ParentID,        //P_PARENT_ID          NUMBER,
                   req.ProvincesID,     //P_PROVINCES_ID       VARCHAR,
                   req.Description,     //P_DESCRIPTION        VARCHAR,
                   req.CreatedBy        //P_CREATED_BY         NUMBER
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã nhóm vùng đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Tên nhóm vùng đã tồn tại. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRegionController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByParentID")]
        public ZResponse GetByParentID(int ParentID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1; 
                List<ZTransportRegionGetByParentIDResponse> model = new List<ZTransportRegionGetByParentIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_REGION_PKG.GET_BY_PARENT_ID", new object[]
                {
                    ParentID,        //P_PARENT          NUMBER
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0                    //P_OUT_CURSOR  OUT T_CURSOR
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
                                    model.Add(new ZTransportRegionGetByParentIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        Name = dr["NAME"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        ProvincesID = dr["PROVINCES_ID"].ToString(),
                                        Description = dr["DESCRIPTION"].ToString(),
                                        CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
                                        CreatedDate = dr["CREATED_DATE"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRegionController.GetByParentID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}