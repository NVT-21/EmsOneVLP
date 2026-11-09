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
    public class TransportServiceRepository
    {
        public static ZResponse SearchList(ZTransportServiceSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZTransportServiceSearchListResponse> model = new List<ZTransportServiceSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_SERVICE_PKG.SEARCH_LIST", new object[]
                {
                    req.ParentID,  //P_PARENT_ID
                    string.IsNullOrEmpty(req.Channel) ? "EMSONE": req.Channel,
                    req.IsLock,    //P_IS_LOCK
                    0,             //P_RETURN_CODE OUT NUMBER
                    0              //P_OUT_CURSOR  OUT T_CURSOR
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
                                    model.Add(new ZTransportServiceSearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        Name = dr["NAME"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        SpecialService = new ZBaseResponse()
                                        {
                                            Code = dr["SPECIAL_SERVICE"].ToString(),
                                            Name = dr["SPECIAL_SERVICE"].ToString() == "Y" ? "Có" : "Không"
                                        },
                                        IsLock = dr["IS_LOCK"].ToString(),
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRepository.SearchList: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }


        public static string SearchById(string Id)
        {
            try
            {
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "Search_Service", new object[]
                {
                    0,             //v_Service OUT VARCHAR2
                    Id            //v_ServiceNumber
                }))
                {
                    XHelper.ExecuteNonQuery(dbCommand);
                    var result = dbCommand.Parameters[0].Value.ToString();
                    return result;
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRepository.SearchList: " + ex.Message);
                return "";
            }
        }

        public static ZResponse SearchListByShopID(ZTransportServiceSearchListByShopIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZTransportServiceSearchListByShopIDResponse> listItem = new List<ZTransportServiceSearchListByShopIDResponse>();
                if(req.ParentID == 0)
                {
                    var reqMainTransportService = SearchListTransportServiceByShopID(new ZTransportServiceSearchListByShopIDRequest() 
                    {
                        ShopID = req.ShopID,
                        IsLock = req.IsLock,
                        ParentID = 1
                    });
                    if(reqMainTransportService.Code == "00")
                    {
                        var listMainTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(reqMainTransportService.Data);
                        listItem.AddRange(listMainTransportService);
                    }

                    var reqExtraTransportService = SearchListTransportServiceByShopID(new ZTransportServiceSearchListByShopIDRequest()
                    {
                        ShopID = req.ShopID,
                        IsLock = req.IsLock,
                        ParentID = 2
                    });
                    if (reqExtraTransportService.Code == "00")
                    {
                        var listExtraTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(reqExtraTransportService.Data);
                        listItem.AddRange(listExtraTransportService);
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
                    res = SearchListTransportServiceByShopID(req);
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRepository.SearchListByShopID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SearchListTransportServiceByShopID(ZTransportServiceSearchListByShopIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZTransportServiceSearchListByShopIDResponse> listItem = new List<ZTransportServiceSearchListByShopIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_SERVICE_PKG.SEARCH_LIST_BY_SHOP_ID", new object[]
                {
                    req.ShopID,
                    req.ParentID,
                    string.IsNullOrEmpty(req.Channel) ? "EMSONE": req.Channel,
                    req.IsLock,
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0,                   //P_OUT_CURSOR  OUT T_CURSOR
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
                                    listItem.Add(new ZTransportServiceSearchListByShopIDResponse()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Code = dr["CODE"].ToString(),
                                        Name = dr["NAME"].ToString(),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        SpecialService = dr["SPECIAL_SERVICE"].ToString(),
                                        IsLock = dr["IS_LOCK"].ToString(),
                                        IsDefault = dr["IS_DEFAULT"].ToString(),
                                        Sort = Convert.ToInt32(dr["SORT"]),
                                        Description = dr["DESCRIPTION"].ToString(),
                                        CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
                                        CreatedDate = dr["CREATED_DATE"].ToString()
                                    });
                                }
                            }

                            if (listItem.Count > 0)
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRepository.SearchListTransportServiceByShopID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}