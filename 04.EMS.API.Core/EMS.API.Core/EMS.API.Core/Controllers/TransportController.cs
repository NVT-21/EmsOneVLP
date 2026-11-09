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
    [RoutePrefix("Transport")]
    public class TransportController : ApiController
    {
        #region Service
        
       
        //[Route("ServiceEdit")]
        //public ZResponse ServiceEdit(ZTransportServiceEditRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.SERVICE_EDIT", new object[]
        //        {
        //           0,
        //           req.ID,              //P_ID                 VARCHAR,
        //           req.Name,            //P_NAME               VARCHAR,
        //           req.SpecialService,  //P_SPECIAL_SERVICE    NUMBER,
        //           req.UpdatedBy        //P_UPDATED_BY         NUMBER
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Không tìm thấy dịch vụ.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Tên dịch vụ đã tồn tại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.ServiceEdit: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("ServiceCheck")]
        //public ZResponse ServiceCheck(ZTransportServiceCheckRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.SERVICE_CHECK", new object[]
        //        {
        //            0,
        //            req.TransportService,
        //            0
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Lấy dữ liệu thành công";
        //                    res.Data = dbCommand.Parameters["P_OUT_SERVICE_ID"].Value.ToString();
        //                }
        //                else
        //                {
        //                    res.Code = "01";
        //                    res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.ServiceCheck: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        #endregion

        #region RegionGroup
        

        //[Route("RegionGroupList")]
        //public ZResponse RegionGroupList()
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    List<ZTransportRegionGroupListResponse> model = new List<ZTransportRegionGroupListResponse>();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.REGION_GROUP_LIST", new object[]
        //        {
        //            0,                   //P_RETURN_CODE OUT NUMBER
        //            0                    //P_OUT_CURSOR  OUT T_CURSOR
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
        //                            model.Add(new ZTransportRegionGroupListResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                Code = dr["CODE"].ToString(),
        //                                Name = dr["NAME"].ToString(),
        //                                Description = dr["DESCRIPTION"].ToString(),
        //                                CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
        //                                CreatedDate = dr["CREATED_DATE"].ToString()
        //                            });
        //                        }
        //                    }

        //                    if (model.Count > 0)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(model);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
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
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.RegionGroupList: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        #endregion

        #region Region
        
        //[Route("RegionList")]
        //public ZResponse RegionList()
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    List<ZTransportRegionListResponse> model = new List<ZTransportRegionListResponse>();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.REGION_LIST", new object[]
        //        {
        //            0,                   //P_RETURN_CODE OUT NUMBER
        //            0                    //P_OUT_CURSOR  OUT T_CURSOR
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
        //                            model.Add(new ZTransportRegionListResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                Code = dr["CODE"].ToString(),
        //                                Name = dr["NAME"].ToString(),
        //                                ParentID = Convert.ToInt32(dr["PARENT_ID"]),
        //                                ProvincesID = dr["PROVINCES_ID"].ToString(),
        //                                Description = dr["DESCRIPTION"].ToString(),
        //                                CreatedBy = Convert.ToInt32(dr["CREATED_BY"]),
        //                                CreatedDate = dr["CREATED_DATE"].ToString()
        //                            });
        //                        }
        //                    }

        //                    if (model.Count > 0)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(model);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
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
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.RegionList: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        
        #endregion

        #region Postage
        
        
        
        //[Route("PostageExtraServiceEdit")]

        //public ZResponse PostageExtraServiceEdit(ZTransportPostageExtraServiceEditRequest req)
        //{
        //    int result = -1;
        //    ZTransportPostageBasicInfoAddResponse model = new ZTransportPostageBasicInfoAddResponse();
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_EXTRA_SERVICE_EDIT", new object[]
        //        {
        //           0,
        //           req.PostageID,
        //           req.ExtraServiceVAT,                     //VAT
        //           req.ExtraServiceID,
        //           req.ExtraServicePostage.Checked,         //Bưu gửi
        //           req.ExtraServicePostage.Value,
        //           req.ExtraServiceQuantity.Checked,        //Số lượng sản phẩm
        //           req.ExtraServiceQuantity.Value,
        //           req.ExtraServiceHeavyGoods.Checked,      //Hàng cồng kềnh
        //           req.ExtraServiceHeavyGoods.Value,
        //           req.ExtraServiceLightGoods.Checked,      //Hàng nhẹ
        //           req.ExtraServiceLightGoods.Value,
        //           req.ExtraServiceRange,               //Phạm vi
        //           req.ExtraServiceWeight,               //Khối lượng
        //           req.ExtraServiceDeclaredValue.Checked,   // Giá trị khái giá
        //           req.ExtraServiceDeclaredValue.Value,
        //           req.ExtraServiceDeclaredValue.MinValue,
        //           req.ExtraServiceDeclaredValue.MaxValue,
        //           req.ExtraServiceMainFee.Checked,         //Cước chính
        //           req.ExtraServiceMainFee.Value,
        //           req.ExtraServiceMainFee.MinValue,
        //           req.ExtraServiceMainFee.MaxValue,
        //           req.ExtraServiceSurcharge.Checked,       //Phụ phí
        //           req.ExtraServiceSurcharge.Value,
        //           req.ExtraServiceSurcharge.MinValue,
        //           req.ExtraServiceSurcharge.MaxValue,
        //           req.UpdateBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Không tìm thấy bảng cước";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Trạng thái không được phép chỉnh sửa. Vui lòng kiểm tra lại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại";
        //                            break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageExtraServiceEdit: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        
        //[Route("PostageMainFeeByID")]
        //public ZResponse PostageMainFeeByID(ZTransportPostageMainFeeByIDRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    ZTransportPostageMainFeeByIDResponse model = new ZTransportPostageMainFeeByIDResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_MAIN_FEE_BY_ID", new object[]
        //        {
        //            req.PostageID,
        //            0,                   //P_RETURN_CODE OUT NUMBER
        //            0                    //P_OUT_CURSOR  OUT T_CURSOR
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
        //                            model = new ZTransportPostageMainFeeByIDResponse()
        //                            {
        //                                PostageID = Convert.ToInt32(dr["ID"]),
        //                                PostageCode = dr["CODE"].ToString(),
        //                                PostageName = dr["NAME"].ToString(),
        //                                MainFeeVAT = dr["MAIN_FEE_VAT"].ToString(),
        //                                MainFeeRange = Convert.ToInt32(dr["MAIN_FEE_RANGE"]),
        //                                Status = dr["STATUS"].ToString(),
        //                                UpdateBy = Convert.ToInt32(dr["UPDATED_BY"]),
        //                                UpdateDate = dr["UPDATED_DATE"].ToString()
        //                            };
        //                        }
        //                    }
        //                    res.Code = "00";
        //                    res.Message = "Lấy dữ liệu thành công";
        //                    res.Data = JsonConvert.SerializeObject(model);
        //                }
        //                else
        //                {
        //                    res.Code = "80";
        //                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageMainFeeByID: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //[Route("PostageSurchargeByID")]
        //public ZResponse PostageSurchargeByID(ZTransportPostageSurchargeByIDRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    ZTransportPostageSurchargeByIDResponse model = new ZTransportPostageSurchargeByIDResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_SURCHARGE_BY_ID", new object[]
        //        {
        //            req.PostageID,
        //            0,                   //P_RETURN_CODE OUT NUMBER
        //            0                    //P_OUT_CURSOR  OUT T_CURSOR
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
        //                            model = new ZTransportPostageSurchargeByIDResponse()
        //                            {
        //                                PostageID = Convert.ToInt32(dr["ID"]),
        //                                PostageCode = dr["CODE"].ToString(),
        //                                PostageName = dr["NAME"].ToString(),
        //                                SurchargeVAT = dr["S_VAT"].ToString(),
        //                                Status = dr["STATUS"].ToString(),
        //                                Fuel = new XValueModel()
        //                                {
        //                                    Checked = dr["S_FUEL"].ToString(),
        //                                    Unit = Convert.ToInt32(dr["S_FUEL_UNIT"]),
        //                                    Value = Convert.ToDouble(dr["S_FUEL_VALUE"]),
        //                                    MinValue = Convert.ToInt64(dr["S_FUEL_MIN_VALUE"]),
        //                                    MaxValue = Convert.ToInt64(dr["S_FUEL_MAX_VALUE"]),
        //                                    InOfProvince = dr["S_FUEL_IN_OF_PROVINCE"].ToString(),
        //                                    OutOfProvince = dr["S_FUEL_OUT_OF_PROVINCE"].ToString()
        //                                },
        //                                RemoteArea = new XValueModel()
        //                                {
        //                                    Checked = dr["S_REMOTE_AREA"].ToString(),
        //                                    Unit = Convert.ToInt32(dr["S_REMOTE_AREA_UNIT"]),
        //                                    Value = Convert.ToDouble(dr["S_REMOTE_AREA_VALUE"]),
        //                                    MinValue = Convert.ToInt64(dr["S_REMOTE_AREA_MIN_VALUE"]),
        //                                    MaxValue = Convert.ToInt64(dr["S_REMOTE_AREA_MAX_VALUE"]),
        //                                    InOfProvince = dr["S_REMOTE_AREA_IN_OF_PROVINCE"].ToString(),
        //                                    OutOfProvince = dr["S_REMOTE_AREA_OUT_OF_PROVINCE"].ToString()
        //                                },
        //                                IslandsArea = new XValueModel()
        //                                {
        //                                    Checked = dr["S_ISLANDS_AREA"].ToString(),
        //                                    Unit = Convert.ToInt32(dr["S_ISLANDS_AREA_UNIT"]),
        //                                    Value = Convert.ToDouble(dr["S_ISLANDS_AREA_VALUE"]),
        //                                    MinValue = Convert.ToInt64(dr["S_ISLANDS_AREA_MIN_VALUE"]),
        //                                    MaxValue = Convert.ToInt64(dr["S_ISLANDS_AREA_MAX_VALUE"]),
        //                                    InOfProvince = dr["S_ISLANDS_AREA_IN_OF_PROVINCE"].ToString(),
        //                                    OutOfProvince = dr["S_ISLANDS_AREA_OUT_OF_PROVINCE"].ToString()
        //                                },
        //                                UpdateBy = Convert.ToInt32(dr["UPDATED_BY"]),
        //                                UpdateDate = dr["UPDATED_DATE"].ToString()
        //                            };
        //                        }
        //                    }
        //                    res.Code = "00";
        //                    res.Message = "Lấy dữ liệu thành công";
        //                    res.Data = JsonConvert.SerializeObject(model);
        //                }
        //                else
        //                {
        //                    res.Code = "80";
        //                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageSurchargeByID: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        //[Route("PostageExtraServiceByID")]
        //public ZResponse PostageExtraServiceByID(ZTransportPostageExtraServiceByIDRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    ZTransportPostageExtraServiceByIDResponse model = new ZTransportPostageExtraServiceByIDResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_EXTRA_SERVICE_BY_ID", new object[]
        //        {
        //            req.PostageID,
        //            0,                   //P_RETURN_CODE OUT NUMBER
        //            0                    //P_OUT_CURSOR  OUT T_CURSOR
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
        //                            model = new ZTransportPostageExtraServiceByIDResponse()
        //                            {
        //                                PostageID = Convert.ToInt32(dr["ID"]),
        //                                PostageCode = dr["CODE"].ToString(),
        //                                PostageName = dr["NAME"].ToString(),
        //                                ExtraServiceVAT = dr["ES_VAT"].ToString(),
        //                                ExtraServiceID = Convert.ToInt32(dr["EXTRA_SERVICE_ID"]),
        //                                ExtraServicePostage = new XValueModel()
        //                                {
        //                                    Checked = dr["ES_POSTAGE"].ToString(),
        //                                    Value = Convert.ToInt32(dr["ES_POSTAGE_VALUE"])
        //                                },
        //                                ExtraServiceQuantity = new XValueModel()
        //                                {
        //                                    Checked = dr["ES_QUANTITY"].ToString(),
        //                                    Value = Convert.ToInt32(dr["ES_QUANTITY_VALUE"])
        //                                },
        //                                ExtraServiceHeavyGoods = new XValueModel()
        //                                {
        //                                    Checked = dr["ES_HEAVY_GOODS"].ToString(),
        //                                    Value = Convert.ToInt32(dr["ES_HEAVY_GOODS_VALUE"])
        //                                },
        //                                ExtraServiceLightGoods = new XValueModel()
        //                                {
        //                                    Checked = dr["ES_LIGHT_GOODS"].ToString(),
        //                                    Value = Convert.ToInt32(dr["ES_LIGHT_GOODS_VALUE"])
        //                                },
        //                                ExtraServiceRange = dr["ES_RANGE"].ToString(),
        //                                ExtraServiceWeight = dr["ES_WEIGHT"].ToString(),
        //                                ExtraServiceDeclaredValue = new XValueModel()
        //                                {
        //                                    Checked = dr["ES_DECLARED_VALUE"].ToString(),
        //                                    Value = Convert.ToDouble(dr["ES_DECLARED_VALUE_VALUE"]),
        //                                    MinValue = Convert.ToInt32(dr["ES_DECLARED_VALUE_MIN_VALUE"]),
        //                                    MaxValue = Convert.ToInt32(dr["ES_DECLARED_VALUE_MAX_VALUE"])
        //                                },
        //                                ExtraServiceMainFee = new XValueModel()
        //                                {
        //                                    Checked = dr["ES_MAIN_FEE"].ToString(),
        //                                    Value = Convert.ToDouble(dr["ES_MAIN_FEE_VALUE"]),
        //                                    MinValue = Convert.ToInt32(dr["ES_MAIN_FEE_MIN_VALUE"]),
        //                                    MaxValue = Convert.ToInt32(dr["ES_MAIN_FEE_MAX_VALUE"])
        //                                },
        //                                ExtraServiceSurcharge = new XValueModel()
        //                                {
        //                                    Checked = dr["ES_SURCHARGE"].ToString(),
        //                                    Value = Convert.ToDouble(dr["ES_SURCHARGE_VALUE"]),
        //                                    MinValue = Convert.ToInt32(dr["ES_SURCHARGE_MIN_VALUE"]),
        //                                    MaxValue = Convert.ToInt32(dr["ES_SURCHARGE_MAX_VALUE"])
        //                                },
        //                                UpdateBy = Convert.ToInt32(dr["UPDATED_BY"]),
        //                                UpdateDate = dr["UPDATED_DATE"].ToString()
        //                            };
        //                        }
        //                    }
        //                    res.Code = "00";
        //                    res.Message = "Lấy dữ liệu thành công";
        //                    res.Data = JsonConvert.SerializeObject(model);
        //                }
        //                else
        //                {
        //                    res.Code = "80";
        //                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageSurchargeByID: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        [Route("PostageRangeSearch")]
        public ZResponse PostageRangeSearch(ZTransportPostageRangeSearchRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<ZTransportPostageRangeSearchResponse> model = new List<ZTransportPostageRangeSearchResponse>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_RANGE_SEARCH", new object[]
                {
                    req.PostageID,
                    req.GroupID,
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
                                    int ID = Convert.ToInt32(dr["ID"]);
                                    int FromRange = Convert.ToInt32(dr["FROM_RANGE"]);
                                    int ToRange = Convert.ToInt32(dr["TO_RANGE"]);
                                    string FromRangeName = "", ToRangeName = "";
                                    if (FromRange == 1)
                                    {
                                        FromRangeName = dr["FROM_REGION_NAME"].ToString();
                                    }
                                    else if (FromRange == 2)
                                    {
                                        FromRangeName = dr["FROM_PROVINCE_NAME"].ToString();
                                    }
                                    if (FromRange == 1)
                                    {
                                        ToRangeName = dr["TO_REGION_NAME"].ToString();
                                    }
                                    else if (FromRange == 2)
                                    {
                                        ToRangeName = dr["TO_PROVINCE_NAME"].ToString();
                                    }
                                    model.Add(new ZTransportPostageRangeSearchResponse()
                                    {
                                        ID = ID,
                                        GroupID = Convert.ToInt32(dr["GROUP_ID"]),
                                        PostageID = Convert.ToInt32(dr["POSTAGE_ID"]),
                                        FromRange = FromRange,
                                        FromRangeValue = Convert.ToInt32(dr["FROM_RANGE_VALUE"]),
                                        FromRangeValueName = FromRangeName,
                                        ToRange = ToRange,
                                        ToRangeValue = Convert.ToInt32(dr["TO_RANGE_VALUE"]),
                                        ToRangeValueName = ToRangeName,
                                        TwoWay = dr["TWO_WAY"].ToString(),
                                        Unit = Convert.ToInt32(dr["UNIT"]),
                                        Value = Convert.ToInt64(dr["VALUE"]),
                                        MinValue = Convert.ToInt64(dr["MIN_VALUE"]),
                                        MaxValue = Convert.ToInt64(dr["MAX_VALUE"])
                                    });
                                }
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(model);
                        }
                        else
                        {
                            res.Code = "80";
                            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageRangeSearch: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("PostageRangeAdd")]
        public ZResponse PostageRangeAdd(ZTransportPostageRangeAddRequest req)
        {
            int result = -1;
            int RangeID = 0;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_RANGE_ADD", new object[]
                {
                        0, //RangeID
                        req.GroupID,
                        req.PostageID,
                        req.FromRange,
                        req.FromRangeValue,
                        req.ToRange,
                        req.ToRangeValue,
                        req.TwoWay,
                        req.Unit,
                        req.Value,
                        req.MinValue,
                        req.MaxValue,
                        req.CreatedBy
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        RangeID = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (RangeID <= 0)
                        {
                            switch (RangeID)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Bảng cước không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Căn cứ tính cước này đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Chiều ngược về đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -99:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
                                    break;
                            }
                            return res;
                        }
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                        return res;
                    }
                }
                if (req.Weight != null && (req.GroupID == 3 || req.GroupID == 6))
                {
                    foreach (var itemW in req.Weight)
                    {

                        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_WEIGHT_ADD", new object[]
                        {
                            0,
                            req.GroupID,
                            req.PostageID,
                            RangeID, // ParentID
                            itemW.FromWeight,
                            itemW.ToWeight,
                            itemW.Unit,
                            itemW.Value,
                            itemW.MinValue,
                            itemW.MaxValue,
                            req.CreatedBy
                        }))
                        {
                            result = XHelper.ExecuteNonQuery(dbCommand);
                            if (result >= 0)
                            {
                                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                                if (result < 0)
                                {
                                    //TransportRepository.RefuseWeight(req.GroupID, req.PostageID, RangeID);
                                    switch (result)
                                    {
                                        case -1:
                                            res.Code = "01";
                                            res.Message = "Bảng cước không tồn tại.";
                                            break;
                                        case -99:
                                            res.Code = "99";
                                            res.Message = "Thất bại.";
                                            break;
                                    }
                                    return res;
                                }
                            }
                            else
                            {
                                TransportRepository.RefuseWeight(req.GroupID, req.PostageID, RangeID);
                                res.Code = "99";
                                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                                return res;
                            }
                        }
                    }
                }
                res = TransportRepository.AcceptRange(RangeID, req.GroupID);
            }
            catch (Exception ex)
            {
                TransportRepository.RefuseWeight(req.GroupID, req.PostageID, RangeID);
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.PostageRangeAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        [Route("PostageWeightSearch")]
        public ZResponse PostageWeightSearch(ZTransportPostageWeightSearchRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<XTransportPostageWeightModel> model = new List<XTransportPostageWeightModel>();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_WEIGHT_SEARCH", new object[]
                {
                    req.PostageID,
                    req.GroupID,
                    req.ParentID,
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
                                    model.Add(new XTransportPostageWeightModel()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        GroupID = Convert.ToInt32(dr["GROUP_ID"]),
                                        PostageID = Convert.ToInt32(dr["POSTAGE_ID"]),
                                        ParentID = Convert.ToInt32(dr["PARENT_ID"]),
                                        FromWeight = Convert.ToInt32(dr["FROM_WEIGHT"]),
                                        ToWeight = Convert.ToInt32(dr["TO_WEIGHT"]),
                                        Unit = Convert.ToInt32(dr["UNIT"]),
                                        Value = Convert.ToInt64(dr["VALUE"]),
                                        MaxValue = Convert.ToInt64(dr["MIN_VALUE"]),
                                        MinValue = Convert.ToInt64(dr["MAX_VALUE"])
                                    });
                                    if (req.GroupID == 3)
                                    {

                                    }
                                }
                            }
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(model);
                        }
                        else
                        {
                            res.Code = "80";
                            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageWeightSearch: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("PostageWeightAdd")]
        public ZResponse PostageWeightAdd(List<ZTransportPostageWeightAddRequest> req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                foreach (var item in req)
                {

                    using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_WEIGHT_ADD", new object[]
                    {
                            0,
                            item.GroupID,
                            item.PostageID,
                            0,
                            item.FromWeight,
                            item.ToWeight,
                            item.Unit,
                            item.Value,
                            item.MaxValue,
                            item.MinValue,
                            item.CreatedBy
                    }))
                    {
                        result = XHelper.ExecuteNonQuery(dbCommand);
                        if (result >= 0)
                        {
                            result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                            if (result < 0)
                            {
                                //TransportRepository.RefuseWeight(req[0].GroupID, req[0].PostageID, 0);
                                switch (result)
                                {
                                    case -1:
                                        res.Code = "01";
                                        res.Message = "Bảng cước không tồn tại.";
                                        break;
                                    case -2:
                                        res.Code = "02";
                                        res.Message = "Đã tồn tại căn cứ tính cước theo khối lượng. Vui lòng kiểm tra lại.";
                                        break;
                                    case -99:
                                        res.Code = "99";
                                        res.Message = "Thất bại.";
                                        break;
                                }
                                return res;
                            }
                        }
                        else
                        {
                            TransportRepository.RefuseWeight(req[0].GroupID, req[0].PostageID, 0);
                            res.Code = "99";
                            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                            return res;
                        }
                    }
                }
                res = TransportRepository.AcceptWeight(req[0].GroupID, req[0].PostageID, 0);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.PostageWeightAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";

                TransportRepository.RefuseWeight(req[0].GroupID, req[0].PostageID, 0);

                return res;
            }
            return res;
        }
        
        
        

        
        #endregion

        #region Postage extra service
        //[Route("PostageExtraServiceAdds")]
        //public ZResponse PostageExtraServiceAdds(ZTransportPostageExtraServiceAddsRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    int countSuccess = 0;
        //    string MessageErr = "";
        //    try
        //    {
        //        foreach (int ExtraServiceID in req.ExtraServiceIDs)
        //        {
        //            var resAdd = PostageExtraServiceAdd(new ZTransportPostageExtraServiceAddRequest()
        //            {
        //                ExtraServiceID = ExtraServiceID,
        //                VAT = req.VAT,
        //                StartDate = req.StartDate,
        //                EndDate = req.EndDate,
        //                ChargingConditionsID = req.ChargingConditionsID,
        //                ChargingConditionsValue = req.ChargingConditionsValue,
        //                ChargingConditionsMinValue = req.ChargingConditionsMinValue,
        //                ServicesUsed = req.ServicesUsed,
        //                CreatedBy = req.CreatedBy
        //            });
        //            if (resAdd.Code == "00")
        //            {
        //                countSuccess++;
        //            }
        //            else
        //            {
        //                MessageErr = MessageErr + resAdd.Message;
        //            }
        //        }
        //        if (countSuccess == req.ExtraServiceIDs.Count)
        //        {
        //            res.Code = "00";
        //            res.Message = "Thành công " + countSuccess + "/" + req.ExtraServiceIDs.Count;
        //        }
        //        else
        //        {
        //            res.Code = "01";
        //            res.Message = "Thành công " + countSuccess + "/" + req.ExtraServiceIDs.Count + "\nChi tiết thất bại:\n" + MessageErr;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageExtraServiceAdds: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("PostageExtraServiceAdd")]
        //public ZResponse PostageExtraServiceAdd(ZTransportPostageExtraServiceAddRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_EXTRA_ADD", new object[]
        //        {
        //           0,
        //           req.ExtraServiceID,
        //           req.VAT,
        //           req.StartDate,
        //           req.EndDate,
        //           req.ChargingConditionsID,
        //           req.ChargingConditionsValue,
        //           req.ChargingConditionsMinValue,
        //           req.ServicesUsed,
        //           req.CreatedBy,
        //           0
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                }
        //                else
        //                {
        //                    var ExtraServiceName = Convert.ToString(dbCommand.Parameters["P_EXTRA_SERVICE_NAME"].Value);
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = ExtraServiceName + ": Đã có bảng cước.\n";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = ExtraServiceName + ": Thất bại.\n";
        //                            break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.\n";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageExtraServiceAdd: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.\n";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("PostageExtraServiceEdit")]
        //public ZResponse PostageExtraServiceEdit(ZTransportPostageExtraServiceEditRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_EXTRA_EDIT", new object[]
        //        {
        //           0,
        //           req.PostageID,
        //           req.VAT,
        //           req.StartDate,
        //           req.EndDate,
        //           req.ChargingConditionsID,
        //           req.ChargingConditionsValue,
        //           req.ChargingConditionsMinValue,
        //           req.ServicesUsed,
        //           req.UpdatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Bảng cước không tồn tại.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Dịch vụ đã tồn tại.";
        //                            break;
        //                        default:
        //                            res.Code = "99";
        //                            res.Message = "Thất bại.";
        //                            break;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "99";
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageExtraServiceEdit: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        [Route("PostageExtraServiceByID")]
        public ZResponse PostageExtraServiceByID(ZTransportPostageExtraServiceByIDRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            ZTransportPostageExtraServiceByIDResponse model = new ZTransportPostageExtraServiceByIDResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_EXTRA_BY_ID", new object[]
                {
                    req.PostageID,
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
                                    model = new ZTransportPostageExtraServiceByIDResponse()
                                    {
                                        PostageID = Convert.ToInt32(dr["ID"]),
                                        ServiceID = Convert.ToInt32(dr["SERVICE_ID"]),
                                        ServiceName = dr["SERVICE_NAME"].ToString(),
                                        StartDate = dr["START_DATE"].ToString(),
                                        EndDate = dr["END_DATE"].ToString(),
                                        VAT = dr["VAT"].ToString(),
                                        ChargingConditionsID = Convert.ToInt32(dr["CHARGING_CONDITIONS_ID"]),
                                        ChargingConditionsValue = Convert.ToDouble(dr["CHARGING_CONDITIONS_VALUE"]),
                                        ChargingConditionsMinValue = Convert.ToInt32(dr["CHARGING_CONDITIONS_MIN_VALUE"]),
                                        ServicesUsed = dr["SERVICES_USED"].ToString()
                                    };
                                }
                            }

                            if (model.PostageID > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageExtraServiceByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region PostageDetail
        
        
        
        #endregion

        #region charged
        [Route("Charged")]
        public ZResponse Charged(ZTransportPostageChargedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportRepository.Charged(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.Charged: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ChargedV2")]
        public ZResponse ChargedV2(ZTransportPostageChargedV2Request req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZTransportPostageChargedV2Response model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.CHARGED_V2", new object[]
                {
                    req.MainServiceCode,
                    req.ExtraServiceCodes,
                    req.FromProvinceID,
                    req.ToProvinceID,
                    req.ToDistrictID,
                    req.ToWardID,
                    req.Weight,
                    req.CustomerCode,
                    req.Quantity,
                    req.Amount,
                    req.COD,
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0,                   //P_MAIN_FEE OUT NUMBER
                    0,                   //P_EXTRA_FEE OUT NUMBER
                    0,                   //P_SURCHARGE OUT NUMBER
                    0,
                    0,
                    0,
                    0,
                    0                    //p_extra_service_out
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
                        if (result > 0)
                        {
                            model = new ZTransportPostageChargedV2Response()
                            {
                                MainFee = Convert.ToInt64(dbCommand.Parameters["P_MAIN_FEE"].Value),
                                ExtraFee = Convert.ToInt64(dbCommand.Parameters["P_EXTRA_FEE"].Value),
                                Surcharge = Convert.ToInt64(dbCommand.Parameters["P_SURCHARGE"].Value),
                                VATOfMainFee = Convert.ToInt64(dbCommand.Parameters["P_VAT_OF_MAIN_FEE"].Value),
                                VATOfExtraFee = Convert.ToInt64(dbCommand.Parameters["P_VAT_OF_EXTRA_FEE"].Value),
                                VATOfSurcharge = Convert.ToInt64(dbCommand.Parameters["P_VAT_OF_SURCHARGE"].Value),
                                TotalAmount = Convert.ToInt64(dbCommand.Parameters["P_TOTAL_AMOUNT"].Value)
                            };

                            if(model != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "98";
                                res.Message = "Dữ liệu không tồn tại.";
                            }
                        }
                        else
                        {
                            var ExtraServiceCode = dbCommand.Parameters["P_EXTRA_SERVICE_OUT"].Value.ToString();
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Chưa có gói cước của dịch vụ chính này.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Chưa khai báo phạm vi tính cước chính.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Chưa cấu hình cước dịnh vụ chính.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Lỗi lấy cước dịch vụ cộng thêm.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Lỗi lấy phụ phí.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Gói cước đang không trong thời gian sử dụng. Vui lòng kiểm tra lại.";
                                    break;
                                case -9:
                                    res.Code = "09";
                                    res.Message = "Gói cước đang khóa. Vui lòng kiểm tra lại.";
                                    break;
                                case -10:
                                    res.Code = "10";
                                    res.Message = "Chưa khai báo vùng, khu vực của dịch vụ chính. Vui lòng kiểm tra lại.";
                                    break;
                                case -11:
                                    res.Code = "11";
                                    res.Message = "Chưa xác định nơi đi. Vui lòng kiểm tra lại.";
                                    break;
                                case -12:
                                    res.Code = "12";
                                    res.Message = "Chưa xác định nơi đến. Vui lòng kiểm tra lại.";
                                    break;
                                case -13:
                                    res.Code = "13";
                                    res.Message = "Khối lượng phải > 0. Vui lòng kiểm tra lại.";
                                    break;
                                case -14:
                                    res.Code = "14";
                                    res.Message = "Giá trị đơn hàng phải > 0. Vui lòng kiểm tra lại.";
                                    break;
                                case -15:
                                    res.Code = "15";
                                    res.Message = "Tiền COD phải > 0. Vui lòng kiểm tra lại.";
                                    break;
                                case -16:
                                    res.Code = "16";
                                    res.Message = "Nơi xuất phát này chưa được cấu hình cước. Vui lòng kiểm tra lại.";
                                    break;
                                case -17:
                                    res.Code = "17";
                                    res.Message = "Gói cước chỉ áp dụng cho nội tỉnh. Vui lòng kiểm tra lại.";
                                    break;
                                case -18:
                                    res.Code = "18";
                                    res.Message = "Gói cước chỉ áp dụng cho liên tỉnh. Vui lòng kiểm tra lại.";
                                    break;
                                case -19:
                                    res.Code = "19";
                                    res.Message = "Gói cước chỉ áp dụng cho nội vùng. Vui lòng kiểm tra lại.";
                                    break;
                                case -20:
                                    res.Code = "20";
                                    res.Message = "Gói cước chỉ áp dụng cho liên vùng. Vui lòng kiểm tra lại.";
                                    break;
                                case -21:
                                    res.Code = "21";
                                    res.Message = "Dịch vụ chính CPN TMĐT Nhanh tiết kiệm chỉ áp dụng cho liên tỉnh - liên vùng. Vui lòng kiểm tra lại.";
                                    break;
                                case -22:
                                    res.Code = "22";
                                    res.Message = "Dịch vụ EMS Thỏa thuận không áp dụng cho nội tỉnh. Vui lòng kiểm tra lại.";
                                    break;
                                case -70:
                                    res.Code = "70";
                                    res.Message = "Dịch vụ chính không được để trống.";
                                    break;
                                case -71:
                                    res.Code = "71";
                                    res.Message = "Dịch vụ chính không tồn tại.";
                                    break;
                                case -72:
                                    res.Code = "72";
                                    res.Message = "Dịch vụ chính đang khóa.";
                                    break;
                                case -73:
                                    res.Code = "73";
                                    res.Message = "Dịch vụ " + ExtraServiceCode + " không tồn tại.";
                                    break;
                                case -74:
                                    res.Code = "74";
                                    res.Message = "Dịch vụ " + ExtraServiceCode + " đang khóa.";
                                    break;
                                default:
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.ChargedV2: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        #endregion

        #region TransportFE
        //[Route("List")]
        //public ZResponse List(ZTransportSearchListRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    List<ZTransportSearchListResponse> list = new List<ZTransportSearchListResponse>();
        //    try
        //    {
        //        if (req.FromDate == "")
        //        {
        //            req.FromDate = DateTime.Now.ToString("01/MM/yyyy");
        //        }
        //        if (req.ToDate == "")
        //        {
        //            req.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
        //        }
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHIPPING_MANAGEMENT_PKG.LIST_ORDER_TRANSPORT", new object[]
        //        {
        //            req.FromDate,
        //            req.ToDate,
        //            req.ShopID,      //P_SHOP_ID NUMBER            
        //            req.MobileNumber, //P_MOBILE_NUMBER
        //            req.OrderCode,   //P_ORDER_CODE
        //            req.EmsBarCode,  //P_EMSBARCODE
        //            req.Status,      //P_STATUS
        //            0,               //P_RETURN_CODE OUT NUMBER
        //            0                //P_OUT_CURSOR  OUT T_CURSOR
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
        //                            list.Add(new ZTransportSearchListResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                Code = dr["CODE"].ToString(),
        //                                CreatedDate = dr["CREATED_DATE"].ToString(),
        //                                RequestSubmissionDate = dr["REQUEST_SUBMISSION_DATE"].ToString(),
        //                                CustomerCode = dr["CUSTOMER_CODE"].ToString(),
        //                                FullName = dr["FULL_NAME"].ToString(),
        //                                MobileNumber = dr["MOBILE_NUMBER"].ToString(),
        //                                TotalAmount = Convert.ToInt64(dr["TOTAL_AMOUNT"]),
        //                                TotalDiscount = Convert.ToInt64(dr["TOTAL_DISCOUNT"]),
        //                                TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
        //                                TotalOrderValue = Convert.ToInt64(dr["TOTAL_ORDER_VALUE"]),
        //                                Channel = dr["CHANNEL"].ToString(),
        //                                TotalProductValue = Convert.ToInt64(dr["TOTAL_PRODUCT_VALUE"]),
        //                                PaymentStatus = dr["PAYMENT_STATUS"].ToString(),
        //                                OrderStatus = dr["ORDER_STATUS"].ToString(),
        //                                Transport = dr["TRANSPORT"].ToString(),
        //                                TransportStatus = dr["TRANSPORT_STATUS"].ToString(),
        //                                IsDeliveryNote = dr["IS_DELIVERY_NOTE"].ToString(),
        //                                PickupProvinceID = Convert.ToInt32(dr["PICK_UP_PROVINCE_ID"]),
        //                                EmsBarCode = dr["EMSBARCODE"].ToString(),
        //                                ActualCharges = Convert.ToInt64(dr["ACTUAL_CHARGES"]),
        //                                TotalMoney = Convert.ToInt64(dr["TOTAL_MONEY"])
        //                            });
        //                        }
        //                    }

        //                    if (list != null)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(list);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu. Vui lòng kiểm tra lại.";
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
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.List: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        
        //[HttpPost]
        //[Route("GetInfoShop")]
        //public ZResponse GetInfoShop(ZTransportInfoShippingRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    ZTransportInfoShippingResponse infoShop = new ZTransportInfoShippingResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHIPPING_MANAGEMENT_PKG.GET_INFO_SHOP", new object[]
        //        {
        //            req.ShopID,      //P_SHOP_ID NUMBER
        //            0,               //P_RETURN_CODE OUT NUMBER
        //            0                //P_OUT_CURSOR  OUT T_CURSOR
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
        //                        if (dr.Read())
        //                        {
        //                            infoShop = new ZTransportInfoShippingResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                CustomerCode = dr["CUSTOMER_CODE"].ToString(),
        //                                CustomerPhone = dr["CUSTOMER_PHONE"].ToString(),
        //                                CustomerName = dr["CUSTOMER_NAME"].ToString(),
        //                                CustomerEmail = dr["CUSTOMER_EMAIL"].ToString(),
        //                                CustomerProvinceID = Convert.ToInt32(dr["CUSTOMER_PROVINCE_ID"]),
        //                                CustomerDistrictID = Convert.ToInt32(dr["CUSTOMER_DISTRICT_ID"]),
        //                                CustomerWardID = Convert.ToInt32(dr["CUSTOMER_WARD_ID"]),
        //                                CustomerAdress = dr["CUSTOMER_ADDRESS"].ToString(),
        //                                PickupProvinceID = Convert.ToInt32(dr["PICK_UP_PROVINCE_ID"]),
        //                                PickupDistrictID = Convert.ToInt32(dr["PICK_UP_DISTRICT_ID"]),
        //                                PickupWardID = Convert.ToInt32(dr["PICK_UP_WARD_ID"]),
        //                                PickupStreet = dr["PICKUP_STREET"].ToString(),
        //                                ContactName = dr["CONTACT_NAME"].ToString(),
        //                                ContactPhone = dr["CONTACT_PHONE"].ToString()
        //                            };
        //                        }
        //                    }

        //                    if (infoShop != null)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(infoShop);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
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
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.GetInfoShop: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        
        //[HttpPost]
        //[Route("GetInfoShopOrderDetail")]
        //public ZResponse GetInfoShopOrderDetail(ZTransportSearchInfoShopOrderDetailRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    ZTransportSearchInfoShopOrderDetailResponse item = null;
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHIPPING_MANAGEMENT_PKG.GET_INFO_SHOP_ORDER_DETAIL", new object[]
        //        {
        //            req.OrderCode,
        //            req.ShopID,      //P_SHOP_ID NUMBER
        //            0,               //P_RETURN_CODE OUT NUMBER
        //            0                //P_OUT_CURSOR  OUT T_CURSOR
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
        //                            item = new ZTransportSearchInfoShopOrderDetailResponse()
        //                            {
        //                                //SenderName = dr["SENDER_NAME"].ToString(),
        //                                //SenderCode = dr["SENDER_CODE"].ToString(),
        //                                //SenderPhone = dr["SENDER_PHONE"].ToString(),
        //                                //SenderEmail = dr["SENDER_EMAIL"].ToString(),
        //                                //SenderProvinceID = Convert.ToInt32(dr["SENDER_PROVINCE_ID"]),
        //                                //SenderDistrictID = Convert.ToInt32(dr["SENDER_DISTRICT_ID"]),
        //                                //SenderWardID = Convert.ToInt32(dr["SENDER_WARD_ID"]),
        //                                //SenderAddress = dr["SENDER_ADDRESS"].ToString(),
        //                                ReceiverName = dr["RECEIVER_NAME"].ToString(),
        //                                ReceiverProvinceID = Convert.ToInt32(dr["RECEIVER_PROVINCE_ID"]),
        //                                ReceiverDistrictID = Convert.ToInt32(dr["RECEIVER_DISTRICT_ID"]),
        //                                ReceiverWardID = Convert.ToInt32(dr["RECEIVER_WARD_ID"]),
        //                                ReceiverAddress = dr["RECEIVER_ADDRESS"].ToString(),
        //                                ReceiverPhone = dr["RECEIVER_PHONE"].ToString(),
        //                                ProductName = dr["PRODUCT_NAME"].ToString(),
        //                                ProductValue = Convert.ToInt32(dr["PRICE"]),
        //                                ProductCode = dr["PRODUCT_CODE"].ToString(),
        //                                //ProductDescription = dr["PRODUCT_DESCRIPTION"].ToString(),
        //                                ProductQuantity = Convert.ToInt32(dr["PRODUCT_QUANTITY"]),
        //                                Weight = Convert.ToInt32(dr["WEIGHT"]),
        //                                TotalAmount = Convert.ToInt64(dr["TOTAL_AMOUNT"]),
        //                                AdditionServices = dr["ADDITION_SERVICE"].ToString(),
        //                                EspeciallyServices = dr["ESPECIALLY_SERVICE"].ToString(),
        //                                TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
        //                            };
        //                        }
        //                    }
        //                    if (item != null)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(item);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
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
        //                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.GetInfoShopOrderDetail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        
        //[HttpPost]
        //[Route("UpdateStatusTransportOrder")]
        //public ZResponse UpdateStatusTransportOrder(ZTransportUpdateOrderRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = OrderRepository.SendTransportStatus(new ZOrderSendTransportStatusRequest() { EMSOneCode = req.OrderCode, Status = req.Status, UpdatedBy = req.UpdatedBy });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.UpdateStatusTransportOrder: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[HttpPost]
        //[Route("GetEmsBarCode")]
        //public ZResponse GetEmsBarCode(ZTransportGetEmsBarCodeRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = TransportRepository.GetEmsBarCode(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.GetEmsBarCode: " + ex.Message);
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[HttpPost]
        //[Route("GetEmsBarCodeCancel")]
        //public ZResponse GetEmsBarCodeCancel(ZTransportSearchEmsBarCodeCancelRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = TransportRepository.GetEmsBarCodeCancel(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.GetEmsBarCodeCancel: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        #endregion
    }
}