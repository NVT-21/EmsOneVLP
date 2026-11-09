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
using EMS.API.Core.Repositories;

namespace EMS.API.Core.Controllers
{
    [RoutePrefix("TransportPostageDetail")]
    public class TransportPostageDetailController : ApiController
    {
        [Route("Add")]
        public ZResponse Add(ZTransportPostageDetailAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_POSTAGE_DETAIL_PKG.ADD_NEW", new object[]
                {
                   0,
                   req.PostageID,
                   req.IsStep,
                   req.Weight,
                   req.LocalFee,
                   req.IntercityFee1,
                   req.IntercityFee2,
                   req.IntercityFee3,
                   req.IntercityFee4,
                   req.IntercityFee5,
                   req.Key,
                   req.CreatedBy
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
                                    res.Message = "Bảng cước không tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không hợp lệ. Vui lòng kiểm tra lại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Khối lượng đã tồn tại. Vui lòng kiểm tra lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Khai báo cước chính - Loại phạm vi đã thay đổi. Vui lòng kiểm tra lại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Căn cứ tính cước đã thay đổi. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageDetail.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Edit")]
        public ZResponse Edit(ZTransportPostageDetailEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_POSTAGE_DETAIL_PKG.EDIT", new object[]
                {
                   0,
                   req.ID,
                   req.Weight,
                   req.LocalFee,
                   req.IntercityFee1,
                   req.IntercityFee2,
                   req.IntercityFee3,
                   req.IntercityFee4,
                   req.IntercityFee5,
                   req.UpdatedBy
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
                                    res.Message = "Không tìm thấy thông tin phù hợp.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không hợp lệ. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageDetailController.Edit: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Remove")]
        public ZResponse Remove(ZTransportPostageDetailRemoveRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_POSTAGE_DETAIL_PKG.REMOVE", new object[]
                {
                    0,
                    req.ID,
                    req.UpdatedBy
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
                                    res.Message = "Không tìm thấy thông tin phù hợp.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không hợp lệ. Vui lòng kiểm tra lại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageDetailController.Remove: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        

        [Route("GetByTransportPostageID")]
        public ZResponse GetByTransportPostageID(int TransportPostageID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                long oWeight = 0;
                var IsStep = "";
                List<ZTransportPostageDetailGetByTransportPostageIDResponse> listItem = new List<ZTransportPostageDetailGetByTransportPostageIDResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_POSTAGE_DETAIL_PKG.GET_BY_TRANSPORT_POSTAGE_ID", new object[]
                {
                    TransportPostageID,
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
                                    IsStep = dr["IS_STEP"].ToString();
                                    if (IsStep == "Y")
                                    {
                                        listItem.Add(new ZTransportPostageDetailGetByTransportPostageIDResponse()
                                        {
                                            ID = Convert.ToInt32(dr["ID"]),
                                            Name = "Trên " + oWeight.ToString("N0") + " cước phí là ",
                                            IsStep = dr["IS_STEP"].ToString(),
                                            Weight = Convert.ToInt64(dr["WEIGHT"]),
                                            LocalFee = Convert.ToInt32(dr["LOCAL_FEE"]),
                                            IntercityFee1 = Convert.ToInt32(dr["INTERCITY_FEE_1"]),
                                            IntercityFee2 = Convert.ToInt32(dr["INTERCITY_FEE_2"]),
                                            IntercityFee3 = Convert.ToInt32(dr["INTERCITY_FEE_3"]),
                                            IntercityFee4 = Convert.ToInt32(dr["INTERCITY_FEE_4"]),
                                            IntercityFee5 = Convert.ToInt32(dr["INTERCITY_FEE_5"]),
                                            ChargingConditionsID = Convert.ToInt32(dr["CHARGING_CONDITIONS_ID"])
                                        });
                                    }
                                    else
                                    {
                                        listItem.Add(new ZTransportPostageDetailGetByTransportPostageIDResponse()
                                        {
                                            ID = Convert.ToInt32(dr["ID"]),
                                            Name = "Trên " + oWeight.ToString("N0") + " đến " + Convert.ToInt64(dr["WEIGHT"]).ToString("N0"),
                                            IsStep = dr["IS_STEP"].ToString(),
                                            Weight = Convert.ToInt64(dr["WEIGHT"]),
                                            LocalFee = Convert.ToInt32(dr["LOCAL_FEE"]),
                                            IntercityFee1 = Convert.ToInt32(dr["INTERCITY_FEE_1"]),
                                            IntercityFee2 = Convert.ToInt32(dr["INTERCITY_FEE_2"]),
                                            IntercityFee3 = Convert.ToInt32(dr["INTERCITY_FEE_3"]),
                                            IntercityFee4 = Convert.ToInt32(dr["INTERCITY_FEE_4"]),
                                            IntercityFee5 = Convert.ToInt32(dr["INTERCITY_FEE_5"])
                                        });
                                        oWeight = Convert.ToInt64(dr["WEIGHT"]);
                                    }
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageDetailController.GetByTransportPostageID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}