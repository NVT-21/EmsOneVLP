using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EMS.API.Core.Repositories
{
    public class TransportRepository
    {
        public static ZResponse TransportPostageRangeAdd(List<XTransportPostageRangeModel> req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                foreach(var item in req)
                {
                    int RangeID = 0;
                    using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_RANGE_ADD", new object[]
                        {
                            0, //RangeID
                            item.GroupID,
                            item.PostageID,
                            item.FromRange,
                            item.FromRangeValue,
                            item.ToRange,
                            item.ToRangeValue,
                            item.TwoWay,
                            item.Unit,
                            item.Value,
                            item.MinValue,
                            item.MaxValue
                        }))
                    {
                        result = XHelper.ExecuteNonQuery(dbCommand);
                        if(result >= 0)
                        {
                            RangeID = Convert.ToInt32(dbCommand.Parameters[0].Value);
                            if(RangeID <= 0)
                            {
                                res.Code = "99";
                                res.Message = "Thất bại.";
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

                    foreach(var itemW in item.TransportPostageWeight)
                    {

                        using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_WEIGHT_ADD", new object[]
                        {
                            0,
                            item.GroupID,
                            item.PostageID,
                            RangeID, // ParentID
                            itemW.FromWeight,
                            itemW.ToWeight,
                            itemW.Unit,
                            itemW.Value,
                            itemW.MinValue,
                            itemW.MaxValue
                        }))
                        {
                            result = XHelper.ExecuteNonQuery(dbCommand);
                            if(result >= 0)
                            {
                                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                                if(result < 0)
                                {
                                    res.Code = "99";
                                    res.Message = "Thất bại.";
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
                    }
                }
                res.Code = "00";
                res.Message = "Thành công.";
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.TransportPostageRangeAdd: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse TransportPostageWeightAdd(List<XTransportPostageWeightModel> req)
        {
            int result = -1;
            ZResponse res = new ZResponse();

            foreach(var item in req)
            {
                try
                {
                    using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_WEIGHT_ADD", new object[]
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
                            item.MinValue
                    }))
                    {
                        result = XHelper.ExecuteNonQuery(dbCommand);
                        if(result >= 0)
                        {
                            result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                            if(result < 0)
                            {
                                res.Code = "99";
                                res.Message = "Thất bại.";
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
                }
                catch(Exception ex)
                {
                    XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.PostageRangeWeightAddRange: " + ex.Message);
                    res.Code = "99";
                    res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                    return res;
                }
            }
            res.Code = "00";
            res.Message = "Thành công.";
            return res;
        }

        public static List<XTransportPostageRangeModel> PostageRangeSearch(int PostageID, int GroupID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<XTransportPostageRangeModel> model = new List<XTransportPostageRangeModel>();
            try
            {
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_RANGE_SEARCH", new object[]
                {
                    PostageID,
                    GroupID,
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0                    //P_OUT_CURSOR  OUT T_CURSOR
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
                                    int ID = Convert.ToInt32(dr["ID"]);
                                    var PostageWeightSearch = new List<XTransportPostageWeightModel>();
                                    if(GroupID == 3 || GroupID == 6)
                                    {
                                        PostageWeightSearch = TransportRepository.PostageWeightSearch(PostageID, GroupID, ID);
                                    }
                                    model.Add(new XTransportPostageRangeModel()
                                    {
                                        ID = ID,
                                        GroupID = Convert.ToInt32(dr["GROUP_ID"]),
                                        PostageID = Convert.ToInt32(dr["POSTAGE_ID"]),
                                        FromRange = Convert.ToInt32(dr["FROM_RANGE"]),
                                        FromRangeValue = Convert.ToInt32(dr["FROM_RANGE_VALUE"]),
                                        ToRange = Convert.ToInt32(dr["TO_RANGE"]),
                                        ToRangeValue = Convert.ToInt32(dr["TO_RANGE_VALUE"]),
                                        TwoWay = dr["TWO_WAY"].ToString(),
                                        Unit = Convert.ToInt32(dr["UNIT"]),
                                        Value = Convert.ToInt64(dr["VALUE"]),
                                        MaxValue = Convert.ToInt64(dr["MIN_VALUE"]),
                                        MinValue = Convert.ToInt64(dr["MAX_VALUE"]),
                                        TransportPostageWeight = PostageWeightSearch
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageMainFeeByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return model;
        }

        public static List<XTransportPostageWeightModel> PostageWeightSearch(int PostageID, int GroupID, int ParentID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            List<XTransportPostageWeightModel> model = new List<XTransportPostageWeightModel>();
            try
            {
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_WEIGHT_SEARCH", new object[]
                {
                    PostageID,
                    GroupID,
                    ParentID,
                    0,                   //P_RETURN_CODE OUT NUMBER
                    0                    //P_OUT_CURSOR  OUT T_CURSOR
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
                                    if(GroupID == 3)
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportController.PostageWeightSearch: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return model;
        }

        public static ZResponse AcceptRange(int RangeID, int GroupID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {

                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_RANGE_ACCEPT", new object[]
                {
                    0,
                    RangeID,
                    GroupID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if(result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Duyệt căn cứ tính cước phạm vi thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.AcceptRange: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse AcceptWeight(int GroupID, int PostageID, int RangeID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {

                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_WEIGHT_ACCEPT", new object[]
                {
                    0,
                    GroupID,
                    PostageID,
                    RangeID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if(result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Duyệt căn cứ tính cước khối lượng thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.AcceptWeight: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse RefuseWeight(int GroupID, int PostageID, int RangeID)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {

                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.POSTAGE_WEIGHT_REFUSE", new object[]
                {
                    0,
                    GroupID,
                    PostageID,
                    RangeID
                }))
                {
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if(result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Thành công";
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Từ chối căn cứ tính cước khối lượng thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.AcceptWeight: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse GetEmsBarCode(ZTransportGetEmsBarCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using(DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHIPPING_MANAGEMENT_PKG.GET_EMS_BARCODE", new object[]
                {
                    0,
                    req.CustomerCode,
                    req.ProvinceID,
                    req.OrderCode,
                    req.CreatedBy,
                    req.AmndState,
                    req.IsConsignment,
                    req.ShippingID,
                    req.IsCommit,
                    0
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if(result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if(result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Lấy mã bưu gửi thành công.";
                            res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_EMS_BARCODE"].Value);
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.GetEmsBarCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //public static ZResponse GetEmsBarCodeCancel(ZTransportSearchEmsBarCodeCancelRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    ZTransportSearchEmsBarCodeCancelResponse Item = null;
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHIPPING_MANAGEMENT_PKG.GET_EMS_BARCODE_CANCEL", new object[]
        //        {
        //            req.EMSOneCode,
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
        //                            Item = new ZTransportSearchEmsBarCodeCancelResponse()
        //                            {
        //                                ID = Convert.ToInt32(dr["ID"]),
        //                                Code = dr["CODE"].ToString(),
        //                                CustomerCode = dr["CUSTOMER_CODE"].ToString()
        //                            };
        //                        }
        //                    }
        //                    if (Item != null)
        //                    {
        //                        res.Code = "00";
        //                        res.Message = "Lấy dữ liệu thành công";
        //                        res.Data = JsonConvert.SerializeObject(Item);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Không tồn tại dữ liệu.";
        //                    }
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Không tồn tại mã bưu gửi.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Sai trạng thái đơn hàng.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.GetEmsBarCodeCancel: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}


        #region Tính cước cũ
        //public static ZResponse Charged(ZTransportPostageChargedRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        ZTransportPostageChargedResponse model = null;

        //        req.Weight = XCommon.ExchangeTransportWeight(new ZTransportExchangeTransportWeightRequest()
        //        {
        //            TransportMainServiceID = req.MainServiceID,
        //            TransportSizeLength = req.TransportSizeLength,
        //            TransportSizeWidth = req.TransportSizeWidth,
        //            TransportSizeHeight = req.TransportSizeHeight,
        //            TransportWeight = req.Weight
        //        });

        //        //if (req.TransportSizeLength > 0 && req.TransportSizeWidth > 0 && req.TransportSizeHeight > 0)
        //        //{
        //        //    var convertTransportWeight = Math.Round((req.TransportSizeLength * req.TransportSizeWidth * req.TransportSizeHeight) / 6);
        //        //    //TMD - TMD_B
        //        //    if (req.MainServiceID == 141 || req.MainServiceID == 360)
        //        //    {
        //        //        List<double> ListLWH = new List<double>() { req.TransportSizeLength, req.TransportSizeWidth, req.TransportSizeHeight };
        //        //        var getMax = ListLWH.Max();
        //        //        if(getMax > 50 && convertTransportWeight > req.Weight)
        //        //        {
        //        //            req.Weight = convertTransportWeight;
        //        //        }
        //        //        else
        //        //        {
        //        //            convertTransportWeight = 0;
        //        //        }
        //        //    }

        //        //    if (convertTransportWeight > req.Weight)
        //        //    {
        //        //        req.Weight = convertTransportWeight;
        //        //    }
        //        //}

        //        var mainService = TransportServiceRepository.SearchById(req.MainServiceID.ToString());
        //        var extraServices = TransportServiceRepository.SearchById(req.ExtraServiceIDs ?? "".ToString());
        //        SqlConnection connection = new SqlConnection();
        //        using(connection = XHelper.SqlConnectionFee)
        //        {
        //            using(DbCommand dbCommand = XHelper.DatabaseFee.GetStoredProcCommand(XHelper.SchemaFee + "Cuoc_Chinh_Tinh_Cuoc_Dich_Vu_Chinh_API_Merchanrt"))
        //            {


        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Dich_Vu_Chinh", DbType.String, mainService);
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Dich_Vu_GTGT", DbType.String, extraServices ?? "");
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Tinh_Nhan", DbType.Int32, Convert.ToInt32(req.FromProvinceID.ToString() + "0000"));
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Tinh_Tra", DbType.Int32, Convert.ToInt32(req.ToProvinceID.ToString() + "0000"));
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Huyen_Tra", DbType.String, req.ToDistrictID.ToString());
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Xa_Tra", DbType.Int32, req.FromWardID);
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Khoi_Luong", DbType.Int32, Convert.ToInt32(req.Weight));
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Khoi_Luong_Quy_Doi", DbType.Int32, Convert.ToInt32(req.Weight));
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_KH", DbType.String, req.CustomerCode ?? "");
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@So_Luong", DbType.Int32, req.Quantity);
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Gia_Tri_Hang", DbType.Int32, Convert.ToInt32(req.Amount));
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@So_Tien", DbType.Int32, Convert.ToInt32(req.COD));
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Nuoc_Tra", DbType.String, "VN");
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Phu_Phi", DbType.String, "");
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Phan_Loai_BP", DbType.String, "M");
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Dai", DbType.Int32, 0);
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Rong", DbType.Int32, 0);
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@Cao", DbType.Int32, 0);
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "@CityCode", DbType.String, "");
        //                XHelper.DatabaseFee.AddInParameter(dbCommand, "PostalCode", DbType.String, "");


        //                using(IDataReader reader = XHelper.SqlExecuteReader(dbCommand, connection))
        //                {
        //                    if(reader.Read())
        //                    {
        //                        model = new ZTransportPostageChargedResponse()
        //                        {

        //                            MainFee = Convert.ToInt64(reader["Cuoc_Chinh_VAT"]),
        //                            ExtraFee = Convert.ToInt64(reader["Tong_Cuoc_DVCT_VAT"]),
        //                            Surcharge = Convert.ToInt64(reader["Cuoc_PP_VAT"]),
        //                            MainFeeNotVAT = Convert.ToInt64(reader["Cuoc_Chinh"]),
        //                            ExtraFeeNotVAT = Convert.ToInt64(reader["Cuoc_PP"]),
        //                            SurchargeNotVAT = Convert.ToInt64(reader["Tong_Cuoc_DVCT"])
        //                        };

        //                        model.TotalAmount = model.MainFee + model.ExtraFee + model.Surcharge;
        //                        model.TotalAmountNotVAT = model.MainFeeNotVAT + model.ExtraFeeNotVAT + model.SurchargeNotVAT;

        //                        res.Code = "00";
        //                        res.Message = "Thành công.";
        //                        res.Data = JsonConvert.SerializeObject(model);
        //                    }
        //                    else
        //                    {
        //                        res.Code = "01";
        //                        res.Message = "Dữ liệu không tồn tại.";
        //                    }
        //                }
        //            }
        //        }


        //        #region TÍNH CƯỚC ONE
        //        //using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_TRANSPORT_PKG.CHARGED", new object[]
        //        //{
        //        //    req.MainServiceID,
        //        //    req.ExtraServiceIDs,
        //        //    req.FromProvinceID,
        //        //    req.FromDistrictID,
        //        //    req.FromWardID,
        //        //    req.ToProvinceID,
        //        //    req.ToDistrictID,
        //        //    req.ToWardID,
        //        //    req.Weight,
        //        //    req.CustomerCode,
        //        //    req.Quantity,
        //        //    req.Amount,
        //        //    req.COD,
        //        //    0,                      //P_RETURN_CODE         OUT NUMBER
        //        //    0,                      //P_MAIN_FEE            OUT NUMBER
        //        //    0,                      //P_EXTRA_FEE           OUT NUMBER
        //        //    0,                      //P_SURCHARGE           OUT NUMBER
        //        //    0,                      //P_MAIN_FEE_NOT_VAT    OUT NUMBER
        //        //    0,                      //P_EXTRA_FEE_NOT_VAT   OUT NUMBER
        //        //    0                       //P_SURCHARGE_NOT_VAT   OUT NUMBER
        //        //}))
        //        //{
        //        //    result = XHelper.ExecuteNonQuery(dbCommand);
        //        //    if (result >= 0)
        //        //    {
        //        //        result = Convert.ToInt32(dbCommand.Parameters["P_RETURN_CODE"].Value);
        //        //        if (result > 0)
        //        //        {
        //        //            model = new ZTransportPostageChargedResponse()
        //        //            {
        //        //                MainFee = Convert.ToInt64(dbCommand.Parameters["P_MAIN_FEE"].Value),
        //        //                ExtraFee = Convert.ToInt64(dbCommand.Parameters["P_EXTRA_FEE"].Value),
        //        //                Surcharge = Convert.ToInt64(dbCommand.Parameters["P_SURCHARGE"].Value),

        //        //                MainFeeNotVAT = Convert.ToInt64(dbCommand.Parameters["P_MAIN_FEE_NOT_VAT"].Value),
        //        //                ExtraFeeNotVAT = Convert.ToInt64(dbCommand.Parameters["P_EXTRA_FEE_NOT_VAT"].Value),
        //        //                SurchargeNotVAT = Convert.ToInt64(dbCommand.Parameters["P_SURCHARGE_NOT_VAT"].Value),
        //        //            };
        //        //            if (model != null)
        //        //            {
        //        //                model.TotalAmount = model.MainFee + model.ExtraFee + model.Surcharge;
        //        //                model.TotalAmountNotVAT = model.MainFeeNotVAT + model.ExtraFeeNotVAT + model.SurchargeNotVAT;

        //        //                res.Code = "00";
        //        //                res.Message = "Thành công.";
        //        //                res.Data = JsonConvert.SerializeObject(model);
        //        //            }
        //        //            else
        //        //            {
        //        //                res.Code = "01";
        //        //                res.Message = "Dữ liệu không tồn tại.";
        //        //            }
        //        //        }
        //        //        else
        //        //        {
        //        //            switch (result)
        //        //            {
        //        //                case -1:
        //        //                    res.Code = "01";
        //        //                    res.Message = "Chưa có gói cước của dịch vụ chính này.";
        //        //                    break;
        //        //                case -2:
        //        //                    res.Code = "02";
        //        //                    res.Message = "Trạng thái không hợp lệ.";
        //        //                    break;
        //        //                case -3:
        //        //                    res.Code = "03";
        //        //                    res.Message = "Chưa khai báo phạm vi tính cước chính.";
        //        //                    break;
        //        //                case -5:
        //        //                    res.Code = "05";
        //        //                    res.Message = "Chưa cấu hình cước dịnh vụ chính.";
        //        //                    break;
        //        //                case -6:
        //        //                    res.Code = "06";
        //        //                    res.Message = "Lỗi lấy cước dịch vụ cộng thêm.";
        //        //                    break;
        //        //                case -7:
        //        //                    res.Code = "07";
        //        //                    res.Message = "Lỗi lấy phụ phí.";
        //        //                    break;
        //        //                case -8:
        //        //                    res.Code = "08";
        //        //                    res.Message = "Gói cước đang không trong thời gian sử dụng. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -9:
        //        //                    res.Code = "09";
        //        //                    res.Message = "Gói cước đang khóa. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -10:
        //        //                    res.Code = "10";
        //        //                    res.Message = "Chưa khai báo vùng, khu vực của dịch vụ chính. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -11:
        //        //                    res.Code = "11";
        //        //                    res.Message = "Chưa xác định nơi đi. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -12:
        //        //                    res.Code = "12";
        //        //                    res.Message = "Chưa xác định nơi đến. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -13:
        //        //                    res.Code = "13";
        //        //                    res.Message = "Khối lượng phải > 0. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -14:
        //        //                    res.Code = "14";
        //        //                    res.Message = "Giá trị đơn hàng phải > 0. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -15:
        //        //                    res.Code = "15";
        //        //                    res.Message = "Tiền COD phải > 0. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -16:
        //        //                    res.Code = "16";
        //        //                    res.Message = "Nơi xuất phát này chưa được cấu hình cước. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -17:
        //        //                    res.Code = "17";
        //        //                    res.Message = "Gói cước chỉ áp dụng cho nội tỉnh. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -18:
        //        //                    res.Code = "18";
        //        //                    res.Message = "Gói cước chỉ áp dụng cho liên tỉnh. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -19:
        //        //                    res.Code = "19";
        //        //                    res.Message = "Gói cước chỉ áp dụng cho nội vùng. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -20:
        //        //                    res.Code = "20";
        //        //                    res.Message = "Gói cước chỉ áp dụng cho liên vùng. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -21:
        //        //                    res.Code = "21";
        //        //                    res.Message = "Dịch vụ chính CPN TMĐT Nhanh tiết kiệm chỉ áp dụng cho liên tỉnh - liên vùng. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -22:
        //        //                    res.Code = "22";
        //        //                    res.Message = "Dịch vụ EMS Thỏa thuận không áp dụng cho nội tỉnh. Vui lòng kiểm tra lại.";
        //        //                    break;
        //        //                case -23:
        //        //                    res.Code = "23";
        //        //                    res.Message = "Lỗi tính cước theo nấc khối lượng.";
        //        //                    break;
        //        //                default:
        //        //                    res.Code = "99";
        //        //                    res.Message = "Thất bại.";
        //        //                    break;
        //        //            }
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //        //        res.Code = "99";
        //        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        //    }
        //        //}
        //        #endregion
        //    }
        //    catch(Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.Charged: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}
        #endregion
        public static ZResponse Charged(ZTransportPostageChargedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                ZTransportPostageChargedResponse model = null;

                // Xử lý khối lượng quy đổi
                req.Weight = XCommon.ExchangeTransportWeight(new ZTransportExchangeTransportWeightRequest()
                {
                    TransportMainServiceID = req.MainServiceID,
                    TransportSizeLength = req.TransportSizeLength,
                    TransportSizeWidth = req.TransportSizeWidth,
                    TransportSizeHeight = req.TransportSizeHeight,
                    TransportWeight = req.Weight
                });

                var mainService = TransportServiceRepository.SearchById(req.MainServiceID.ToString());
                var extraServices = TransportServiceRepository.SearchById(req.ExtraServiceIDs ?? "");

                using(SqlConnection connection = new SqlConnection(XHelper.ConnectionStringFee))
                {
                    connection.Open();

                    using(DbCommand dbCommand = XHelper.DatabaseFee.GetStoredProcCommand(
                        XHelper.SchemaFee + "Cuoc_Chinh_Tinh_Cuoc_Dich_Vu_Chinh_API_Merchanrt"))
                    {
                        dbCommand.Connection = connection;

                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Dich_Vu_Chinh", DbType.String, mainService);
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Dich_Vu_GTGT", DbType.String, extraServices ?? "");
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Tinh_Nhan", DbType.Int32, Convert.ToInt32(req.FromProvinceID.ToString() + "0000"));
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Tinh_Tra", DbType.Int32, Convert.ToInt32(req.ToProvinceID.ToString() + "0000"));
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Huyen_Tra", DbType.String, req.ToDistrictID.ToString());
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Xa_Tra", DbType.Int32, req.FromWardID);
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Khoi_Luong", DbType.Int32, Convert.ToInt32(req.Weight));
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Khoi_Luong_Quy_Doi", DbType.Int32, Convert.ToInt32(req.Weight));
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_KH", DbType.String, req.CustomerCode ?? "");
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@So_Luong", DbType.Int32, req.Quantity);
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Gia_Tri_Hang", DbType.Int32, Convert.ToInt32(req.Amount));
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@So_Tien", DbType.Int32, Convert.ToInt32(req.COD));
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Nuoc_Tra", DbType.String, "VN");
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Ma_Phu_Phi", DbType.String, "");
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Phan_Loai_BP", DbType.String, "M");
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Dai", DbType.Int32, 0);
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Rong", DbType.Int32, 0);
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@Cao", DbType.Int32, 0);
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@CityCode", DbType.String, "");
                        XHelper.DatabaseFee.AddInParameter(dbCommand, "@PostalCode", DbType.String, "");

                        using(IDataReader reader = dbCommand.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                model = new ZTransportPostageChargedResponse()
                                {
                                    MainFee = Convert.ToInt64(reader["Cuoc_Chinh_VAT"]),
                                    ExtraFee = Convert.ToInt64(reader["Tong_Cuoc_DVCT_VAT"]),
                                    Surcharge = Convert.ToInt64(reader["Cuoc_PP_VAT"]),
                                    MainFeeNotVAT = Convert.ToInt64(reader["Cuoc_Chinh"]),
                                    ExtraFeeNotVAT = Convert.ToInt64(reader["Tong_Cuoc_DVCT"]),
                                    SurchargeNotVAT = Convert.ToInt64(reader["Cuoc_PP"])
                                };

                                model.TotalAmount = model.MainFee + model.ExtraFee + model.Surcharge;
                                model.TotalAmountNotVAT = model.MainFeeNotVAT + model.ExtraFeeNotVAT + model.SurchargeNotVAT;

                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Dữ liệu không tồn tại.";
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportRepository.Charged: " + ex.ToString() + "CUSTOMER_CODE: " + req.CustomerCode ?? "");
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }

            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        #region Tính cước mới
        #endregion
    }
}