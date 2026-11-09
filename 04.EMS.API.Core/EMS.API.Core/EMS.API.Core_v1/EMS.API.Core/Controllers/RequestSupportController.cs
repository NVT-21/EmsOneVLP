using EMS.API.Core.Repositories;
using EMS.Core;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Web.Http;
namespace EMS.API.Core.Controllers
{
    [RoutePrefix("RequestSupport")]
    public class RequestSupportController : ApiController
    {
        [Route("FESearchList")]
        public ZResponse FESearchList(ZRequestSupportFESearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZRequestSupportFESearchListResponse> listItem = new List<ZRequestSupportFESearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.FE_SEARCH_LIST", new object[]
                {
                    req.TypeDate == 0 ? 1 : req.TypeDate,
                    req.FromDate,
                    req.ToDate,
                    req.RequestGroup,
                    req.RequestType,
                    req.Status,
                    req.SearchKey,
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
                                    listItem.Add(new ZRequestSupportFESearchListResponse()
                                    {
                                        ID = Convert.ToInt32(dr["REQUEST_SUPPORT_ID"]),
                                        Code = Convert.ToString(dr["REQUEST_SUPPORT_CODE"]),
                                        EShippingCode = Convert.ToString(dr["ESHIPPING_CODE"]),
                                        RequestGroup = Convert.ToString(dr["REQUEST_SUPPORT_GROUP_NAME"]),
                                        RequestType = Convert.ToString(dr["REQUEST_SUPPORT_TYPE_NAME"]),
                                        Status = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["REQUEST_SUPPORT_STATUS"]),
                                            Name = XData.GetNameRequestSupportStatus(Convert.ToString(dr["REQUEST_SUPPORT_STATUS"])),
                                        },
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),
                                    });
                                }
                            }
                            if (listItem.Count > 0)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(listItem);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.FESearchList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("BESearchList")]
        public ZResponse BESearchList(ZRequestSupportBESearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZRequestSupportBESearchListResponse> lst = new List<ZRequestSupportBESearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.BE_SEARCH_LIST", new object[]
                {
                    req.TypeDate,
                    req.FromDate,
                    req.ToDate,
                    req.RequestSupportCode,
                    req.EShippingCode,
                    req.CrmOrPaypostCode,
                    req.RequestSupportGroup,
                    req.RequestSupportType,
                    req.RequestSupportStatus,
                    req.UserID,
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
                                    lst.Add(new ZRequestSupportBESearchListResponse()
                                    {
                                        RequestSupportID = Convert.ToInt32(dr["REQUEST_SUPPORT_ID"]),
                                        RequestSupportCode = Convert.ToString(dr["REQUEST_SUPPORT_CODE"]),
                                        EShippingCode = Convert.ToString(dr["ESHIPPING_CODE"]),
                                        RequestSupportType = new ZBaseResponse()
                                        {
                                            Name = Convert.ToString(dr["REQUEST_SUPPORT_TYPE_NAME"]),
                                        },
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        Note = Convert.ToString(dr["NOTE"]),
                                        RequestSupportStatus = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["REQUEST_SUPPORT_STATUS"]),
                                            Name = XData.GetNameRequestSupportStatus(Convert.ToString(dr["REQUEST_SUPPORT_STATUS"])),
                                        },
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        UpdatedDate = Convert.ToString(dr["UPDATED_DATE"]),

                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CrmOrPaypostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),

                                        RequestSupportGroup = new ZBaseResponse()
                                        {
                                            ID = string.IsNullOrEmpty(dr["REQUEST_SUPPORT_GROUP_ID"].ToString()) ? 0 : Convert.ToInt32(dr["REQUEST_SUPPORT_GROUP_ID"]),
                                            Name = Convert.ToString(dr["REQUEST_SUPPORT_GROUP_NAME"])
                                        },
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.BESearchList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Add")]
        public ZResponse Add(ZRequestSupportAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resAddRequestSupport = RequestSupportRepository.AddRequestSupport(req);
                if (resAddRequestSupport.Code == "00")
                {
                    var RequestSupportID = Convert.ToInt32(resAddRequestSupport.Data);
                    var resSearchInfoSendCMS = RequestSupportRepository.SearchInfoSendCMS(new ZRequestSupportSearchInfoSendCMSRequest()
                    {
                        RequestSupportID = RequestSupportID,
                        ShopID = req.ShopID,
                    });
                    if (resSearchInfoSendCMS.Code == "00")
                    {
                        var dataSearchInfoSendCMS = JsonConvert.DeserializeObject<ZRequestSupportSearchInfoSendCMSResponse>(resSearchInfoSendCMS.Data);
                        ZRequestSupportSearchInfoSendMailRequest reqSendMail = new ZRequestSupportSearchInfoSendMailRequest()
                        {
                            ShopFullName = dataSearchInfoSendCMS.ShopInfo.FullName,
                            CrmOrPayPostCode = dataSearchInfoSendCMS.ShopInfo.CrmOrPayPostCode,
                            EmailBCG = dataSearchInfoSendCMS.ShopInfo.EmailBCG,
                            EmailCSKH = dataSearchInfoSendCMS.ShopInfo.EmailCSKH,
                            RequestSupportCode = dataSearchInfoSendCMS.RequestSupportInfo.Code,
                            ShippingCode = dataSearchInfoSendCMS.RequestSupportInfo.ShippingCode,
                            RequestSupportGroup = dataSearchInfoSendCMS.RequestSupportInfo.Group,
                            RequestSupportType = dataSearchInfoSendCMS.RequestSupportInfo.Type,
                            Description = dataSearchInfoSendCMS.RequestSupportInfo.Description,
                            ShopID = dataSearchInfoSendCMS.ShopInfo.ID,
                            CreatedDate = dataSearchInfoSendCMS.RequestSupportInfo.CreatedDate,
                        };
                        ZRequestSupportSearchInfoSendEMSRequest reqSendEMS = new ZRequestSupportSearchInfoSendEMSRequest()
                        {
                            RequestSupportID = dataSearchInfoSendCMS.RequestSupportInfo.ID,
                            RequestSupportCode = dataSearchInfoSendCMS.RequestSupportInfo.Code,
                            AcceptancePosCode = dataSearchInfoSendCMS.ShopInfo.AcceptancePosCode,
                            ShippingCode = dataSearchInfoSendCMS.RequestSupportInfo.ShippingCode,
                            UpdateInformation = dataSearchInfoSendCMS.RequestSupportInfo.UpdateInformation,
                            TransportExtraService = "COD,TDTNT",
                            ShopID = dataSearchInfoSendCMS.ShopInfo.ID,
                        };
                        if (dataSearchInfoSendCMS.RequestSupportInfo.Group.ID == 1 && dataSearchInfoSendCMS.RequestSupportInfo.Type.ID == 7)
                        {
                            if (!string.IsNullOrEmpty(dataSearchInfoSendCMS.ShopInfo.AcceptancePosCode))
                            {
                                //Sửa tiền đơn hàng (tiền COD)
                                ZPaypostEditCODMoneyRequest reqPaypostEditCODMoney = new ZPaypostEditCODMoneyRequest()
                                {
                                    ParcelCode = dataSearchInfoSendCMS.RequestSupportInfo.ShippingCode,
                                    Amount = long.Parse(dataSearchInfoSendCMS.RequestSupportInfo.UpdateInformation),
                                    PoCode = dataSearchInfoSendCMS.ShopInfo.AcceptancePosCode
                                };
                                var resPaypostEditCODMoney = PaypostRepository.EditCODMoney(reqPaypostEditCODMoney);

                                XHelper.LogToFile(LogFileType.TRACE, string.Format("RequestSupportController.Add: reqPaypostEditCODMoney={0}||resPaypostEditCODMoney={1}", JsonConvert.SerializeObject(reqPaypostEditCODMoney), JsonConvert.SerializeObject(resPaypostEditCODMoney)));

                                if (resPaypostEditCODMoney.Code == "00")
                                {
                                    var resUpdateEditCODMoney = RequestSupportRepository.UpdateEditCODMoney(new ZRequestSupportUpdateEditCODMoneyRequest()
                                    {
                                        RequestSupportID = dataSearchInfoSendCMS.RequestSupportInfo.ID,
                                        ShopID = dataSearchInfoSendCMS.ShopInfo.ID,
                                        UpdatedBy = dataSearchInfoSendCMS.EmployeeInfo.ID,
                                    });
                                    if (resUpdateEditCODMoney.Code == "00")
                                    {
                                        RequestSupportRepository.SendMailAndPushEMS(reqSendMail, reqSendEMS);
                                        res.Code = resUpdateEditCODMoney.Code;
                                        res.Message = "Tạo yêu cầu hỗ trợ thành công.";
                                    }
                                    else
                                    {
                                        res.Code = resUpdateEditCODMoney.Code;
                                        res.Message = resUpdateEditCODMoney.Message;
                                    }
                                }
                                else
                                {
                                    res.Code = resPaypostEditCODMoney.Code;
                                    res.Message = resPaypostEditCODMoney.Message;
                                }
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Chưa gán mã bưu cục thu gom cho shop. Vui lòng kiểm tra lại.";
                            }
                        }
                        else
                        {
                            RequestSupportRepository.SendEmail(reqSendMail);
                            res.Code = resAddRequestSupport.Code;
                            res.Message = resAddRequestSupport.Message;
                        }
                    }
                    else
                    {
                        res.Code = resSearchInfoSendCMS.Code;
                        res.Message = resSearchInfoSendCMS.Message;
                    }
                }
                else
                {
                    res.Code = resAddRequestSupport.Code;
                    res.Message = resAddRequestSupport.Message;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.Add: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("Add")]
        //public ZResponse Add(ZRequestSupportAddRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var resAddRequestSupport = RequestSupportRepository.AddRequestSupport(req);
        //        if (resAddRequestSupport.Code == "00")
        //        {
        //            var resSearchInfoSendMail = RequestSupportRepository.SearchInfoSendMail(new ZRequestSupportSearchInfoSendMailRequest() { RequestSupportCode = resAddRequestSupport.Data, ShopID = req.ShopID });
        //            if (resSearchInfoSendMail.Code == "00")
        //            {
        //                var InfoSendMail = JsonConvert.DeserializeObject<ZRequestSupportSearchInfoSendMailResponse>(resSearchInfoSendMail.Data);
        //                if (string.IsNullOrEmpty(InfoSendMail.EmailBCG) && string.IsNullOrEmpty(InfoSendMail.EmailCSKH))
        //                {
        //                    res.Code = resAddRequestSupport.Code;
        //                    res.Message = resAddRequestSupport.Message;
        //                }
        //                else
        //                {
        //                    ZApiZohoSendMailFromRequest ReceiveEmail = new ZApiZohoSendMailFromRequest();
        //                    if (string.IsNullOrEmpty(InfoSendMail.EmailCSKH))
        //                    {
        //                        ReceiveEmail = new ZApiZohoSendMailFromRequest()
        //                        {
        //                            address = InfoSendMail.EmailBCG,
        //                            name = "Bưu cục gửi"
        //                        };
        //                    }
        //                    else if (string.IsNullOrEmpty(InfoSendMail.EmailBCG) && (InfoSendMail.RequestGroup == "Sửa thông tin đơn hàng" || InfoSendMail.RequestGroup == "Rút bưu gửi"))
        //                    {
        //                        res.Code = resAddRequestSupport.Code;
        //                        res.Message = resAddRequestSupport.Message;
        //                    }
        //                    else
        //                    {
        //                        if (InfoSendMail.RequestGroup == "Sửa thông tin đơn hàng" || InfoSendMail.RequestGroup == "Rút bưu gửi")
        //                        {
        //                            ReceiveEmail = new ZApiZohoSendMailFromRequest()
        //                            {
        //                                address = InfoSendMail.EmailBCG,
        //                                name = "Bưu cục gửi"
        //                            };
        //                        }
        //                        else
        //                        {
        //                            ReceiveEmail = new ZApiZohoSendMailFromRequest()
        //                            {
        //                                address = InfoSendMail.EmailCSKH,
        //                                name = "Chăm sóc khách hàng"
        //                            };
        //                        }
        //                    }

        //                    List<ZApiZohoSendMailToRequest> to = new List<ZApiZohoSendMailToRequest>();
        //                    to.Add(new ZApiZohoSendMailToRequest()
        //                    {
        //                        email_address = ReceiveEmail
        //                    });

        //                    var stringBody = "";
        //                    stringBody += "Tiêu đề: [EMSOne] Yêu cầu hỗ trợ";
        //                    stringBody += "<br>";
        //                    stringBody += "Nội dung:";
        //                    stringBody += "<br>";
        //                    stringBody += "Bạn có yêu cầu hỗ trợ mới:";
        //                    stringBody += "<br>";
        //                    stringBody += "<table style='border-collapse: collapse; border: 1px solid black'>";
        //                    stringBody += "<tbody>";
        //                    stringBody += "<tr>";
        //                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Khách hàng</td>";
        //                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", InfoSendMail.CustomerName);
        //                    stringBody += "</tr>";
        //                    stringBody += "<tr>";
        //                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Mã yêu cầu</td>";
        //                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", InfoSendMail.RequestSupportCode);
        //                    stringBody += "</tr>";
        //                    stringBody += "<tr>";
        //                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Mã CRM</td>";
        //                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", InfoSendMail.CrmOrPaypostCode);
        //                    stringBody += "</tr>";
        //                    stringBody += "<tr>";
        //                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Mã E1</td>";
        //                    var TrackingURL = string.Format(XHelper.FrontEndURL + "Handle/DetailShippingView?ShippingCode={0}&ShopID={1}", Security.EncodeBase64(InfoSendMail.EShippingCode), Security.EncodeBase64(req.ShopID.ToString()));
        //                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'><a href='{1}' target='_blank'>{0}</a></td>", InfoSendMail.EShippingCode, TrackingURL);
        //                    stringBody += "</tr>";
        //                    stringBody += "<tr>";
        //                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Loại yêu cầu</td>";
        //                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", InfoSendMail.RequestType);
        //                    stringBody += "</tr>";
        //                    stringBody += "<tr>";
        //                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Nội dung</td>";
        //                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", InfoSendMail.Description);
        //                    stringBody += "</tr>";
        //                    stringBody += "<tr>";
        //                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Thời gian tạo</td>";
        //                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", InfoSendMail.CreatedDate);
        //                    stringBody += "</tr>";
        //                    stringBody += "</tbody>";
        //                    stringBody += "</table>";


        //                    var resSendMail = ZohoRepository.SendMail(new ZApiZohoSendMailRequest()
        //                    {
        //                        to = to,
        //                        subject = "[EMSOne] Yêu cầu hỗ trợ",
        //                        htmlbody = stringBody
        //                    });

        //                    if (resSendMail.Code == "00")
        //                    {
        //                        var resUpdateStatusSendMail = RequestSupportRepository.UpdateStatusSendMail(new ZRequestSupportUpdateStatusSendMailRequest() 
        //                        {
        //                            RequestSupportCode = InfoSendMail.RequestSupportCode,
        //                            ShopID = req.ShopID
        //                        });

        //                        if(resUpdateStatusSendMail.Code == "00")
        //                        {
        //                            res.Code = resAddRequestSupport.Code;
        //                            res.Message = resAddRequestSupport.Message;
        //                        }
        //                        else
        //                        {
        //                            res.Code = resAddRequestSupport.Code;
        //                            res.Message = resAddRequestSupport.Message;
        //                            XHelper.LogToFile(LogFileType.MESSAGE, "RequestSupport.Add: Tạo yêu cầu hỗ trợ thành công nhưng cập nhật trạng thái gửi email thất bại.||RequestSupportCode=" + InfoSendMail.RequestSupportCode);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        res.Code = resAddRequestSupport.Code;
        //                        res.Message = resAddRequestSupport.Message;
        //                        XHelper.LogToFile(LogFileType.MESSAGE, "RequestSupport.Add: Tạo yêu cầu hỗ trợ thành công nhưng gửi email thất bại.||RequestSupportCode=" + InfoSendMail.RequestSupportCode);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                res.Code = resAddRequestSupport.Code;
        //                res.Message = resAddRequestSupport.Message;
        //                XHelper.LogToFile(LogFileType.MESSAGE, "RequestSupport.Add: Tạo yêu cầu hỗ trợ thành công nhưng lấy thông tin gửi email thất bại.||RequestSupportCode=" + resSearchInfoSendMail.Data);
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resAddRequestSupport.Code;
        //            res.Message = resAddRequestSupport.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.Add: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("SearchInfoSendRequestSupport")]
        //public ZResponse SearchInfoSendRequestSupport(ZRequestSupportSearchInfoSendRequestSupportRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = RequestSupportRepository.SearchInfoSendRequestSupport(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.SearchInfoSendRequestSupport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("UpdateStatus")]
        public ZResponse UpdateStatus(ZRequestSupportUpdateStatusRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.UPDATE_STATUS", new object[]
                {
                    0,
                    req.RequestSupportCode,
                    req.AmndState,
                    req.ShopID,
                    req.UpdatedBy,
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã yêu cầu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.UpdateStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("GetByID")]
        public ZResponse GetByID(int RequestSupportID, int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = RequestSupportRepository.GetByID(new ZRequestSupportGetByIDRequest()
                {
                    RequestSupportID = RequestSupportID,
                    ShopID = ShopID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("Detail")]
        public ZResponse Detail(ZRequestSupportDetailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                ZRequestSupportDetailResponse item = null;
                var resGetByID = RequestSupportRepository.GetByID(new ZRequestSupportGetByIDRequest()
                {
                    RequestSupportID = req.RequestSupportID,
                    ShopID = req.ShopID
                });
                if (resGetByID.Code == "00")
                {
                    var itemGetByID = JsonConvert.DeserializeObject<ZRequestSupportDetailResponse>(resGetByID.Data);
                    item = new ZRequestSupportDetailResponse()
                    {
                        RequestSupportID = itemGetByID.RequestSupportID,
                        RequestSupportCode = itemGetByID.RequestSupportCode,
                        RequestGroup = itemGetByID.RequestGroup,
                        RequestType = itemGetByID.RequestType,
                        RequestSupportDescription = itemGetByID.RequestSupportDescription,
                        RequestSupportDocument = itemGetByID.RequestSupportDocument,
                        EmployeeName = itemGetByID.EmployeeName,
                        CreatedDate = itemGetByID.CreatedDate,
                        RequestSupportStatus = itemGetByID.RequestSupportStatus,
                        UpdateInformation = itemGetByID.UpdateInformation,
                        OldUpdateInformation = itemGetByID.OldUpdateInformation,
                        PosC = itemGetByID.PosC,
                        EShippingCode = itemGetByID.EShippingCode,
                        OrderID = itemGetByID.OrderID,
                        OrderCode = itemGetByID.OrderCode,
                        OrderMessage = itemGetByID.OrderMessage,
                        WeightBefore = itemGetByID.WeightBefore,
                        WeightAfter = itemGetByID.WeightAfter,
                        SenderName = itemGetByID.SenderName,
                        SenderPhone = itemGetByID.SenderPhone,
                        SenderFullAddress = itemGetByID.SenderFullAddress,
                        ReceiverName = itemGetByID.ReceiverName,
                        ReceiverPhone = itemGetByID.ReceiverPhone,
                        ReceiverFullAddress = itemGetByID.ReceiverFullAddress,
                        OrderStatus = itemGetByID.OrderStatus,
                        TransportOrderValue = itemGetByID.TransportOrderValue,
                        TotalCOD = itemGetByID.TotalCOD,
                        TransportFee = itemGetByID.TransportFee,
                        TransportMainService = itemGetByID.TransportMainService,
                        TransportExtraService = itemGetByID.TransportExtraService,

                        ShopID = itemGetByID.ShopID,
                        ShopCode = itemGetByID.ShopCode,
                        CrmOrPaypostCode = itemGetByID.CrmOrPaypostCode,
                        ShopPhone = itemGetByID.ShopPhone,
                        ShopFullAddress = itemGetByID.ShopFullAddress,

                        ListSearchHistory = new List<ZRequestSupportSearchHistoryResponse>(),

                        ListSearchProcess = new List<ZRequestSupportSearchProcessResponse>(),
                    };

                    //Lấy lịch sử
                    var resHistory = RequestSupportRepository.SearchHistory(new ZRequestSupportSearchHistoryRequest() { RequestSupportID = req.RequestSupportID });
                    if (resHistory.Code == "00")
                    {
                        item.ListSearchHistory = JsonConvert.DeserializeObject<List<ZRequestSupportSearchHistoryResponse>>(resHistory.Data);
                    }

                    //Lấy kết quả xử lý
                    var resProcess = RequestSupportRepository.SearchProcess(new ZRequestSupportSearchProcessRequest() { RequestSupportID = req.RequestSupportID });
                    if (resProcess.Code == "00")
                    {
                        item.ListSearchProcess = JsonConvert.DeserializeObject<List<ZRequestSupportSearchProcessResponse>>(resProcess.Data);
                    }

                    if (item != null)
                    {
                        res.Code = "00";
                        res.Message = "Thành công";
                        res.Data = JsonConvert.SerializeObject(item);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không tồn tại.";
                    }
                }
                else
                {
                    res.Code = resGetByID.Code;
                    res.Message = resGetByID.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.Detail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AcceptedStatus")]
        public ZResponse AcceptedStatus(ZRequestSupportAcceptedStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = RequestSupportRepository.AcceptedStatus(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.AcceptedStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UnAcceptedStatus")]
        public ZResponse UnAcceptedStatus(ZRequestSupportUnAcceptedStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.UNACCEPTED_STATUS", new object[]
                {
                    0,
                    req.RequestSupportID,
                    req.ShopID,
                    req.Reason,
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
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã yêu cầu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không được phép thực hiện thao tác.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lưu lịch sử thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.UnAcceptedStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ProcessStatus")]
        public ZResponse ProcessStatus(ZRequestSupportProcessStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.PROCESS_STATUS", new object[]
                {
                    0,
                    req.RequestSupportID,
                    req.ShopID,
                    req.ResultDocument,
                    req.ResultDescription,
                    req.ResultIndemnify,
                    req.ResultRequestSupport,
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
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã yêu cầu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không được phép thực hiện thao tác.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lưu lịch sử thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.ProcessStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("CloseStatus")]
        public ZResponse CloseStatus(ZRequestSupportCloseStatusRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.CLOSE_STATUS", new object[]
                {
                    0,
                    req.RequestSupportID,
                    req.ShopID,
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
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã yêu cầu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không được phép thực hiện thao tác.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Lưu lịch sử thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.CloseStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ReOpenStatus")]
        public ZResponse ReOpenStatus(ZRequestSupportReOpenStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.RE_OPEN_STATUS", new object[]
                {
                    0,
                    req.RequestSupportID,
                    req.ShopID,
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
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã yêu cầu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không được phép thực hiện thao tác.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Nhóm yêu cầu không được phép mở lại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Lưu lịch sử thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.ReOpenStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("ExistStatus")]
        public ZResponse ExistStatus(ZRequestSupportExistStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = RequestSupportRepository.ExistStatus(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.ExistStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("NotifyList")]
        public ZResponse NotifyList(ZRequestSupportNotifyListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZRequestSupportNotifyListResponse item = null;
                List<ZRequestSupportNotifyListDetail> listItem = new List<ZRequestSupportNotifyListDetail>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.NOTIFY_LIST", new object[]
                {
                    req.MaxRequestSupportID,
                    req.UserID,
                    0,
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
                                    listItem.Add(new ZRequestSupportNotifyListDetail()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        RequestSupportCode = Convert.ToString(dr["REQUEST_SUPPORT_CODE"]),
                                        EShippingCode = Convert.ToString(dr["ESHIPPING_CODE"]),
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                    });
                                }
                            }

                            item = new ZRequestSupportNotifyListResponse()
                            {
                                MaxRequestSupportID = Convert.ToInt32(dbCommand.Parameters["P_OUT_MAX_REQUEST_SUPPORT_ID"].Value),
                                CountRequestSupport = listItem.Count(),
                                ListRequestSupportNotify = listItem
                            };

                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportController.NotifyList " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống. Vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("UpdateClickNotify")]
        public ZResponse UpdateClickNotify(ZRequestSupportReOpenStatusRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.UPDATE_CLICK_NOTIFY", new object[]
                {
                    0,
                    req.RequestSupportID,
                    req.ShopID
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Cập nhật thành công.";
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã yêu cầu không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.UpdateClickNotify: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        //[Route("LockAcceptedStatus")]
        //public ZResponse LockAcceptedStatus(ZRequestSupportLockAcceptedStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = RequestSupportRepository.LockAcceptedStatus(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.LockAcceptedStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //[Route("UnLockAcceptedStatus")]
        //public ZResponse UnLockAcceptedStatus(ZRequestSupportUnLockAcceptedStatusRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = RequestSupportRepository.UnLockAcceptedStatus(req);
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.UnLockAcceptedStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        [Route("SendRequestSupport")]
        public ZResponse SendRequestSupport(ZRequestSupportSendRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resSearchInfoSendCMS = RequestSupportRepository.SearchInfoSendCMS(new ZRequestSupportSearchInfoSendCMSRequest()
                {
                    RequestSupportID = req.RequestSupportID,
                    ShopID = req.ShopID,
                    UserID = req.UserID
                });
                if (resSearchInfoSendCMS.Code == "00")
                {
                    var dataSearchInfoSendCMS = JsonConvert.DeserializeObject<ZRequestSupportSearchInfoSendCMSResponse>(resSearchInfoSendCMS.Data);
                    if (dataSearchInfoSendCMS.RequestSupportInfo.Status.Code != "W" /*&& dataSearchInfoSendRequestSupport.Status != "O"*/)
                    {
                        res.Code = "01";
                        res.Message = "Trạng thái đã bị thay đổi.";
                    }
                    else if (dataSearchInfoSendCMS.RequestSupportInfo.Group.ID == 1 || dataSearchInfoSendCMS.RequestSupportInfo.Group.ID == 30)
                    {
                        //Sửa thông tin đơn hàng hoặc Rút bưu gửi
                        if (dataSearchInfoSendCMS.RequestSupportInfo.Type.ID == 7)
                        {
                            //Nếu là thuộc loại yêu cầu sửa tiền COD thì báo lỗi
                            res.Code = "01";
                            res.Message = "Loại yêu cầu không hợp lệ.";
                        }
                        else
                        {
                            res = RequestSupportRepository.UpdateOrderInformation(new ZRequestSupportUpdateOrderInformationRequest() { RequestSupportID = req.RequestSupportID, ShopID = req.ShopID, UpdatedBy = req.UserID });
                        }
                    }
                    else if (dataSearchInfoSendCMS.RequestSupportInfo.Group.ID == 27 && dataSearchInfoSendCMS.RequestSupportInfo.Type.ID == 28)
                    {
                        //Phát lại/ Chuyển hoàn(Yêu cầu BCP phát lại đơn hàng)
                        res = RequestSupportRepository.AcceptedStatus(new ZRequestSupportAcceptedStatusRequest() { RequestSupportID = req.RequestSupportID, ShopID = req.ShopID, UpdatedBy = req.UserID });
                    }
                    else
                    {
                        List<ZCMSAddRequestSupportItemRequest> ListRequestSupport = new List<ZCMSAddRequestSupportItemRequest>();
                        ListRequestSupport.Add(new ZCMSAddRequestSupportItemRequest()
                        {
                            Id = dataSearchInfoSendCMS.RequestSupportInfo.Code,
                            AcountID = dataSearchInfoSendCMS.ShopInfo.CrmOrPayPostCode,
                            Employee__c = "", //dataSearchInfoSendRequestSupport.EmployeeID.ToString(),
                            Nhan_vien_CSKH__c = dataSearchInfoSendCMS.UserInfo.UserCMS,
                            Tai_Lieu_Dinh_Kem__c = dataSearchInfoSendCMS.RequestSupportInfo.Document == "" ? "" : XHelper.PathRequestSupport + dataSearchInfoSendCMS.RequestSupportInfo.Document,
                            CaseNumber = "",
                            Description = dataSearchInfoSendCMS.RequestSupportInfo.Description,
                            So_phieu_khieu_nai__c = "",
                            Li_do_khieu_nai__c = dataSearchInfoSendCMS.RequestSupportInfo.ReasonForComplaint,
                            Priority = dataSearchInfoSendCMS.RequestSupportInfo.Priority,
                            Origin = dataSearchInfoSendCMS.RequestSupportInfo.Origin,
                            Nhom_dich_vu__c = "Dịch vụ bưu chính chuyển phát",//dataSearchInfoSendRequestSupport.ServiceGroup,
                            Location_Type__c = "Yêu cầu khách hàng",//dataSearchInfoSendRequestSupport.Scope,
                            Service__c = dataSearchInfoSendCMS.RequestSupportInfo.TransportServiceCode,
                            Status = "Chưa xử lý",
                            So_Ho_So__c = "",
                            //POS__c = dataSearchInfoSendCMS.RequestSupportInfo.PosC,
                            POS__c = dataSearchInfoSendCMS.UserInfo.UserCMS,
                            //Ng_y_ti_p_nh_n__c = DateTime.Parse(dataSearchInfoSendCMS.RequestSupportInfo.CreatedDate, CultureInfo.InvariantCulture).ToString("yyyy-MM-ddTHH:mm:ss"),
                            Ng_y_ti_p_nh_n__c = DateTime.ParseExact(dataSearchInfoSendCMS.RequestSupportInfo.CreatedDate, "dd/MM/yyyy HH:mm:ss", null).ToString("yyyy-MM-ddTHH:mm:ss"),
                            Sales_Order__c = dataSearchInfoSendCMS.RequestSupportInfo.ShippingCode,
                            SuppliedName = dataSearchInfoSendCMS.EmployeeInfo.FullName,
                            SuppliedPhone = dataSearchInfoSendCMS.EmployeeInfo.MobileNumber,
                            SuppliedEmail = dataSearchInfoSendCMS.EmployeeInfo.Email,
                            Contact_Address__c = dataSearchInfoSendCMS.EmployeeInfo.Street,
                            Contact_province__c = dataSearchInfoSendCMS.EmployeeInfo.Province.Name,
                            Contact_district__c = dataSearchInfoSendCMS.EmployeeInfo.District.Name,
                            Contact_commune__c = dataSearchInfoSendCMS.EmployeeInfo.Ward.Name,
                            CreatedDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                            LastModifiedDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                            Files = null,
                            Attachments = null
                        });
                        var resSendRequestSupport = XCommon.AddRequestSupport(ListRequestSupport);

                        XHelper.LogToFile(LogFileType.TRACE, string.Format("RequestSupportController.SendRequestSupport: req={0}||res={1}", JsonConvert.SerializeObject(ListRequestSupport), JsonConvert.SerializeObject(resSendRequestSupport)));

                        if (resSendRequestSupport.Code == "00")
                        {
                            res = RequestSupportRepository.AcceptedStatus(new ZRequestSupportAcceptedStatusRequest() { RequestSupportID = req.RequestSupportID, ShopID = req.ShopID, UserCMS = dataSearchInfoSendCMS.UserInfo.UserCMS, UpdatedBy = req.UserID });
                        }
                        else if (resSendRequestSupport.Code == "01")
                        {
                            var checkSuccessData = JsonConvert.DeserializeObject<ZCMSAddRequestSupportResponse>(resSendRequestSupport.Data);
                            if (checkSuccessData.rows != null && checkSuccessData.rows.Count > 0)
                            {
                                var checkRequestSupportCodeError = checkSuccessData.rows.Where(m => m.Id == dataSearchInfoSendCMS.RequestSupportInfo.Code).ToList();
                                if (checkRequestSupportCodeError.Count > 0)
                                {
                                    if (checkRequestSupportCodeError[0].error.Trim() == "Yêu cầu đã tồn tại trong hệ thống HTKH.")
                                    {
                                        var resExistStatus = RequestSupportRepository.ExistStatus(new ZRequestSupportExistStatusRequest() { RequestSupportID = req.RequestSupportID, ShopID = req.ShopID, UpdatedBy = req.UserID }); // Trạng thái đã tồn tại
                                        if (resExistStatus.Code == "00")
                                        {
                                            res.Code = "01";
                                            res.Message = checkRequestSupportCodeError[0].error;
                                        }
                                        else
                                        {
                                            res.Code = resExistStatus.Code;
                                            res.Message = resExistStatus.Message;
                                        }
                                    }
                                    else
                                    {
                                        res.Code = "01";
                                        res.Message = checkRequestSupportCodeError[0].error;
                                    }
                                }
                                else
                                {
                                    res = RequestSupportRepository.AcceptedStatus(new ZRequestSupportAcceptedStatusRequest() { RequestSupportID = req.RequestSupportID, ShopID = req.ShopID, UserCMS = dataSearchInfoSendCMS.UserInfo.UserCMS, UpdatedBy = req.UserID });
                                }
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Lỗi kết nối với API của đối tác, vui lòng liên hệ quản trị.";
                            }
                        }
                        else
                        {
                            res.Code = resSendRequestSupport.Code;
                            res.Message = resSendRequestSupport.Message;
                        }
                    }
                }
                else
                {
                    res.Code = resSearchInfoSendCMS.Code;
                    res.Message = resSearchInfoSendCMS.Message;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.SendRequestSupport: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        [Route("AddProcessResult")]
        public ZResponse AddProcessResult(ZRequestSupportAddProcessResultRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resResultRequestSupport = XCommon.ResultRequestSupport(new ZCMSResultRequestSupportRequest() { RequestSupportCode = req.RequestSupportCode });
                if (resResultRequestSupport.Code == "00")
                {
                    var DataResultRequestSupport = JsonConvert.DeserializeObject<ZCMSResultRequestSupportResponse>(resResultRequestSupport.Data);
                    if (DataResultRequestSupport.lstTTicketActionModel.Count > 0)
                    {
                        var isSuccess = true;
                        foreach (var ItemProcess in DataResultRequestSupport.lstTTicketActionModel)
                        {
                            var resAddProcess = RequestSupportRepository.AddProcess(new ZRequestSupportAddProcessRequest()
                            {
                                RequestSupportCode = req.RequestSupportCode,
                                ActID = ItemProcess.actId,
                                ActType = ItemProcess.actType,
                                ActContent = ItemProcess.actContent,
                                ActAttachFile = ItemProcess.actAttachFile,
                                ActResult = ItemProcess.actResult,
                                NextOrg = ItemProcess.nextOrg,
                                NextOrgName = ItemProcess.nextOrgName,
                                RefOrg = ItemProcess.refOrg,
                                RefOrgName = ItemProcess.refOrgName,
                                CreatedUser = ItemProcess.createdUser,
                                CreatedOrg = ItemProcess.createdOrg,
                                CreatedDate = string.IsNullOrEmpty(ItemProcess.createdDate) ? "" : XData.TimezoneToDateLocal(ItemProcess.createdDate).ToString("dd/MM/yyyy HH:mm:ss"),
                                ModifyDate = string.IsNullOrEmpty(ItemProcess.modifyDate) ? "" : XData.TimezoneToDateLocal(ItemProcess.modifyDate).ToString("dd/MM/yyyy HH:mm:ss"),
                                AmndState = "I",
                                ShopID = req.ShopID,
                            });
                            if (resAddProcess.Code != "00")
                            {
                                isSuccess = false;
                                res.Code = resAddProcess.Code;
                                res.Message = resAddProcess.Message;
                                break;
                            }
                        }
                        if (isSuccess == true)
                        {
                            res = RequestSupportRepository.UpdateAmndStateProcess(new ZRequestSupportUpdateAmndStateProcessRequest()
                            {
                                RequestSupportCode = req.RequestSupportCode,
                                Status = DataResultRequestSupport.ttkStatus,
                                UpdatedBy = req.UpdatedBy,
                                ShopID = req.ShopID
                            });
                        }
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu tiến trình xử lý yêu cầu hỗ trợ không tồn tại";
                    }
                }
                else
                {
                    res.Code = resResultRequestSupport.Code;
                    res.Message = resResultRequestSupport.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupport.AddProcessResult: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}