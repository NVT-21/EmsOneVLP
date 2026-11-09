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
    public class ShopRepository
    {
        public static ZResponse AddShop(ZShopAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                var oPassword = Security.SHA256(req.MobileNumber + req.Password + Security.PASSWORD_SALT);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.ADD_NEW", new object[]
                {
                    0,
                    req.ContractNumber,
                    req.CustomerCode,
                    req.CustomerToken,
                    req.FullName,               //P_FULL_NAME
                    req.StoreName,              //P_STORE_NAME
                    req.Email,                  //P_EMAIL
                    req.MobileNumber,           //P_MOBILE_NUMBER
                    oPassword,                  //P_PASSWORD
                    req.ProductBusiness,        //P_PRODUCT_BUSINESS
                    req.TypeBusiness,           //P_TYPE_BUSINESS
                    req.TypeShop,               //P_TYPE_SHOP
                    req.AccountType,
                    req.NumberOfEmployees,
                    req.ProvinceID,             //P_PROVINCE_ID
                    req.DistrictID,             //P_DISTRICT_ID
                    req.WardID,                 //P_WARD_ID
                    req.Street,                 //P_STREET
                    req.MainTransportServiceID,
                    req.ExtraTransportServiceID,
                    //req.Address,              //P_ADDRESS
                    req.CollectionType,
                    req.CollectionPostOfficeID,
                    req.ChildCustomerCode,
                    req.CODPeriodType,
                    req.FeePeriodType,
                    req.PaymentMethod,
                    req.FullNameOfPayment,
                    req.NameOfPayment,
                    req.BranchOfPayment,
                    req.NumberOfPayment,
                    req.EmailBCG,
                    req.EmailCSKH,
                    req.TaxCode,
                    req.PartnerID,
                    req.CreatedBy,
                    0
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
                            res.Data = dbCommand.Parameters["P_OUT_SHOP_ID"].Value.ToString();
                        }
                        else
                        {
                            switch (result)
                            {
                                case -1:
                                    res.Code = "01";
                                    res.Message = "Email đã tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Số điện thoại đã tồn tại.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Số hợp đồng đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Mã CMR/ mã Paypost đã tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Bưu cục thu gom không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Token đã tồn tại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopController.Add: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ApprovedShop(ZShopApprovedShopRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                string oPassword = Security.SHA256(req.MobileNumber + req.Password + Security.PASSWORD_SALT);
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.APPROVED_SHOP", new object[]
                {
                    0,
                    req.ID,                     //P_ID
                    oPassword,                  //P_PASSWORD
                    req.Email,
                    req.ContractNumber,
                    req.CustomerCode,
                    req.CustomerToken,
                    req.CollectionType,
                    req.CollectionPostOfficeID, //P_COLLECTION_POST_OFFICE_ID   NUMBER
                    req.ChildCustomerCode,
                    req.AccountType,            //P_ACCOUNT_TYPE
                    req.NumberOfEmployees,      //P_NUMBER_OF_EMPLOYEES
                    req.MainTransportServiceID,
                    req.ExtraTransportServiceID,
                    req.CODPeriodType,
                    req.FeePeriodType,
                    req.PaymentMethod,
                    req.FullNameOfPayment,
                    req.NameOfPayment,
                    req.BranchOfPayment,
                    req.NumberOfPayment,
                    req.EmailBCG,
                    req.EmailCSKH,
                    req.TaxCode,
                    req.PartnerID,
                    req.ConfirmedBy             //P_CONFIRMED_BY
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
                                    res.Message = "Thông tin đăng ký không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Trạng thái thông tin đăng ký đã thay đổi.";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Số hợp đồng đã tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Mã CMR/ mã Paypost đã tồn tại.";
                                    break;
                                case -5:
                                    res.Code = "05";
                                    res.Message = "Bưu cục thu gom không tồn tại.";
                                    break;
                                case -6:
                                    res.Code = "06";
                                    res.Message = "Email đã tồn tại";
                                    break;
                                case -7:
                                    res.Code = "07";
                                    res.Message = "Token đã tồn tại";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.ApprovedShop: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse UpdateStatusSendMail(ZShopUpdateConfirmationEmailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.UPDATE_CONFIRMATION_EMAIL", new object[]
                {
                    0,
                    req.ID
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
                                    res.Message = "Không tìm thấy thông tin cập nhật";
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
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.UpdateStatusSendMail: " + ex.Message);
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse SendEmailRegisterSuccess(ZShopSearchInfoSendEmailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var link = Security.MD5Encrypt(JsonConvert.SerializeObject(new
                {
                    MobileNumber = req.MobileNumber,
                    Password = req.Password
                }), Security.PASSWORD_SALT);

                var stringBody = "";
                stringBody = string.Format("<div>Chào {0},", req.FullName);
                stringBody += "<p>Bạn đã được cấp tài khoản trên hệ thống EMSOne. Thông tin đăng nhập như sau:</p>";
                stringBody += string.Format("<p>Số điện thoại đăng nhập: {0}</p>", req.MobileNumber);
                stringBody += string.Format("<p>Truy cập vào đây để tạo mật khẩu và sử dụng: <a href={0}Account/UpdatePasswordView?data={1}>tại đây</a> !</p>", XHelper.FrontEndURL, link);
                stringBody += "<p>Trân trọng cảm ơn!</p></div>";

                var to = new List<ZZohoSendMailToRequest>();
                to.Add(new ZZohoSendMailToRequest()
                {
                    email_address = new ZZohoSendMailFromRequest()
                    {
                        address = req.ToEmail,
                        name = req.FullName
                    }
                });

                ZZohoSendMailRequest reqSendMail = new ZZohoSendMailRequest()
                {
                    to = to,
                    subject = "EMSONE - KÍCH HOẠT TÀI KHOẢN",
                    htmlbody = stringBody
                };

                var resSendMail = ZohoRepository.SendMail(reqSendMail);

                if (resSendMail.Code == "00")
                {
                    ZShopUpdateConfirmationEmailRequest reqUpdateStatusSendMail = new ZShopUpdateConfirmationEmailRequest()
                    {
                        ID = req.ShopID,
                    };
                    var resUpdateStatusSendMail = UpdateStatusSendMail(reqUpdateStatusSendMail);

                    if (resUpdateStatusSendMail.Code == "00")
                    {
                        res.Code = "00";
                        res.Message = "Duyệt và gửi email xác nhận thành công.";
                    }
                    else
                    {
                        res.Code = "02";
                        res.Message = "Duyệt và đã gửi email xác nhận.";
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ShopRepository.UpdateStatusSendMail: Cập nhật trạng thái gửi email thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqUpdateStatusSendMail), JsonConvert.SerializeObject(resUpdateStatusSendMail)));
                    }
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Duyệt thành công nhưng gửi email xác nhận thất bại. Vui lòng liên hệ quản trị.";
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ShopRepository.SendEmailRegisterSuccess: Gửi email thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqSendMail), JsonConvert.SerializeObject(resSendMail)));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.SendEmailRegisterSuccess: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SendEmailForgetPassword(ZShopSearchInfoSendEmailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                //var link = Security.MD5Encrypt(JsonConvert.SerializeObject(new
                //{
                //    MobileNumber = req.MobileNumber,
                //    Password = req.Password
                //}), Security.PASSWORD_SALT);

                //var stringBody = string.Format("<div>Vui lòng click vào <a href={0}Account/UpdateForgetPasswordView?data={1}>đây</a> để đổi lại mật khẩu trên EMSOne </div>", XHelper.FrontEndURL, link);

                var stringBody = "";
                stringBody = "<div>";
                stringBody += string.Format("Xin chào {0},", req.StoreName);
                stringBody += "<p>Vui lòng nhập mã xác thực dưới đây để tiếp tục đổi lại mật khẩu trên EMSOne</p>";
                stringBody += string.Format("<p>{0}</p>", req.OtpCode);
                stringBody += "<p>Được gửi từ EMSOne</p>";
                stringBody += "</div>";

                var to = new List<ZZohoSendMailToRequest>();
                to.Add(new ZZohoSendMailToRequest()
                {
                    email_address = new ZZohoSendMailFromRequest()
                    {
                        address = req.ToEmail,
                        name = req.FullName
                    }
                });

                ZZohoSendMailRequest reqSendMail = new ZZohoSendMailRequest()
                {
                    to = to,
                    subject = "[Quên mật khẩu EMSOne] Gửi mã xác thực tài khoản",
                    htmlbody = stringBody
                };

                var resSendMail = ZohoRepository.SendMail(reqSendMail);

                if (resSendMail.Code == "00")
                {
                    res.Code = "00";
                    res.Message = "Gửi email thành công.";
                }
                else
                {
                    res.Code = "01";
                    res.Message = "Gửi email thất bại. Vui lòng liên hệ quản trị.";
                    XHelper.LogToFile(LogFileType.MESSAGE, string.Format("ShopRepository.SendEmailForgetPassword: Gửi email thất bại.||request={0}||response={1}", JsonConvert.SerializeObject(reqSendMail), JsonConvert.SerializeObject(resSendMail)));
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.SendEmailForgetPassword: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetByCrmOrPaypostCode(string CrmOrPaypostCode)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZShopGetByCrmOrPaypostCodeResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.GET_BY_CRM_OR_PAYPOST_CODE", new object[]
                {
                    CrmOrPaypostCode,
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
                                    item = new ZShopGetByCrmOrPaypostCodeResponse()
                                    {
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        CrmOrPaypostCode = Convert.ToString(dr["CRM_OR_PAYPOST_CODE"]),
                                        CollectionType = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                        TypeShop = Convert.ToInt32(dr["TYPE_SHOP"]),
                                        DefaultWareHouseID = Convert.ToInt32(dr["DEFAULT_WAREHOUSE_ID"]),
                                        StoreOwnerID = Convert.ToInt32(dr["STORE_OWNER_ID"]),
                                        PickUpAddressID = Convert.ToInt32(dr["PICKUP_ADDRESS_ID"]),
                                        PickUpAddressFullName = Convert.ToString(dr["PICKUP_ADDRESS_FULL_NAME"]),
                                        PickUpAddressMobileNumber = Convert.ToString(dr["PICKUP_ADDRESS_MOBILE_NUMBER"]),
                                        PickUpAddressProvinceID = Convert.ToInt32(dr["PICKUP_ADDRESS_PROVINCE_ID"]),
                                        PickUpAddressDistrictID = Convert.ToInt32(dr["PICKUP_ADDRESS_DISTRICT_ID"]),
                                        PickUpAddressWardID = Convert.ToInt32(dr["PICKUP_ADDRESS_WARD_ID"]),
                                        PickUpAddressStreet = Convert.ToString(dr["PICKUP_ADDRESS_STREET"]),
                                    };
                                }
                            }
                            if (item != null)
                            {
                                res.Code = "00";
                                res.Message = "Thành công.";
                                res.Data = JsonConvert.SerializeObject(item);
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Không tồn tại dữ liệu.";
                                res.Data = null;
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.GetByCrmOrPaypostCode: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse ForgetPasswordByEmail(ZShopForgetPasswordByEmailRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                ZShopForgetPasswordByEmailResponse item = null;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_SHOP_PKG.FORGET_PASSWORD_BY_EMAIL", new object[]
                {
                    req.Email,
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
                                    item = new ZShopForgetPasswordByEmailResponse()
                                    {
                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        StoreName = Convert.ToString(dr["STORE_NAME"]),
                                        FullName = Convert.ToString(dr["FULL_NAME"]),
                                        Email = Convert.ToString(dr["EMAIL"]),
                                        EmployeeID = Convert.ToInt32(dr["EMPLOYEE_ID"]),
                                    };
                                }
                            }
                            if(item != null)
                            {
                                var otpCode = XData.RandomString(6);
                                var resAddOtp = OtpRepository.AddOtp(new ZOtpAddRequest() 
                                {
                                    OtpCode = otpCode,
                                    OtpType = 1,
                                    EmployeeID = item.EmployeeID
                                });

                                if(resAddOtp.Code == "00")
                                {
                                    item.OtpID = Convert.ToInt32(resAddOtp.Data);
                                    item.OtpCode = otpCode;

                                    res.Code = "00";
                                    res.Message = "Thành công.";
                                    res.Data = JsonConvert.SerializeObject(item);
                                }
                                else
                                {
                                    res.Code = "02";
                                    res.Message = "Tạo mã xác thực thất bại.";
                                }
                            }
                            else
                            {
                                res.Code = "01";
                                res.Message = "Thông tin Email không khớp với thông tin đăng ký, vui lòng kiểm tra lại.";
                            }
                        }
                        else
                        {
                            res.Code = "99";
                            res.Message = "Thất bại.";

                            //switch (result)
                            //{
                            //    case -1:
                            //        res.Code = "01";
                            //        res.Message = "Thông tin Email không khớp với thông tin đăng ký, vui lòng kiểm tra lại.";
                            //        break;
                            //    default:
                            //        res.Code = "99";
                            //        res.Message = "Thất bại.";
                            //        break;
                            //}
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopRepository.ForgetPasswordByEmail: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}