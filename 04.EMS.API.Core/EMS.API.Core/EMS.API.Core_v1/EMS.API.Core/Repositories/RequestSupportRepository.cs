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
using System.Threading.Tasks;
using System.Threading;

namespace EMS.API.Core.Repositories
{
    public class RequestSupportRepository
    {
        public static ZResponse GetByID(ZRequestSupportGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZRequestSupportGetByIDResponse model = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.GET_BY_ID", new object[]
                {
                    req.RequestSupportID,
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
                                if (dr.Read())
                                {
                                    model = new ZRequestSupportGetByIDResponse()
                                    {
                                        RequestSupportID = Convert.ToInt32(dr["REQUEST_SUPPORT_ID"]),
                                        RequestSupportCode = Convert.ToString(dr["REQUEST_SUPPORT_CODE"]),
                                        RequestGroup = Convert.ToString(dr["REQUEST_GROUP"]),
                                        RequestType = Convert.ToString(dr["REQUEST_TYPE"]),
                                        RequestSupportDescription = Convert.ToString(dr["REQUEST_SUPPORT_DESCRIPTION"]),
                                        RequestSupportDocument = Convert.ToString(dr["REQUEST_SUPPORT_DOCUMENT"]),
                                        EmployeeName = Convert.ToString(dr["EMPLOYEE_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        RequestSupportStatus = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["REQUEST_SUPPORT_STATUS"]),
                                            Name = XData.GetNameRequestSupportStatus(Convert.ToString(dr["REQUEST_SUPPORT_STATUS"])),
                                        },
                                        OldUpdateInformation = Convert.ToString(dr["OLD_UPDATE_INFORMATION"]),
                                        UpdateInformation = Convert.ToString(dr["NEW_UPDATE_INFORMATION"]),
                                        PosC = Convert.ToString(dr["POS_C"]),
                                        EShippingCode = Convert.ToString(dr["ESHIPPING_CODE"]),
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        OrderCode = Convert.ToString(dr["ORDER_CODE"]),
                                        OrderMessage = Convert.ToString(dr["ORDER_MESSAGE"]),
                                        WeightBefore = Convert.ToInt64(dr["WEIGHT_BEFORE"]),
                                        WeightAfter = Convert.ToInt64(dr["WEIGHT_AFTER"]),
                                        SenderName = Convert.ToString(dr["SENDER_NAME"]),
                                        SenderPhone = Convert.ToString(dr["SENDER_PHONE"]),
                                        SenderFullAddress = Convert.ToString(dr["SENDER_FULL_ADDRESS"]),
                                        ReceiverName = Convert.ToString(dr["RECEIVER_NAME"]),
                                        ReceiverPhone = Convert.ToString(dr["RECEIVER_PHONE"]),
                                        ReceiverFullAddress = Convert.ToString(dr["RECEIVER_FULL_ADDRESS"]),
                                        OrderStatus = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["ORDER_STATUS"]),
                                            Name = XData.GetNameOrderStatus(Convert.ToString(dr["ORDER_STATUS"])),
                                        },
                                        TransportOrderValue = Convert.ToInt64(dr["TRANSPORT_ORDER_VALUE"]),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        TransportMainService = Convert.ToString(dr["TRANSPORT_MAIN_SERVICE"]),
                                        TransportExtraService = Convert.ToString(dr["TRANSPORT_EXTRA_SERVICE"]),

                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        ShopCode = Convert.ToString(dr["SHOP_CODE"]),
                                        CrmOrPaypostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),
                                        ShopPhone = Convert.ToString(dr["SHOP_PHONE"]),
                                        ShopFullAddress = Convert.ToString(dr["SHOP_FULL_ADDRESS"]),

                                    };
                                }
                            }
                            if (model != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại";
                                res.Data = null;
                            }

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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.GetByID: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static void SendEmail(ZRequestSupportSearchInfoSendMailRequest req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.EmailBCG) && string.IsNullOrEmpty(req.EmailCSKH))
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("req={0}||reason={1}", JsonConvert.SerializeObject(req), "EmailBCG và EmailCSKH đều trống"));
                    return;
                }
                else
                {
                    ZZohoSendMailFromRequest ReceiveEmail = new ZZohoSendMailFromRequest();
                    if (string.IsNullOrEmpty(req.EmailCSKH))
                    {
                        ReceiveEmail = new ZZohoSendMailFromRequest()
                        {
                            address = req.EmailBCG,
                            name = "Bưu cục gửi"
                        };
                    }
                    else if (string.IsNullOrEmpty(req.EmailBCG) && (req.RequestSupportGroup.ID == 1 || req.RequestSupportGroup.ID == 30))
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("req={0}||reason={1}", JsonConvert.SerializeObject(req), "EmailBCG trống và nhóm yêu cầu thuộc Sửa thông tin đơn hàng hoặc Rút bưu gửi"));
                        return;
                    }
                    else
                    {
                        if (req.RequestSupportGroup.ID == 1 || req.RequestSupportGroup.ID == 30)
                        {
                            ReceiveEmail = new ZZohoSendMailFromRequest()
                            {
                                address = req.EmailBCG,
                                name = "Bưu cục gửi"
                            };
                        }
                        else
                        {
                            ReceiveEmail = new ZZohoSendMailFromRequest()
                            {
                                address = req.EmailCSKH,
                                name = "Chăm sóc khách hàng"
                            };
                        }
                    }

                    List<ZZohoSendMailToRequest> to = new List<ZZohoSendMailToRequest>();
                    to.Add(new ZZohoSendMailToRequest()
                    {
                        email_address = ReceiveEmail
                    });

                    var stringBody = "";
                    stringBody += "Tiêu đề: [EMSOne] Yêu cầu hỗ trợ";
                    stringBody += "<br>";
                    stringBody += "Nội dung:";
                    stringBody += "<br>";
                    stringBody += "Bạn có yêu cầu hỗ trợ mới:";
                    stringBody += "<br>";
                    stringBody += "<table style='border-collapse: collapse; border: 1px solid black'>";
                    stringBody += "<tbody>";
                    stringBody += "<tr>";
                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Khách hàng</td>";
                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", req.ShopFullName);
                    stringBody += "</tr>";
                    stringBody += "<tr>";
                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Mã yêu cầu</td>";
                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", req.RequestSupportCode);
                    stringBody += "</tr>";
                    stringBody += "<tr>";
                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Mã CRM</td>";
                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", req.CrmOrPayPostCode);
                    stringBody += "</tr>";
                    stringBody += "<tr>";
                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Mã E1</td>";
                    var TrackingURL = string.Format(XHelper.FrontEndURL + "Handle/DetailShippingView?ShippingCode={0}&ShopID={1}", Security.EncodeBase64(req.ShippingCode), Security.EncodeBase64(req.ShopID.ToString()));
                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'><a href='{1}' target='_blank'>{0}</a></td>", req.ShippingCode, TrackingURL);
                    stringBody += "</tr>";
                    stringBody += "<tr>";
                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Loại yêu cầu</td>";
                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", req.RequestSupportType.Name);
                    stringBody += "</tr>";
                    stringBody += "<tr>";
                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Nội dung</td>";
                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", req.Description);
                    stringBody += "</tr>";
                    stringBody += "<tr>";
                    stringBody += "<td style='border: 1px solid black; padding: 5px'>Thời gian tạo</td>";
                    stringBody += string.Format("<td style='border: 1px solid black; padding: 5px'>{0}</td>", req.CreatedDate);
                    stringBody += "</tr>";
                    stringBody += "</tbody>";
                    stringBody += "</table>";

                    ZZohoSendMailRequest reqSendMail = new ZZohoSendMailRequest()
                    {
                        to = to,
                        subject = "[EMSOne] Yêu cầu hỗ trợ",
                        htmlbody = stringBody
                    };

                    var resSendMail = ZohoRepository.SendMail(reqSendMail);

                    if (resSendMail.Code == "00")
                    {
                        ZRequestSupportUpdateStatusSendMailRequest reqUpdateStatusSendMail = new ZRequestSupportUpdateStatusSendMailRequest()
                        {
                            RequestSupportCode = req.RequestSupportCode,
                            ShopID = req.ShopID
                        };
                        var resUpdateStatusSendMail = UpdateStatusSendMail(reqUpdateStatusSendMail);

                        if (resUpdateStatusSendMail.Code == "00")
                        {
                            return;
                        }
                        else
                        {
                            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("RequestSupportRepository.UpdateStatusSendMail: Cập nhật trạng thái gửi email thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqUpdateStatusSendMail), JsonConvert.SerializeObject(resUpdateStatusSendMail)));
                        }
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("RequestSupportRepository.SendMail: Gửi email thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqSendMail), JsonConvert.SerializeObject(resSendMail)));
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.SendEmail: " + ex.Message);
            }
        }

        public static void SendEMS(ZRequestSupportSearchInfoSendEMSRequest req)
        {
            try
            {
                List<ZEMSPushAffairRequest> reqEMSPushAffair = new List<ZEMSPushAffairRequest>();
                reqEMSPushAffair.Add(new ZEMSPushAffairRequest()
                {
                    id_e1 = req.RequestSupportID.ToString(),
                    ma_bc_khai_thac = Convert.ToInt32(req.AcceptancePosCode),
                    ma_e1 = req.ShippingCode,
                    tien_cod = Convert.ToInt32(req.UpdateInformation),
                    dv_cong_them = req.TransportExtraService
                });
                var resEMSPushAffair = EMSRepository.PushAffair(reqEMSPushAffair);
                if (resEMSPushAffair.Code == "00")
                {
                    ZRequestSupportUpdateStatusSendEMSRequest reqUpdateStatusSendEMS = new ZRequestSupportUpdateStatusSendEMSRequest() 
                    {
                        RequestSupportCode = req.RequestSupportCode,
                        ShopID = req.ShopID
                    };
                    var resUpdateStatusSendEMS = UpdateStatusSendEMS(reqUpdateStatusSendEMS);

                    if (resUpdateStatusSendEMS.Code == "00")
                    {
                        return;
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("RequestSupport.UpdateStatusSendEMS: Cập nhật trạng thái gửi ems thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqUpdateStatusSendEMS), JsonConvert.SerializeObject(resUpdateStatusSendEMS)));
                    }
                }
                else
                {
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("RequestSupport.SendEMS: Gửi ems thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqEMSPushAffair), JsonConvert.SerializeObject(resEMSPushAffair)));
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.SendEMS: " + ex.Message);
            }
        }

        public static void SendMailAndPushEMS(ZRequestSupportSearchInfoSendMailRequest reqSendMail, ZRequestSupportSearchInfoSendEMSRequest reqSendEMS)
        {
            Task SendEmailTask = new Task(()=> { SendEmail(reqSendMail); });
            SendEmailTask.Start();
            Task SendEMSTask = new Task(() => { SendEMS(reqSendEMS); });
            SendEMSTask.Start();
            Task.WaitAll(SendEmailTask, SendEMSTask);
        }

        public static ZResponse SearchInfoSendCMS(ZRequestSupportSearchInfoSendCMSRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZRequestSupportSearchInfoSendCMSResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.SEARCH_INFO_SEND_CMS", new object[]
                {
                    req.RequestSupportID,   //P_REQUEST_SUPPORT_ID      NUMBER
                    req.ShopID,             //P_SHOP_ID                 NUMBER
                    //req.UserID,             //P_USER_ID                 NUMBER
                    0,                      //P_RETURN_CODE             OUT NUMBER
                    0                       //P_OUT_CURSOR              OUT T_CURSOR
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
                                if (dr.Read())
                                {
                                    item = new ZRequestSupportSearchInfoSendCMSResponse()
                                    {
                                        RequestSupportInfo = new ZRequestSupportSearchInfoSendCMSRequestSupportInfoResponse() 
                                        {
                                            ID = Convert.ToInt32(dr["REQUEST_SUPPORT_ID"]),
                                            Code = Convert.ToString(dr["REQUEST_SUPPORT_CODE"]),
                                            ShippingCode = Convert.ToString(dr["ESHIPPING_CODE"]),
                                            Group = new ZBaseResponse() 
                                            {
                                                ID = Convert.ToInt32(dr["REQUEST_SUPPORT_GROUP_ID"]),
                                                Name = Convert.ToString(dr["REQUEST_SUPPORT_GROUP_NAME"]),
                                            },
                                            Type = new ZBaseResponse()
                                            {
                                                ID = Convert.ToInt32(dr["REQUEST_SUPPORT_TYPE_ID"]),
                                                Name = Convert.ToString(dr["REQUEST_SUPPORT_TYPE_NAME"]),
                                            },
                                            Document = Convert.ToString(dr["REQUEST_SUPPORT_DOCUMENT"]),
                                            Description = Convert.ToString(dr["REQUEST_SUPPORT_DESCRIPTION"]),
                                            ReasonForComplaint = Convert.ToString(dr["REASON_FOR_COMPLAINT"]),
                                            Priority = Convert.ToString(dr["REQUEST_SUPPORT_PRIORITY"]),
                                            Origin = Convert.ToString(dr["REQUEST_SUPPORT_ORIGIN"]),
                                            ServiceGroup = Convert.ToString(dr["REQUEST_SUPPORT_SERVICE_GROUP"]),
                                            Scope = Convert.ToString(dr["REQUEST_SUPPORT_SCOPE"]),
                                            TransportServiceCode = Convert.ToString(dr["TRANSPORT_SERVICE_CODE"]),
                                            Status = new ZBaseResponse() 
                                            {
                                                Code = Convert.ToString(dr["REQUEST_SUPPORT_STATUS"]),
                                                Name = XData.GetNameRequestSupportStatus(Convert.ToString(dr["REQUEST_SUPPORT_STATUS"])),
                                            },
                                            PosC = Convert.ToString(dr["POS_C"]),
                                            UpdateInformation = Convert.ToString(dr["UPDATE_INFORMATION"]),
                                            CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
                                        },
                                        ShopInfo = new ZRequestSupportSearchInfoSendCMSShopInfoResponse() 
                                        {
                                            ID = Convert.ToInt32(dr["SHOP_ID"]),
                                            FullName = Convert.ToString(dr["SHOP_FULL_NAME"]),
                                            CrmOrPayPostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),
                                            EmailBCG = Convert.ToString(dr["EMAIL_BCG"]),
                                            EmailCSKH = Convert.ToString(dr["EMAIL_CSKH"]),
                                            AcceptancePosCode = Convert.ToString(dr["ACCEPTANCE_POSCODE"]),
                                        },
                                        EmployeeInfo = new ZRequestSupportSearchInfoSendCMSEmployeeInfoResponse() 
                                        {
                                            ID = Convert.ToInt32(dr["EMPLOYEE_ID"]),
                                            FullName = Convert.ToString(dr["EMPLOYEE_NAME"]),
                                            MobileNumber = Convert.ToString(dr["EMPLOYEE_PHONE"]),
                                            Email = Convert.ToString(dr["EMPLOYEE_EMAIL"]),
                                            Province = new ZBaseResponse() 
                                            {
                                                Name = Convert.ToString(dr["EMPLOYEE_PROVINCE"]),
                                            },
                                            District = new ZBaseResponse()
                                            {
                                                Name = Convert.ToString(dr["EMPLOYEE_DISTRICT"]),
                                            },
                                            Ward = new ZBaseResponse()
                                            {
                                                Name = Convert.ToString(dr["EMPLOYEE_WARD"]),
                                            },
                                            Street = Convert.ToString(dr["EMPLOYEE_STREET"]),
                                            FullAddress = Convert.ToString(dr["EMPLOYEE_FULL_ADDRESS"]),
                                        },
                                        UserInfo = new ZRequestSupportSearchInfoSendCMSUserInfoResponse() 
                                        {
                                            UserCMS = Convert.ToString(dr["USER_CMS"]),
                                        },
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Lấy dữ liệu thành công";
                                res.Data = JsonConvert.SerializeObject(item);
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.SearchInfoSendCMS: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse UpdateEditCODMoney(ZRequestSupportUpdateEditCODMoneyRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.UPDATE_EDIT_COD_MONEY", new object[]
                {
                    0,
                    req.RequestSupportID,
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
                                    res.Message = "Dữ liệu không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Mã bưu gửi không tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Mã đơn hàng không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Lưu lịch sử thất bại.";
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
            catch(Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.UpdateEditCODMoney: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse UpdateOrderInformation(ZRequestSupportUpdateOrderInformationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.UPDATE_ORDER_INFORMATION", new object[]
                {
                    0,
                    req.RequestSupportID,
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
                                    res.Message = "Yêu cầu hỗ trợ không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái không hợp lệ.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Thông tin cập nhật bị trống.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Mã bưu gửi không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Mã đơn hàng không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Loại yêu cầu hỗ trợ không tồn tại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Không thuộc loại yêu cầu hỗ trợ thay đổi thông tin đơn hàng.";
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Lưu lịch sử thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.UpdateOrderInformation: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        //public static ZResponse LockAcceptedStatus(ZRequestSupportLockAcceptedStatusRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.LOCK_ACCEPTED_STATUS", new object[]
        //        {
        //            0,
        //            req.RequestSupportID,
        //            req.ShopID,
        //            req.UpdatedBy
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Cập nhật thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Dữ liệu không tồn tại.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Trạng thái yêu cầu hỗ trợ không hợp lệ.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Loại yêu cầu không tồn tại.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Không thuộc loại yêu cầu sửa tiền thu hộ (COD).";
        //                            break;
        //                        case -5:
        //                            res.Code = "05";
        //                            res.Message = "Mã bưu gửi không tồn tại.";
        //                            break;
        //                        case -6:
        //                            res.Code = "06";
        //                            res.Message = "Mã đơn hàng không tồn tại.";
        //                            break;
        //                        case -7:
        //                            res.Code = "07";
        //                            res.Message = "Trạng thái bưu gửi không cho phép thay đổi COD.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.LockAcceptedStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        //public static ZResponse UnLockAcceptedStatus(ZRequestSupportUnLockAcceptedStatusRequest req)
        //{
        //    int result = -1;
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.UNLOCK_ACCEPTED_STATUS", new object[]
        //        {
        //            0,
        //            req.RequestSupportID,
        //            req.ShopID,
        //            req.UpdatedBy,
        //            req.Type
        //        }))
        //        {
        //            result = XHelper.ExecuteNonQuery(dbCommand);
        //            if (result >= 0)
        //            {
        //                result = Convert.ToInt32(dbCommand.Parameters[0].Value);
        //                if (result > 0)
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Cập nhật thành công.";
        //                }
        //                else
        //                {
        //                    switch (result)
        //                    {
        //                        case -1:
        //                            res.Code = "01";
        //                            res.Message = "Dữ liệu không tồn tại.";
        //                            break;
        //                        case -2:
        //                            res.Code = "02";
        //                            res.Message = "Mã bưu gửi không tồn tại.";
        //                            break;
        //                        case -3:
        //                            res.Code = "03";
        //                            res.Message = "Mã đơn hàng không tồn tại.";
        //                            break;
        //                        case -4:
        //                            res.Code = "04";
        //                            res.Message = "Lưu lịch sử thất bại.";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.UnLockAcceptedStatus: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        //    return res;
        //}

        public static ZResponse AcceptedStatus(ZRequestSupportAcceptedStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.ACCEPTED_STATUS", new object[]
                {
                    0,
                    req.RequestSupportID,
                    req.ShopID,
                    req.UserCMS,
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
                                    res.Message = "Tài khoản vận hành không tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.AcceptedStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ExistStatus(ZRequestSupportExistStatusRequest req)
        {
            int result = -1;
            ZResponse res = new ZResponse();
            try
            {
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.EXIST_STATUS", new object[]
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.ExistStatus: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SearchHistory(ZRequestSupportSearchHistoryRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZRequestSupportSearchHistoryResponse> listItem = new List<ZRequestSupportSearchHistoryResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQ_SUP_HISTORY_PKG.SEARCH_REQUEST_SUPPORT_HISTORY", new object[]
                {
                    req.RequestSupportID,
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
                                    listItem.Add(new ZRequestSupportSearchHistoryResponse()
                                    {
                                        Description = Convert.ToString(dr["DESCRIPTION"]),
                                        ResultDocument = Convert.ToString(dr["RESULT_DOCUMENT"]),
                                        Reason = Convert.ToString(dr["REASON"]),
                                        Status = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["STATUS"]),
                                            Name = XData.GetNameRequestSupportStatus(Convert.ToString(dr["STATUS"]))
                                        },
                                        CreatedName = Convert.ToString(dr["CREATED_NAME"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
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
                                res.Message = "Dữ liệu không tồn tại";
                            }
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.SearchHistory: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SearchProcess(ZRequestSupportSearchProcessRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZRequestSupportSearchProcessResponse> listItem = new List<ZRequestSupportSearchProcessResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQ_SUP_PROCESS_PKG.SEARCH_REQUEST_SUPPORT_PROCESS", new object[]
                {
                    req.RequestSupportID,
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
                                    listItem.Add(new ZRequestSupportSearchProcessResponse()
                                    {
                                        Description = Convert.ToString(dr["ACT_CONTENT"]),
                                        ResultDocument = Convert.ToString(dr["ACT_ATTACH_FILE"]),
                                        Result = Convert.ToString(dr["ACT_RESULT"]),
                                        Type = new ZBaseResponse()
                                        {
                                            Code = Convert.ToString(dr["ACT_TYPE"]),
                                            Name = XData.GetNameRequestSupportTypeStatus(Convert.ToString(dr["ACT_TYPE"]))
                                        },
                                        CreatedName = Convert.ToString(dr["CREATED_USER"]),
                                        CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
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
                                res.Message = "Dữ liệu không tồn tại";
                            }
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
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.SearchProcess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse AddProcess(ZRequestSupportAddProcessRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQ_SUP_PROCESS_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.RequestSupportCode,
                    req.ActID,
                    req.ActType,
                    req.ActContent,
                    req.ActAttachFile,
                    req.ActResult,
                    req.NextOrg,
                    req.NextOrgName,
                    req.RefOrg,
                    req.RefOrgName,
                    req.CreatedUser,
                    req.CreatedOrg,
                    req.CreatedDate,
                    req.ModifyDate,
                    req.AmndState,
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
                            res.Message = "Thêm tiến trình xử lý yêu cầu thành công.";
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
                                    res.Message = "Trạng thái của yêu cầu đã bị thay đổi.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.AddProcess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse UpdateAmndStateProcess(ZRequestSupportUpdateAmndStateProcessRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQ_SUP_PROCESS_PKG.UPDATE_AMND_STATE", new object[]
                {
                    0,
                    req.RequestSupportCode,
                    req.Status,
                    req.UpdatedBy,
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.UpdateAmndStateProcess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse AddRequestSupport(ZRequestSupportAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.ADD_REQUEST_SUPPORT", new object[]
                {
                    0,
                    req.EShippingCode,
                    req.RequestGroup,
                    req.RequestType,
                    req.Description,
                    req.Note,
                    req.UpdateInformation,
                    req.Document,
                    "Cao", //req.Priority,
                    "Web", //req.Origin,
                    "Bưu chính chuyển phát", //req.ServiceGroup,
                    "Trong nước", //req.Scope,
                    req.ShopID,
                    req.CreatedBy,
                    XHelper.DeviceName,
                    0,                       // P_OUT_CODE OUT VARCHAR,
                    0                        // P_OUT_REQUEST_SUPPORT_CODE OUT VARCHAR
                }))
                {
                    result = XHelper.ExecuteNonQuery(dbCommand);
                    if (result >= 0)
                    {
                        result = Convert.ToInt32(dbCommand.Parameters[0].Value);
                        if (result > 0)
                        {
                            res.Code = "00";
                            res.Message = "Tạo yêu cầu hỗ trợ thành công.";
                            res.Data = Convert.ToString(dbCommand.Parameters["P_OUT_REQUEST_SUPPORT_ID"].Value);
                        }
                        else
                        {
                            var OrderStatusName = XData.GetNameOrderStatus(Convert.ToString(dbCommand.Parameters["P_OUT_CURRENT_ORDER_STATUS"].Value));
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Mã bưu gửi không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lưu lịch sử thất bại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Đơn hàng không có dịch vụ thu hộ - chi hộ.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Mã đơn hàng không tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    //res.Message = "Trạng thái bưu gửi không cho phép thay đổi COD.";
                                    res.Message = "Loại yêu cầu không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Nhóm yêu cầu không tồn tại.";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = string.Format("Đơn hàng ở trạng thái {0} nên không thể tạo yêu cầu.", OrderStatusName);
                                    break;
                                case -8:
                                    res.Code = "08";
                                    res.Message = "Đơn hàng đã phát đủ số lần quy định, vui lòng liên hệ nhân viên CSKH để được hỗ trợ";
                                    break;
                                case -9:
                                    res.Code = "09";
                                    res.Message = "Số tiền COD tối đa 50 triệu đồng/BG.";
                                    break;
                                case -10:
                                    res.Code = "10";
                                    res.Message = "Chưa gán mã bưu cục thu gom cho shop. Vui lòng kiểm tra lại.";
                                    break;
                                case -11:
                                    res.Code = "11";
                                    res.Message = "Vui lòng gửi yêu cầu sửa tiền COD cho bưu gửi đầu tiên của lô.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.AddRequestSupport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        //public static ZResponse SearchInfoSendMail(ZRequestSupportSearchInfoSendMailRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        int result = -1;
        //        ZRequestSupportSearchInfoSendMailResponse item = null;
        //        using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.SEARCH_INFO_SEND_MAIL", new object[]
        //        {
        //            req.RequestSupportCode, //P_REQUEST_SUPPORT_CODE  VARCHAR
        //            req.ShopID,             //P_SHOP_ID               NUMBER
        //            0,                      //P_RETURN_CODE           OUT NUMBER
        //            0                       //P_OUT_CURSOR            OUT T_CURSOR
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
        //                            item = new ZRequestSupportSearchInfoSendMailResponse()
        //                            {
        //                                CustomerName = Convert.ToString(dr["CUSTOMER_NAME"]),
        //                                CrmOrPaypostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),
        //                                EmailBCG = Convert.ToString(dr["EMAIL_BCG"]),
        //                                EmailCSKH = Convert.ToString(dr["EMAIL_CSKH"]),
        //                                RequestSupportCode = Convert.ToString(dr["REQUEST_SUPPORT_CODE"]),
        //                                ShippingCode = Convert.ToString(dr["ESHIPPING_CODE"]),
        //                                RequestGroup = Convert.ToString(dr["REQUEST_GROUP"]),
        //                                RequestType = Convert.ToString(dr["REQUEST_TYPE"]),
        //                                Description = Convert.ToString(dr["DESCRIPTION"]),
        //                                CreatedDate = Convert.ToString(dr["CREATED_DATE"]),
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
        //                    res.Message = "Thất bại";
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
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.SearchInfoSendMail: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}

        public static ZResponse UpdateStatusSendMail(ZRequestSupportUpdateStatusSendMailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.UPDATE_STATUS_SEND_MAIL", new object[]
                {
                    0,
                    req.RequestSupportCode,
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
                            res.Message = "Gửi email thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.UpdateStatusSendMail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse UpdateStatusSendEMS(ZRequestSupportUpdateStatusSendEMSRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_REQUEST_SUPPORT_PKG.UPDATE_STATUS_SEND_EMS", new object[]
                {
                    0,
                    req.RequestSupportCode,
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
                            res.Message = "Gửi ems thành công.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "RequestSupportRepository.UpdateStatusSendEMS: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}