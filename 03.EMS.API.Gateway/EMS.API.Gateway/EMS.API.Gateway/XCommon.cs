using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
namespace EMS.API.Gateway
{
    public static class XCommon
    {
        //#region ZOHO CRM
        //public static string GetAccessTokenZoho()
        //{
        //    string Access_Token_Zoho = "";
        //    try
        //    {
        //        var res = XHelper.InvokeSyncGateway(new ZRequest()
        //        {
        //            Code = "ZOHO_ACCESS_TOKEN_GET",
        //            Data = ""
        //        });
        //        if (res.Code == "00")
        //        {
        //            var data = JsonConvert.DeserializeObject<ZApiZohoTokenRefreshResponse>(res.Data);
        //            if (data.error == null)
        //            {
        //                Access_Token_Zoho = data.access_token;
        //            }
        //            else
        //            {
        //                throw new Exception("XCommon.GetAccessTokenZoho: Lỗi hệ thống Zoho CRM");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("XCommon.GetAccessTokenZoho: Lỗi hệ thống Zoho CRM");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetAccessTokenZoho: " + ex.Message);
        //    }
        //    return Access_Token_Zoho;
        //}

        //public static XZohoModel GetZohoCRMLeads(string zohoToken, string Phone, string Email)
        //{
        //    XZohoModel zohoInfo = null;
        //    try
        //    {
        //        if (Phone != "")
        //        {
        //            var res = XHelper.InvokeSyncGateway(new ZRequest()
        //            {
        //                Code = "ZOHO_LEADS_PHONE_GET",
        //                Data = JsonConvert.SerializeObject(new ZApiZohoCRMLeadsRequest()
        //                {
        //                    zohoToken = zohoToken,
        //                    phone = Phone
        //                })
        //            });
        //            if (res.Code == "00")
        //            {
        //                var result = JsonConvert.DeserializeObject<ZApiZohoGetLeadsResponse>(res.Data);
        //                if (result.data != null)
        //                {
        //                    zohoInfo = new XZohoModel()
        //                    {
        //                        id = result.data[0].id,
        //                        FullName = result.data[0].FullName,
        //                        Phone = result.data[0].Phone,
        //                        Email = result.data[0].Email,
        //                        Company = result.data[0].Company
        //                    };
        //                }
        //            }
        //            else
        //            {
        //                XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.GetZohoCRMLeads: Phone=" + JsonConvert.SerializeObject(res));
        //            }
        //        }
        //        if (zohoInfo == null)
        //        {
        //            if (Email != "")
        //            {
        //                var res = XHelper.InvokeSyncGateway(new ZRequest()
        //                {
        //                    Code = "ZOHO_LEADS_EMAIL_GET",
        //                    Data = JsonConvert.SerializeObject(new ZApiZohoCRMLeadsRequest()
        //                    {
        //                        zohoToken = zohoToken,
        //                        phone = Email
        //                    })
        //                });
        //                if (res.Code == "00")
        //                {
        //                    var result = JsonConvert.DeserializeObject<ZApiZohoGetLeadsResponse>(res.Data);
        //                    if (result.data != null)
        //                    {
        //                        zohoInfo = new XZohoModel()
        //                        {
        //                            id = result.data[0].id,
        //                            FullName = result.data[0].FullName,
        //                            Phone = result.data[0].Phone,
        //                            Email = result.data[0].Email,
        //                            Company = result.data[0].Company
        //                        };
        //                    }
        //                }
        //                else
        //                {
        //                    //res.Message = "XCommon.GetZohoCRMLeadsPhone: Không tìm thấy khách hàng trên Zoho CRM";
        //                    XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetZohoCRMLeads: Email=" + JsonConvert.SerializeObject(res));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetZohoCRMLeads: " + ex.Message + " Data: " + Phone + "|" + Email);
        //    }
        //    return zohoInfo;
        //}

        //public static XZohoModel CreateZohoCRMLeads(string zohoToken, string Phone, string Email, string FullName, string Company)
        //{
        //    ZResponse res = new ZResponse();
        //    XZohoModel zohoInfo = new XZohoModel();
        //    ZZohoCRMCreateLeadsRequest req = new ZZohoCRMCreateLeadsRequest();
        //    List<ZZohoCRMDetailLeadsRequest> listtemp = new List<ZZohoCRMDetailLeadsRequest>();
        //    List<string> temp = new List<string>();
        //    temp.Add("approval");
        //    temp.Add("workflow");
        //    temp.Add("blueprint");

        //    listtemp.Add(new ZZohoCRMDetailLeadsRequest()
        //    {
        //        Company = Company,
        //        First_Name = "",
        //        Last_Name = FullName,
        //        Phone = Phone,
        //        Email = Email
        //    });
        //    req.data = listtemp;
        //    req.trigger = temp;
        //    req.zohoToken = zohoToken;
        //    try
        //    {
        //        res = XHelper.InvokeSyncGateway(new ZRequest()
        //        {
        //            Code = "ZOHO_LEADS_ADD",
        //            Data = JsonConvert.SerializeObject(req)
        //        });

        //        if (res.Code == "00")
        //        {
        //            var result = JsonConvert.DeserializeObject<ZApiZohoCreatResponse>(res.Data);
        //            if (result.data[0].code == "SUCCESS")
        //            {
        //                if (Phone != "" || Email != "")
        //                {
        //                    zohoInfo = GetZohoCRMLeads(zohoToken, Phone, Email);
        //                }
        //                else
        //                {
        //                    zohoInfo = new XZohoModel();
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception("XCommon.CreateZohoCRMLeads: Lỗi hệ thống Zoho CRM");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("XCommon.CreateZohoCRMLeads: Lỗi hệ thống Zoho CRM");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.CreateZohoCRMLeads: " + ex.Message + "|| request=: " + JsonConvert.SerializeObject(req) + "|| response=" + JsonConvert.SerializeObject(res));
        //    }
        //    return zohoInfo;
        //}
        //#endregion

        //#region Gửi y/c v/c
        //public static ZResponse SendRequestTransport(ZOrderSearchInfoSendRequestTransportResponse dataSearchInfo, int UpdatedBy)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var CrmOrPayPostCode = ""; var ShopPhone = ""; var ShopFullName = ""; var ShopEmail = "";
        //        if (dataSearchInfo.CrmOrPayPostCode == "")
        //        {
        //            var zohoToken = GetAccessTokenZoho();
        //            XZohoModel ZohoInfo = new XZohoModel();
        //            if (zohoToken != "")
        //            {
        //                ZohoInfo = GetZohoCRMLeads(zohoToken, dataSearchInfo.ShopPhone, dataSearchInfo.ShopEmail);
        //                if (ZohoInfo == null)
        //                {
        //                    ZohoInfo = CreateZohoCRMLeads(zohoToken, dataSearchInfo.ShopPhone, dataSearchInfo.ShopEmail, dataSearchInfo.ShopFullName, dataSearchInfo.ShopStoreName);
        //                }
        //                if (ZohoInfo != null)
        //                {
        //                    CrmOrPayPostCode = ZohoInfo.id; //dataSearchInfo.CrmOrPayPostCode;
        //                    ShopPhone = ZohoInfo.Phone;
        //                    ShopFullName = ZohoInfo.FullName;
        //                    ShopEmail = ZohoInfo.Email;
        //                }
        //                else
        //                {
        //                    res.Code = "03";
        //                    res.Message = "Không lấy được thông tin khách hàng từ ZohoCRM.";
        //                }
        //            }
        //            else
        //            {
        //                res.Code = "02";
        //                res.Message = "Lỗi lấy AccessToken ZohoCRM.";
        //            }
        //        }
        //        else
        //        {
        //            CrmOrPayPostCode = dataSearchInfo.CrmOrPayPostCode;
        //            ShopPhone = dataSearchInfo.ShopPhone;
        //            ShopFullName = dataSearchInfo.ShopFullName;
        //            ShopEmail = dataSearchInfo.ShopEmail;
        //        }

        //        var reqTrackingCode = new ZTransportGetEmsBarCodeRequest()
        //        {
        //            CustomerCode = CrmOrPayPostCode,
        //            ProvinceID = dataSearchInfo.SenderProvinceID,
        //            OrderCode = dataSearchInfo.EMSOneCode
        //        };

        //        var resTrackingCode = XHelper.Invoke("POST", "Transport/GetEmsBarCode", reqTrackingCode);

        //        if (resTrackingCode.Code == "00")
        //        {
        //            List<ZListItemInfoShippingRequest> TransportInfoShopDetail = new List<ZListItemInfoShippingRequest>();
        //            TransportInfoShopDetail.Add(new ZListItemInfoShippingRequest()
        //            {
        //                TRACKING_CODE = resTrackingCode.Data,
        //                SENDER_NAME = dataSearchInfo.SenderName,
        //                SENDER_CODE = CrmOrPayPostCode,
        //                SENDER_PHONE = dataSearchInfo.SenderPhone,
        //                SENDER_STREET = dataSearchInfo.SenderFullAddress, //SenderStreet
        //                SENDER_ADDRESS = dataSearchInfo.SenderFullAddress,
        //                SENDER_PROVINCE_ID = dataSearchInfo.SenderProvinceID.ToString(),
        //                SENDER_DISTRICT_ID = dataSearchInfo.SenderDistrictID.ToString(),
        //                SENDER_WARD_ID = dataSearchInfo.SenderWardID.ToString(),
        //                SENDER_EMAIL = dataSearchInfo.SenderEmail,
        //                RECEIVER_NAME = dataSearchInfo.ReceiverName,
        //                RECEIVER_PROVINCE_ID = dataSearchInfo.ReceiverProvinceID.ToString(),
        //                RECEIVER_PHONE = dataSearchInfo.ReceiverPhone,
        //                RECEIVER_DISTRICT_ID = dataSearchInfo.ReceiverDistrictID.ToString(),
        //                RECEIVER_WARD_ID = dataSearchInfo.ReceiverWardID.ToString(),
        //                RECEIVER_EMAIL = "",
        //                RECEIVER_STREET = dataSearchInfo.ReceiverFullAddress, //ReceiverFullAddress
        //                RECEIVER_ADDRESS = dataSearchInfo.ReceiverFullAddress,
        //                PRODUCT_NAME = dataSearchInfo.ProductName,
        //                PRODUCT_VALUE = Convert.ToInt32(dataSearchInfo.ProductValue),
        //                PRODUCT_CODE = dataSearchInfo.ProductCode,
        //                PRODUCT_DESCRIPTION = dataSearchInfo.ProductDescription,
        //                PRODUCT_QUANTITY = dataSearchInfo.ProductQuantity,
        //                WEIGHT = dataSearchInfo.Weight,
        //                ADDITION_SERVICE = dataSearchInfo.AdditionService,
        //                SPECIAL_SERVICE = dataSearchInfo.EspeciallyServiceCode,
        //                TOTAL_AMOUNT = dataSearchInfo.TotalAmount,
        //                CUSTOMER_CODE = dataSearchInfo.CrmOrPayPostCode,
        //                TO_COUNTRY = "VN",
        //                PO_CREATE = dataSearchInfo.POCode == "" ? "0100" : dataSearchInfo.POCode,
        //                COMPLETE = 0,
        //                POSTAL_PROVINCE_CODE = dataSearchInfo.ReceiverProvinceID.ToString(),
        //                POSTAL_DISTRICT_CODE = dataSearchInfo.ReceiverDistrictID.ToString(),
        //                POSTAL_COMMUNE_CODE = dataSearchInfo.ReceiverWardID.ToString(),
        //                POSTAL_CODE = "",
        //                SERVICE_TYPE = dataSearchInfo.EspeciallyServiceType
        //            });
        //            ZTransportInfoShippingApiRequest reqPickupCreate = new ZTransportInfoShippingApiRequest()
        //            {
        //                PICKUP_CODE_API = "",
        //                PO_CODE = dataSearchInfo.POCode,
        //                DESCRIPTION = "",
        //                CUSTOMER_CODE = CrmOrPayPostCode,
        //                CUSTOMER_PHONE = ShopPhone,
        //                CUSTOMER_NAME = ShopFullName,
        //                CUSTOMER_EMAIL = ShopEmail,
        //                STOREID = "",
        //                PICKUP_PROVINCE_ID = dataSearchInfo.SenderProvinceID,
        //                PICKUP_DISTRICT_ID = dataSearchInfo.SenderDistrictID,
        //                PICKUP_WARD_ID = dataSearchInfo.SenderWardID,
        //                PICKUP_STREET = dataSearchInfo.SenderFullAddress, //SenderStreet
        //                PICKUP_DATE = DateTime.Now.ToString("yyyyMMdd"),
        //                PICKUP_HOURS = DateTime.Now.ToString("HHmm"),
        //                CONTACT_NAME = dataSearchInfo.SenderName,
        //                CONTACT_PHONE = dataSearchInfo.SenderPhone,
        //                PICKUP_POSTMAN = "",
        //                PICKUP_POCODE = "",
        //                PICKUP_CONFIG = dataSearchInfo.CollectionType == 2 ? "1" : "0",
        //                //EMS: 1: thu gom, 2: gui hang
        //                //PNS: 0: thu gom, 1: gui hang
        //                SignatureCode = Security.SHA256(CrmOrPayPostCode + XHelper.EmsApiKey).ToUpper(),
        //                LIST_ITEM = TransportInfoShopDetail
        //            };

        //            var resPickupCreate = XHelper.InvokeSyncGateway(new ZRequest()
        //            {
        //                Code = "PNS_EMS_PICKUP_CREATE",
        //                Data = JsonConvert.SerializeObject(reqPickupCreate)
        //            });

        //            if (resPickupCreate.Code == "00")
        //            {
        //                var reqTransportUpdateOrder = new ZTransportUpdateOrderRequest()
        //                {
        //                    OrderCode = dataSearchInfo.EMSOneCode,
        //                    Status = "W",
        //                    UpdatedBy = UpdatedBy
        //                };

        //                var resTransportUpdateOrder = XHelper.Invoke("POST", "Transport/UpdateStatusTransportOrder", reqTransportUpdateOrder);

        //                if (resTransportUpdateOrder.Code == "00")
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Tạo đơn hàng và gửi vận chuyển thành công";
        //                }
        //                else
        //                {
        //                    res.Code = resTransportUpdateOrder.Code;
        //                    res.Message = resTransportUpdateOrder.Message;
        //                    XHelper.LogToFile(LogFileType.MESSAGE, "OrderRepository.AddOrderAndSendTransport: request=" + JsonConvert.SerializeObject(reqTransportUpdateOrder) + "|| response=" + JsonConvert.SerializeObject(resTransportUpdateOrder));
        //                }
        //            }
        //            else
        //            {
        //                XHelper.LogToFile(LogFileType.MESSAGE, "OrderRepository.AddOrderAndSendTransport: request=" + JsonConvert.SerializeObject(reqPickupCreate) + "|| response=" + JsonConvert.SerializeObject(resPickupCreate));
        //                res.Code = "99";
        //                res.Message = "Lỗi kết nối API vận chuyển, vui lòng liên hệ quản trị.";
        //            }
        //        }
        //        else
        //        {
        //            res.Code = resTrackingCode.Code;
        //            res.Message = resTrackingCode.Message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SendRequestTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}
        //#endregion

        //#region Huỷ y/c v/c
        //public static ZResponse CancelRequestTransport(ZTransportShippingCancelDataRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.InvokeSyncGateway(new ZRequest()
        //        {
        //            Code = "PNS_EMS_PICKUP_CANCEL",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.CancelRequestTransport: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}
        //#endregion

    }
}