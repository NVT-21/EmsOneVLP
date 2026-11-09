using EMS.Core;
using EMS.Library;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Zen.Barcode;
using EMS.Entity;
using Newtonsoft.Json;
using EMS.API.Core.Repositories;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace EMS.API.Core
{
    public static class XCommon
    {
        #region ZOHO CRM
        public static string GetAccessTokenZoho()
        {
            string Access_Token_Zoho = "";
            try
            {
                var res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "ZOHO_ACCESS_TOKEN_GET",
                    Data = ""
                });
                if (res.Code == "00")
                {
                    var data = JsonConvert.DeserializeObject<ZApiZohoTokenRefreshResponse>(res.Data);
                    if (data.error == null)
                    {
                        Access_Token_Zoho = data.access_token;
                    }
                    else
                    {
                        throw new Exception("XCommon.GetAccessTokenZoho: Lỗi hệ thống Zoho CRM");
                    }
                }
                else
                {
                    throw new Exception("XCommon.GetAccessTokenZoho: Lỗi hệ thống Zoho CRM");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetAccessTokenZoho: " + ex.Message);
            }
            return Access_Token_Zoho;
        }

        public static XZohoModel GetZohoCRMLeads(string zohoToken, string Phone, string Email)
        {
            XZohoModel zohoInfo = null;
            try
            {
                if (Phone != "")
                {
                    var res = XHelper.InvokeSyncGateway(new ZRequest()
                    {
                        Code = "ZOHO_LEADS_PHONE_GET",
                        Data = JsonConvert.SerializeObject(new ZApiZohoCRMLeadsRequest()
                        {
                            zohoToken = zohoToken,
                            phone = Phone
                        })
                    });
                    if (res.Code == "00")
                    {
                        var result = JsonConvert.DeserializeObject<ZApiZohoGetLeadsResponse>(res.Data);
                        if (result.data != null)
                        {
                            zohoInfo = new XZohoModel()
                            {
                                id = result.data[0].id,
                                FullName = result.data[0].FullName,
                                Phone = result.data[0].Phone,
                                Email = result.data[0].Email,
                                Company = result.data[0].Company
                            };
                        }
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.GetZohoCRMLeads: Phone=" + JsonConvert.SerializeObject(res));
                    }
                }
                if (zohoInfo == null)
                {
                    if (Email != "")
                    {
                        var res = XHelper.InvokeSyncGateway(new ZRequest()
                        {
                            Code = "ZOHO_LEADS_EMAIL_GET",
                            Data = JsonConvert.SerializeObject(new ZApiZohoCRMLeadsRequest()
                            {
                                zohoToken = zohoToken,
                                phone = Email
                            })
                        });
                        if (res.Code == "00")
                        {
                            var result = JsonConvert.DeserializeObject<ZApiZohoGetLeadsResponse>(res.Data);
                            if (result.data != null)
                            {
                                zohoInfo = new XZohoModel()
                                {
                                    id = result.data[0].id,
                                    FullName = result.data[0].FullName,
                                    Phone = result.data[0].Phone,
                                    Email = result.data[0].Email,
                                    Company = result.data[0].Company
                                };
                            }
                        }
                        else
                        {
                            //res.Message = "XCommon.GetZohoCRMLeadsPhone: Không tìm thấy khách hàng trên Zoho CRM";
                            XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetZohoCRMLeads: Email=" + JsonConvert.SerializeObject(res));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetZohoCRMLeads: " + ex.Message + " Data: " + Phone + "|" + Email);
            }
            return zohoInfo;
        }

        public static XZohoModel CreateZohoCRMLeads(string zohoToken, string Phone, string Email, string FullName, string Company)
        {
            ZResponse res = new ZResponse();
            XZohoModel zohoInfo = new XZohoModel();
            ZZohoCRMCreateLeadsRequest req = new ZZohoCRMCreateLeadsRequest();
            List<ZZohoCRMDetailLeadsRequest> listtemp = new List<ZZohoCRMDetailLeadsRequest>();
            List<string> temp = new List<string>();
            temp.Add("approval");
            temp.Add("workflow");
            temp.Add("blueprint");

            listtemp.Add(new ZZohoCRMDetailLeadsRequest()
            {
                Company = Company,
                First_Name = "",
                Last_Name = FullName,
                Phone = Phone,
                Email = Email
            });
            req.data = listtemp;
            req.trigger = temp;
            req.zohoToken = zohoToken;
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "ZOHO_LEADS_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });

                if (res.Code == "00")
                {
                    var result = JsonConvert.DeserializeObject<ZApiZohoCreatResponse>(res.Data);
                    if (result.data[0].code == "SUCCESS")
                    {
                        if (Phone != "" || Email != "")
                        {
                            zohoInfo = GetZohoCRMLeads(zohoToken, Phone, Email);
                        }
                        else
                        {
                            zohoInfo = new XZohoModel();
                        }
                    }
                    else
                    {
                        throw new Exception("XCommon.CreateZohoCRMLeads: Lỗi hệ thống Zoho CRM");
                    }
                }
                else
                {
                    throw new Exception("XCommon.CreateZohoCRMLeads: Lỗi hệ thống Zoho CRM");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.CreateZohoCRMLeads: " + ex.Message + "|| request=: " + JsonConvert.SerializeObject(req) + "|| response=" + JsonConvert.SerializeObject(res));
            }
            return zohoInfo;
        }
        #endregion

        #region Gửi yêu cầu vận chuyển qua API PNS_EMS_PICKUP_CREATE
        //public static ZResponse SendRequestTransport(ZOrderSearchInfoSendRequestTransportResponse dataSearchInfo, int UpdatedBy)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var CrmOrPayPostCode = ""; var ShopPhone = ""; var ShopFullName = ""; var ShopEmail = "";
        //        if (string.IsNullOrEmpty(dataSearchInfo.CrmOrPayPostCode))
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

        //        var resTrackingCode = TransportRepository.GetEmsBarCode(new ZTransportGetEmsBarCodeRequest()
        //        {
        //            CustomerCode = CrmOrPayPostCode,
        //            ProvinceID = dataSearchInfo.SenderProvinceID,
        //            OrderCode = dataSearchInfo.EMSOneCode
        //        });

        //        if (resTrackingCode.Code == "00")
        //        {
        //            if (dataSearchInfo.EspeciallyServiceCode == "TMD_B")
        //            {
        //                dataSearchInfo.EspeciallyServiceCode = "TMD";
        //            }

        //            var Description = "";
        //            if (!string.IsNullOrEmpty(dataSearchInfo.ProductDescription) && !string.IsNullOrEmpty(dataSearchInfo.ProductNote))
        //            {
        //                Description = dataSearchInfo.ProductDescription + " || " + dataSearchInfo.ProductNote;
        //            }
        //            else
        //            {
        //                Description = string.IsNullOrEmpty(dataSearchInfo.ProductDescription) ? dataSearchInfo.ProductNote : dataSearchInfo.ProductDescription;
        //            }

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
        //                PRODUCT_DESCRIPTION = Description,
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
        //                SERVICE_TYPE = dataSearchInfo.EspeciallyServiceType,
        //                SERVICE_FEE = dataSearchInfo.ServiceFee,
        //                CLASSIFY = dataSearchInfo.Classification.Code,
        //                IS_CONSIGNMENT = Convert.ToInt32(dataSearchInfo.Consignment.Code)
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
        //                PICKUP_POCODE = dataSearchInfo.CollectionPostOfficeCode,
        //                PICKUP_CONFIG = dataSearchInfo.CollectionType == 2 ? "1" : "0",
        //                //EMS: 1: thu gom, 2: gui hang
        //                //PNS: 0: thu gom, 1: gui hang
        //                SignatureCode = Security.SHA256(CrmOrPayPostCode + XHelper.EMSTransportKey).ToUpper(),
        //                LIST_ITEM = TransportInfoShopDetail
        //            };

        //            var resPickupCreate = EMSRepository.SendRequestTransport(reqPickupCreate);

        //            if (resPickupCreate.Code == "00")
        //            {
        //                var resTransportUpdateOrder = OrderRepository.SendTransportStatus(new ZOrderSendTransportStatusRequest()
        //                {
        //                    EMSOneCode = dataSearchInfo.EMSOneCode,
        //                    Status = "W",
        //                    UpdatedBy = UpdatedBy
        //                });

        //                if (resTransportUpdateOrder.Code == "00")
        //                {
        //                    res.Code = "00";
        //                    res.Message = "Tạo đơn hàng và gửi vận chuyển thành công";
        //                    res.Data = JsonConvert.SerializeObject(new { EMSOneCode = dataSearchInfo.EMSOneCode });
        //                }
        //                else
        //                {
        //                    res.Code = resTransportUpdateOrder.Code;
        //                    res.Message = resTransportUpdateOrder.Message;
        //                    XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.SendRequestTransport: request=" + JsonConvert.SerializeObject(new { OrderCode = dataSearchInfo.EMSOneCode, Status = "W", UpdatedBy = UpdatedBy }) + "|| response=" + JsonConvert.SerializeObject(resTransportUpdateOrder));
        //                }
        //            }
        //            else
        //            {
        //                XHelper.LogToFile(LogFileType.MESSAGE, "XCommon.SendRequestTransport: request=" + JsonConvert.SerializeObject(reqPickupCreate) + "|| response=" + JsonConvert.SerializeObject(resPickupCreate));
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

        public static ZResponse SendTransportOrder(string AmndState, int OrderID, int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resSearchInfoSendTransport = OrderRepository.SendTransportInfo(new ZOrderSendTransportInfoRequest()
                {
                    OrderID = OrderID,
                    ShopID = ShopID,
                    AmndState = AmndState
                });
                if (resSearchInfoSendTransport.Code == "00")
                {
                    var dataSearchInfo = JsonConvert.DeserializeObject<ZOrderSendTransportInfoResponse>(resSearchInfoSendTransport.Data);

                    if (dataSearchInfo.EspeciallyServiceCode == "TMD_B" || dataSearchInfo.EspeciallyServiceCode == "TMDHCM")
                    {
                        dataSearchInfo.EspeciallyServiceCode = "TMD";
                    }

                    if (dataSearchInfo.TransportPayer == 2 && dataSearchInfo.AdditionService.Contains("COD") == true)
                    {
                        dataSearchInfo.AdditionService += ",CVS";
                    }

                    var Description = "";
                    if (!string.IsNullOrEmpty(dataSearchInfo.ProductDescription) && !string.IsNullOrEmpty(dataSearchInfo.ProductNote))
                    {
                        Description = dataSearchInfo.ProductDescription + " || " + dataSearchInfo.ProductNote;
                    }
                    else
                    {
                        Description = string.IsNullOrEmpty(dataSearchInfo.ProductDescription) ? dataSearchInfo.ProductNote : dataSearchInfo.ProductDescription;
                    }

                    var ProductName = dataSearchInfo.ProductName;
                    if (string.IsNullOrEmpty(ProductName))
                    {
                        ProductName = dataSearchInfo.ProductItem;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dataSearchInfo.ProductItem))
                        {
                            ProductName = dataSearchInfo.ProductItem + " + " + dataSearchInfo.ProductName;
                        }
                    }

                    List<ZListItemInfoShippingRequest> TransportInfoShopDetail = new List<ZListItemInfoShippingRequest>();
                    TransportInfoShopDetail.Add(new ZListItemInfoShippingRequest()
                    {
                        TRACKING_CODE = dataSearchInfo.ShippingCode,
                        SENDER_NAME = dataSearchInfo.SenderName,
                        SENDER_CODE = dataSearchInfo.CrmOrPayPostCode,
                        SENDER_PHONE = dataSearchInfo.SenderPhone,
                        SENDER_STREET = dataSearchInfo.SenderFullAddress, //SenderStreet
                        SENDER_ADDRESS = dataSearchInfo.SenderFullAddress,
                        SENDER_PROVINCE_ID = dataSearchInfo.SenderProvinceID.ToString(),
                        SENDER_DISTRICT_ID = dataSearchInfo.SenderDistrictID.ToString(),
                        SENDER_WARD_ID = dataSearchInfo.SenderWardID.ToString(),
                        SENDER_EMAIL = dataSearchInfo.SenderEmail,
                        RECEIVER_NAME = dataSearchInfo.ReceiverName,
                        RECEIVER_PROVINCE_ID = dataSearchInfo.ReceiverProvinceID.ToString(),
                        RECEIVER_PHONE = dataSearchInfo.ReceiverPhone,
                        RECEIVER_DISTRICT_ID = dataSearchInfo.ReceiverDistrictID.ToString(),
                        RECEIVER_WARD_ID = dataSearchInfo.ReceiverWardID.ToString(),
                        RECEIVER_EMAIL = "",
                        RECEIVER_STREET = dataSearchInfo.ReceiverFullAddress, //ReceiverFullAddress
                        RECEIVER_ADDRESS = dataSearchInfo.ReceiverFullAddress,
                        PRODUCT_NAME = ProductName,
                        PRODUCT_VALUE = Convert.ToInt32(dataSearchInfo.ProductValue),
                        PRODUCT_CODE = dataSearchInfo.ProductCode,
                        PRODUCT_DESCRIPTION = Description,
                        PRODUCT_QUANTITY = dataSearchInfo.ProductQuantity,
                        WEIGHT = dataSearchInfo.Weight,
                        ADDITION_SERVICE = dataSearchInfo.AdditionService,
                        SPECIAL_SERVICE = dataSearchInfo.EspeciallyServiceCode,
                        TOTAL_AMOUNT = dataSearchInfo.TotalAmount,
                        CUSTOMER_CODE = dataSearchInfo.CrmOrPayPostCode,
                        TO_COUNTRY = "VN",
                        PO_CREATE = dataSearchInfo.POCode == "" ? "0100" : dataSearchInfo.POCode,
                        COMPLETE = 0,
                        POSTAL_PROVINCE_CODE = dataSearchInfo.ReceiverProvinceID.ToString(),
                        POSTAL_DISTRICT_CODE = dataSearchInfo.ReceiverDistrictID.ToString(),
                        POSTAL_COMMUNE_CODE = dataSearchInfo.ReceiverWardID.ToString(),
                        POSTAL_CODE = "",
                        SERVICE_TYPE = dataSearchInfo.EspeciallyServiceType,
                        SERVICE_FEE = dataSearchInfo.ServiceFee,
                        CLASSIFY = dataSearchInfo.Classification.Code,
                        IS_CONSIGNMENT = Convert.ToInt32(dataSearchInfo.Consignment.Code),
                        CHANNEL = "ONE"
                    });

                    ZTransportInfoShippingApiRequest reqPickupCreate = new ZTransportInfoShippingApiRequest()
                    {
                        PICKUP_CODE_API = "",
                        PO_CODE = dataSearchInfo.POCode,
                        DESCRIPTION = "",
                        CUSTOMER_CODE = dataSearchInfo.CrmOrPayPostCode,
                        CUSTOMER_PHONE = dataSearchInfo.ShopPhone,
                        CUSTOMER_NAME = dataSearchInfo.ShopFullName,
                        CUSTOMER_EMAIL = dataSearchInfo.ShopEmail,
                        STOREID = "",
                        PICKUP_PROVINCE_ID = dataSearchInfo.SenderProvinceID,
                        PICKUP_DISTRICT_ID = dataSearchInfo.SenderDistrictID,
                        PICKUP_WARD_ID = dataSearchInfo.SenderWardID,
                        PICKUP_STREET = dataSearchInfo.SenderFullAddress, //SenderStreet
                        PICKUP_DATE = DateTime.Now.ToString("yyyyMMdd"),
                        PICKUP_HOURS = DateTime.Now.ToString("HHmm"),
                        CONTACT_NAME = dataSearchInfo.SenderName,
                        CONTACT_PHONE = dataSearchInfo.SenderPhone,
                        PICKUP_POSTMAN = "",
                        PICKUP_POCODE = dataSearchInfo.CollectionPostOfficeCode,
                        PICKUP_CONFIG = dataSearchInfo.CollectionType == 2 ? "1" : "0",
                        //EMS: 1: thu gom, 2: gui hang
                        //PNS: 0: thu gom, 1: gui hang
                        SignatureCode = Security.SHA256(dataSearchInfo.CrmOrPayPostCode + XHelper.EMSTransportKey).ToUpper(),
                        LIST_ITEM = TransportInfoShopDetail
                    };

                    
                    var resSendRequestTransport = EMSRepository.SendRequestTransport(reqPickupCreate);
                    

                    XHelper.LogToFile(LogFileType.TRACE, string.Format("XCommon.SendTransportOrder: request={0}||response={1}", JsonConvert.SerializeObject(reqPickupCreate), JsonConvert.SerializeObject(resSendRequestTransport)));

                    if (resSendRequestTransport.Code == "00")
                    {
                        res.Code = "00";
                        res.Message = "Tạo đơn hàng và gửi vận chuyển thành công";
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi kết nối API vận chuyển, vui lòng liên hệ quản trị.";
                    }
                }
                else
                {
                    res.Code = resSearchInfoSendTransport.Code;
                    res.Message = resSearchInfoSendTransport.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SendTransportOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        public static ZResponse SendTransportOrderBatch(string AmndState, string BatchCode, int ShopID)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resSearchInfoSendTransport = OrderBatchRepository.SendTransportInfo(new ZOrderBatchSendTransportInfoRequest() { AmndState = AmndState, BatchCode = BatchCode, ShopID = ShopID });
                if (resSearchInfoSendTransport.Code == "00")
                {
                    var itemSearchInfoSendTransport = JsonConvert.DeserializeObject<List<ZOrderBatchSendTransportInfoResponse>>(resSearchInfoSendTransport.Data);
                    List<ZListItemInfoShippingRequest> TransportInfoShopDetail = new List<ZListItemInfoShippingRequest>();
                    foreach (var item in itemSearchInfoSendTransport)
                    {
                        if (item.EspeciallyServiceCode == "TMD_B" || item.EspeciallyServiceCode == "TMDHCM")
                        {
                            item.EspeciallyServiceCode = "TMD";
                        }

                        if (item.TransportPayer == 2 && item.AdditionService.Contains("COD") == true)
                        {
                            item.AdditionService += ",CVS";
                        }

                        var Description = "";
                        if (!string.IsNullOrEmpty(item.ProductDescription) && !string.IsNullOrEmpty(item.ProductNote))
                        {
                            Description = item.ProductDescription + " || " + item.ProductNote;
                        }
                        else
                        {
                            Description = string.IsNullOrEmpty(item.ProductDescription) ? item.ProductNote : item.ProductDescription;
                        }

                        var ProductName = item.ProductName;
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = item.ProductItem;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.ProductItem))
                            {
                                ProductName = item.ProductItem + " + " + item.ProductName;
                            }
                        }

                        TransportInfoShopDetail.Add(new ZListItemInfoShippingRequest()
                        {
                            TRACKING_CODE = item.ShippingCode,
                            SENDER_NAME = item.SenderName,
                            SENDER_CODE = item.CrmOrPayPostCode,
                            SENDER_PHONE = item.SenderPhone,
                            SENDER_STREET = item.SenderFullAddress, //SenderStreet
                            SENDER_ADDRESS = item.SenderFullAddress,
                            SENDER_PROVINCE_ID = item.SenderProvinceID.ToString(),
                            SENDER_DISTRICT_ID = item.SenderDistrictID.ToString(),
                            SENDER_WARD_ID = item.SenderWardID.ToString(),
                            SENDER_EMAIL = item.SenderEmail,
                            RECEIVER_NAME = item.ReceiverName,
                            RECEIVER_PROVINCE_ID = item.ReceiverProvinceID.ToString(),
                            RECEIVER_PHONE = item.ReceiverPhone,
                            RECEIVER_DISTRICT_ID = item.ReceiverDistrictID.ToString(),
                            RECEIVER_WARD_ID = item.ReceiverWardID.ToString(),
                            RECEIVER_EMAIL = "",
                            RECEIVER_STREET = item.ReceiverFullAddress, //ReceiverFullAddress
                            RECEIVER_ADDRESS = item.ReceiverFullAddress,
                            PRODUCT_NAME = ProductName,
                            PRODUCT_VALUE = Convert.ToInt32(item.ProductValue),
                            PRODUCT_CODE = item.ProductCode,
                            PRODUCT_DESCRIPTION = Description,
                            PRODUCT_QUANTITY = item.ProductQuantity,
                            WEIGHT = item.Weight,
                            ADDITION_SERVICE = item.AdditionService,
                            SPECIAL_SERVICE = item.EspeciallyServiceCode,
                            TOTAL_AMOUNT = item.TotalAmount,
                            CUSTOMER_CODE = item.CrmOrPayPostCode,
                            TO_COUNTRY = "VN",
                            PO_CREATE = item.POCode == "" ? "0100" : item.POCode,
                            COMPLETE = 0,
                            POSTAL_PROVINCE_CODE = item.ReceiverProvinceID.ToString(),
                            POSTAL_DISTRICT_CODE = item.ReceiverDistrictID.ToString(),
                            POSTAL_COMMUNE_CODE = item.ReceiverWardID.ToString(),
                            POSTAL_CODE = "",
                            SERVICE_TYPE = item.EspeciallyServiceType,
                            SERVICE_FEE = item.ServiceFee,
                            CLASSIFY = item.Classification.Code,
                            IS_CONSIGNMENT = Convert.ToInt32(item.Consignment.Code),
                            CHANNEL = "ONE"
                        });
                    }

                    ZTransportInfoShippingApiRequest reqPickupCreate = new ZTransportInfoShippingApiRequest()
                    {
                        PICKUP_CODE_API = "",
                        PO_CODE = itemSearchInfoSendTransport[0].POCode,
                        DESCRIPTION = "",
                        CUSTOMER_CODE = itemSearchInfoSendTransport[0].CrmOrPayPostCode,
                        CUSTOMER_PHONE = itemSearchInfoSendTransport[0].ShopPhone,
                        CUSTOMER_NAME = itemSearchInfoSendTransport[0].ShopFullName,
                        CUSTOMER_EMAIL = itemSearchInfoSendTransport[0].ShopEmail,
                        STOREID = "",
                        PICKUP_PROVINCE_ID = itemSearchInfoSendTransport[0].SenderProvinceID,
                        PICKUP_DISTRICT_ID = itemSearchInfoSendTransport[0].SenderDistrictID,
                        PICKUP_WARD_ID = itemSearchInfoSendTransport[0].SenderWardID,
                        PICKUP_STREET = itemSearchInfoSendTransport[0].SenderFullAddress, //SenderStreet
                        PICKUP_DATE = DateTime.Now.ToString("yyyyMMdd"),
                        PICKUP_HOURS = DateTime.Now.ToString("HHmm"),
                        CONTACT_NAME = itemSearchInfoSendTransport[0].SenderName,
                        CONTACT_PHONE = itemSearchInfoSendTransport[0].SenderPhone,
                        PICKUP_POSTMAN = "",
                        PICKUP_POCODE = itemSearchInfoSendTransport[0].CollectionPostOfficeCode,
                        PICKUP_CONFIG = itemSearchInfoSendTransport[0].CollectionType == 2 ? "1" : "0",
                        //EMS: 1: thu gom, 2: gui hang
                        //PNS: 0: thu gom, 1: gui hang
                        SignatureCode = Security.SHA256(itemSearchInfoSendTransport[0].CrmOrPayPostCode + XHelper.EMSTransportKey).ToUpper(),
                        LIST_ITEM = TransportInfoShopDetail
                    };

                    var resSendRequestTransport = EMSRepository.SendRequestTransport(reqPickupCreate);
                    //foreach(var i in itemSearchInfoSendTransport)
                    //{
                    //    Task.Run(() =>
                    //    {
                    //        try
                    //        {
                    //            ZSendReceiverInforRequest reqSendReceiverInfor = new ZSendReceiverInforRequest()
                    //            {
                    //                ItemCode = i.ShippingCode,
                    //                OrderCode = i.EMSOneCode,
                    //                CustomerCode = i.CrmOrPayPostCode,
                    //                SenderName = i.SenderName,
                    //                SenderPhone = i.SenderPhone,
                    //                SenderAddress = i.SenderFullAddress,
                    //                ReceiverName = i.ReceiverName,
                    //                ReceiverPhone = i.ReceiverPhone,
                    //                ReceiverAddress = i.ReceiverFullAddress,
                    //                Channel = "EMSONE",
                    //                Extend_1 = i.ProductDescription,

                    //            };
                    //            var resSendReceiverInfor = EMSRepository.SendReceiverInfor(reqSendReceiverInfor);
                    //        }
                    //        catch
                    //        {
                    //            // Bỏ qua luôn hoặc ghi log nếu muốn
                    //        }
                    //    });
                    //}
                    

                    XHelper.LogToFile(LogFileType.TRACE, string.Format("XCommon.SendTransportOrderBatch: request={0}||response={1}", JsonConvert.SerializeObject(reqPickupCreate), JsonConvert.SerializeObject(resSendRequestTransport)));

                    if (resSendRequestTransport.Code == "00")
                    {
                        res.Code = "00";
                        res.Message = "Gửi vận chuyển thành công";
                    }
                    else
                    {
                        res.Code = "99";
                        res.Message = "Lỗi kết nối API vận chuyển, vui lòng liên hệ quản trị.";
                    }
                }
                else
                {
                    res.Code = resSearchInfoSendTransport.Code;
                    res.Message = resSearchInfoSendTransport.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SendTransportOrderBatch: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        #region Huỷ yêu cầu vận chuyển qua API PNS_EMS_PICKUP_CANCEL

        public static ZResponse CancelTransportOrder(string CrmOrPaypostCode, string ShippingCode)
        {
            ZResponse res = new ZResponse();
            try
            {
                var SignatureCode = Security.SHA256(ShippingCode + CrmOrPaypostCode + XHelper.EMSTransportKey).ToUpper();

                ZTransportShippingCancelDataRequest reqCancelTransport = new ZTransportShippingCancelDataRequest()
                {
                    CustomerCode = CrmOrPaypostCode,
                    TrackingCode = ShippingCode,
                    SignatureCode = SignatureCode
                };

                var resCancelTransport = EMSRepository.CancelRequestTransport(reqCancelTransport);

                XHelper.LogToFile(LogFileType.TRACE, string.Format("XCommon.CancelTransportOrder: request={0}||response={1}", JsonConvert.SerializeObject(reqCancelTransport), JsonConvert.SerializeObject(resCancelTransport)));

                if (resCancelTransport.Code == "00")
                {
                    res.Code = "00";
                    res.Message = "Hủy yêu cầu vận chuyển thành công";
                }
                else
                {
                    res.Code = "99";
                    res.Message = resCancelTransport.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.CancelTransportOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }

        //public static ZResponse CancelTransportOrderBatch(string ShippingCode, string CrmOrPaypostCode)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var SignatureCode = Security.SHA256(ShippingCode + CrmOrPaypostCode + XHelper.EMSTransportKey).ToUpper();

        //        ZTransportShippingCancelDataRequest reqCancelTransport = new ZTransportShippingCancelDataRequest()
        //        {
        //            CustomerCode = CrmOrPaypostCode,
        //            TrackingCode = ShippingCode,
        //            SignatureCode = SignatureCode
        //        };

        //        var resCancelTransport = EMSRepository.CancelRequestTransport(reqCancelTransport);

        //        XHelper.LogToFile(LogFileType.TRACE, string.Format("XCommon.CancelTransportOrderBatch: request={0}||response={1}", JsonConvert.SerializeObject(reqCancelTransport), JsonConvert.SerializeObject(resCancelTransport)));

        //        if (resCancelTransport.Code == "00")
        //        {
        //            res.Code = "00";
        //            res.Message = "Hủy yêu cầu vận chuyển thành công";
        //        }
        //        else
        //        {
        //            res.Code = "99";
        //            res.Message = "Lỗi kết nối API vận chuyển, vui lòng liên hệ quản trị.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.CancelTransportOrderBatch: " + ex.Message);
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //    }
        //    return res;
        //}

        public static ZResponse CancelRequestTransport(ZTransportShippingCancelDataRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = EMSRepository.CancelRequestTransport(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.CancelRequestTransport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        #region Gửi - Huỷ yêu cầu vận chuyển qua API Grab
        public static ZResponse SendReqGrabDelivery(int OrderID, int ShopID, string AmndState, string PromoCode)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resSearchInfoSendReqDelivery = OrderRepository.SendTransportInfo(new ZOrderSendTransportInfoRequest()
                {
                    OrderID = OrderID,
                    ShopID = ShopID,
                    AmndState = AmndState
                });
                if (resSearchInfoSendReqDelivery.Code == "00")
                {
                    var dataSearchInfo = JsonConvert.DeserializeObject<ZOrderSendTransportInfoResponse>(resSearchInfoSendReqDelivery.Data);

                    List<ZGrabDeliveryOrderOrigin> DeliveryOrderOrigin = new List<ZGrabDeliveryOrderOrigin>();

                    DeliveryOrderOrigin.Add(new ZGrabDeliveryOrderOrigin()
                    {
                        name = dataSearchInfo.SenderName,
                        phone = dataSearchInfo.SenderPhone,
                        province = dataSearchInfo.SenderProvinceID.ToString(),
                        district = dataSearchInfo.SenderDistrictID.ToString(),
                        ward = dataSearchInfo.SenderWardID.ToString(),
                        address = dataSearchInfo.SenderFullAddress,
                        description = dataSearchInfo.Note
                    });

                    List<ZGrabDeliveryOrderDestination> DeliveryOrderDestination = new List<ZGrabDeliveryOrderDestination>();

                    DeliveryOrderDestination.Add(new ZGrabDeliveryOrderDestination()
                    {
                        name = dataSearchInfo.ReceiverName,
                        phone = dataSearchInfo.ReceiverPhone,
                        province = dataSearchInfo.ReceiverProvinceID.ToString(),
                        district = dataSearchInfo.ReceiverDistrictID.ToString(),
                        ward = dataSearchInfo.ReceiverWardID.ToString(),
                        address = dataSearchInfo.ReceiverFullAddress,
                        product_name = dataSearchInfo.ProductName,
                        quantity = dataSearchInfo.ProductQuantity,
                        total_amount = Convert.ToInt32(dataSearchInfo.ProductValue),
                        weight = dataSearchInfo.Weight,
                        height = dataSearchInfo.TransportSizeHeight,
                        width = dataSearchInfo.TransportSizeWidth,
                        length = dataSearchInfo.TransportSizeLength,
                        money_collect = Convert.ToInt32(dataSearchInfo.TotalCOD),
                        extra = 1
                    });

                    object obj = new object();
                    if (string.IsNullOrEmpty(PromoCode))
                    {
                        obj = new
                        {
                            ordercode = dataSearchInfo.EMSOneCode,
                            serviceType = dataSearchInfo.EspeciallyServiceCode,
                            origin = DeliveryOrderOrigin,
                            destination = DeliveryOrderDestination,
                            customer_code = dataSearchInfo.CrmOrPayPostCode
                        };
                    }
                    else
                    {
                        obj = new
                        {
                            ordercode = dataSearchInfo.EMSOneCode,
                            serviceType = dataSearchInfo.EspeciallyServiceCode,
                            origin = DeliveryOrderOrigin,
                            destination = DeliveryOrderDestination,
                            customer_code = dataSearchInfo.CrmOrPayPostCode,
                            promo_code = PromoCode
                        };
                    }

                    //ZGrabDeliveryOrderRequest reqDeliveryOrder = new ZGrabDeliveryOrderRequest()
                    //{
                    //    ordercode = dataSearchInfo.EMSOneCode,
                    //    serviceType = dataSearchInfo.EspeciallyServiceCode,
                    //    origin = DeliveryOrderOrigin,
                    //    destination = DeliveryOrderDestination,
                    //    customer_code = dataSearchInfo.CrmOrPayPostCode
                    //};

                    var resSendReqGrabDelivery = GrabRepository.SendReqGrabDelivery(obj);

                    //XHelper.LogToFile(LogFileType.MESSAGE, string.Format("XCommon.SendReqGrabDelivery: request={0}||response={1}", JsonConvert.SerializeObject(reqDeliveryOrder), JsonConvert.SerializeObject(resSendReqGrabDelivery)));

                    if (resSendReqGrabDelivery.Code == "00")
                    {
                        res.Code = "00";
                        res.Message = "Tạo đơn hàng và gửi vận chuyển Grab thành công";
                        res.Data = resSendReqGrabDelivery.Data;
                    }
                    else
                    {
                        XHelper.LogToFile(LogFileType.MESSAGE, string.Format("XCommon.SendReqGrabDelivery: request={0}||response={1}", JsonConvert.SerializeObject(obj), JsonConvert.SerializeObject(resSendReqGrabDelivery)));
                        res.Code = "99";
                        //res.Message = "Lỗi kết nối API vận chuyển, vui lòng liên hệ quản trị.";
                        res.Message = resSendReqGrabDelivery.Message;
                    }
                }
                else
                {
                    res.Code = resSearchInfoSendReqDelivery.Code;
                    res.Message = resSearchInfoSendReqDelivery.Message;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SendReqGrabDelivery: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        public static ZResponse SendReqCancelGrabDelivery(ZGrabDeliveryCancelRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = GrabRepository.SendReqCancelGrabDelivery(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SendReqCancelGrabDelivery: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        #region Gửi yêu cầu hỗ trợ CMS
        public static ZResponse AddRequestSupport(List<ZCMSAddRequestSupportItemRequest> req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "CMS_ADD_REQUEST_SUPPORT",
                    Data = JsonConvert.SerializeObject(new ZCMSAddRequestSupportRequest()
                    {
                        items = req
                    })
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.AddRequestSupport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        #region Lấy kết quả yêu cầu hỗ trợ CMS
        public static ZResponse ResultRequestSupport(ZCMSResultRequestSupportRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "CMS_RESULT_REQUEST_SUPPORT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.ResultRequestSupport: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return res;
        }
        #endregion

        #region Lấy Tỉnh/ Thành phố || Quận/ Huyện || Phường/ Xã theo địa chỉ nhập vào (từ gợi ý địa chỉ (Search) của EMS)
        public static ZResponse SearchPWDByEMSMapsSearch(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resEMSMapSearch = EMSRepository.MapSearch(new ZEMSMapSearchRequest() { AddressFull = Address });
                if (resEMSMapSearch.Code == "00")
                {
                    var dataEMSMapSearch = JsonConvert.DeserializeObject<ZEMSMapSearchDataResponse>(resEMSMapSearch.Data);
                    if (dataEMSMapSearch.Province_Code == "0")
                    {
                        res.Code = "01";
                        res.Message = "Không tìm thấy địa chỉ";
                    }
                    else
                    {
                        // Comment lại để sửa lấy luôn hàm phân tích của MCS

                        //res = MapsRepository.GetByMBC2017Code(new ZMapsGetByMBC2017CodeRequest()
                        //{
                        //    ProvinceMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.Province_Code) ? "0" : dataEMSMapSearch.Province_Code,
                        //    DistrictMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.District_Code) ? "0" : dataEMSMapSearch.District_Code,
                        //    WardMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.Ward_Code) ? "0" : dataEMSMapSearch.Ward_Code,
                        //    Street = dataEMSMapSearch.Label.Split(',')[0]
                        //});

                        var model = new ZMapsGetByMBC2017CodeResponse()
                        {
                            ProvinceID = Convert.ToInt32(dataEMSMapSearch.Province_Code),
                            ProvinceName = dataEMSMapSearch.Province_Name,
                            DistrictID = Convert.ToInt32(dataEMSMapSearch.District_Code),
                            DistrictName = dataEMSMapSearch.District_Name,
                            WardID = Convert.ToInt32(dataEMSMapSearch.Ward_Code),
                            WardName = dataEMSMapSearch.Ward_Name,
                            Street = dataEMSMapSearch.Label.Split(',')[0]
                        };
                        res = new ZResponse
                        {
                            Code = "00",
                            Message = "Lấy dữ liệu thành công",
                            Data = JsonConvert.SerializeObject(model)
                        };
                    }
                    
                }
                else
                {
                    res.Code = resEMSMapSearch.Code;
                    res.Message = resEMSMapSearch.Message;
                }
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchPWDByEMSMapSearch: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchPWDByEMSVMapsSearch(string Address)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resEMSMapSearch = EMSRepository.VMapSearch(new ZEMSMapSearchRequest() { AddressFull = Address });
                if(resEMSMapSearch.Code == "00")
                {
                    var dataEMSMapSearch = JsonConvert.DeserializeObject<ZEMSMapSearchDataResponse>(resEMSMapSearch.Data);
                    if(dataEMSMapSearch.Province_Code == "0")
                    {
                        res.Code = "01";
                        res.Message = "Không tìm thấy địa chỉ";
                    }
                    else
                    {
                        // Comment lại để sửa lấy luôn hàm phân tích của MCS

                        //res = MapsRepository.GetByMBC2017Code(new ZMapsGetByMBC2017CodeRequest()
                        //{
                        //    ProvinceMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.Province_Code) ? "0" : dataEMSMapSearch.Province_Code,
                        //    DistrictMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.District_Code) ? "0" : dataEMSMapSearch.District_Code,
                        //    WardMBC2017Code = string.IsNullOrEmpty(dataEMSMapSearch.Ward_Code) ? "0" : dataEMSMapSearch.Ward_Code,
                        //    Street = dataEMSMapSearch.Label.Split(',')[0]
                        //});

                        var model = new ZMapsGetByMBC2017CodeResponse()
                        {
                            ProvinceID = Convert.ToInt32(dataEMSMapSearch.Province_Code),
                            ProvinceName = dataEMSMapSearch.Province_Name,
                            DistrictID = Convert.ToInt32(dataEMSMapSearch.District_Code),
                            DistrictName = dataEMSMapSearch.District_Name,
                            WardID = Convert.ToInt32(dataEMSMapSearch.Ward_Code),
                            WardName = dataEMSMapSearch.Ward_Name,
                            Street = dataEMSMapSearch.Label.Split(',')[0]
                        };
                        res = new ZResponse
                        {
                            Code = "00",
                            Message = "Lấy dữ liệu thành công",
                            Data = JsonConvert.SerializeObject(model)
                        };
                    }

                }
                else
                {
                    res.Code = resEMSMapSearch.Code;
                    res.Message = resEMSMapSearch.Message;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchPWDByEMSMapSearch: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchPWDByVNPOSTSearch(ZVNPOSTMapSearchRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resEMSMapSearch = EMSRepository.MapSearchVNPOST(new ZVNPOSTMapSearchRequest() {
                    direction = 2,
                    provinceCode = req.provinceCode,
                    wardCode = req.wardCode,
                    territory = 2,
                    inputText = req.inputText
                });
                if(resEMSMapSearch.Code == "00")
                {
                    var dataEMSMapSearch = JsonConvert.DeserializeObject<List<ZVNPOSTMapSearchDataResponse>>(resEMSMapSearch.Data);
                    var firstItem = dataEMSMapSearch.FirstOrDefault();

                    var model = new ZMapsGetByMBC2017CodeResponse()
                    {
                        ProvinceID = Convert.ToInt32(firstItem?.provinceCodeOld ?? "0"),
                        ProvinceName = firstItem?.provinceNameOld ?? "",
                        DistrictID = Convert.ToInt32(firstItem?.districtCodeOld ?? "0"),
                        DistrictName = firstItem?.districtNameOld ?? "",
                        WardID = Convert.ToInt32(firstItem?.wardCodeOld ?? "0"),
                        WardName = firstItem?.wardNameOld ?? "",
                        Street = req.inputText ?? ""
                    };

                    res = new ZResponse
                    {
                        Code = "00",
                        Message = "Lấy dữ liệu thành công",
                        Data = JsonConvert.SerializeObject(model)
                    };

                }
                else
                {
                    res.Code = resEMSMapSearch.Code;
                    res.Message = resEMSMapSearch.Message;
                }
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.SearchPWDByEMSMapSearch: " + ex.Message);
            }
            return res;
        }
        #endregion

        #region QR_CODE
        public static dynamic GenerateQRCode(string data)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }
        #endregion

        #region BAR_CODE WITH TEXT
        public static dynamic GenerateBarCodeWithText(string data)
        {
            var barcodeImage = BarcodeDrawFactory.Code128WithChecksum.Draw(data, 50);
            var resultImage = new Bitmap(barcodeImage.Width, barcodeImage.Height + 20); // 20 is bottom padding, to adjust text.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Graphics graphics = Graphics.FromImage(resultImage))
                {
                    Font font = new Font("Arial", 10);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    PointF point = new PointF(25f, 50f);
                    graphics.Clear(Color.White);
                    graphics.DrawImage(barcodeImage, 0, 0);
                    graphics.DrawString(data, font, brush, point);
                }
                resultImage.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
                //ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        #endregion

        #region BAR_CODE WITHOUT TEXT
        public static dynamic GenerateBarCodeWithoutText(string data)
        {
            var barcodeImage = BarcodeDrawFactory.Code128WithChecksum.Draw(data, 50);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                barcodeImage.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
                //ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        #endregion

        #region Lấy OrderCode từ OrderID
        public static string ConvertToOrderCode(ZOrderConvertToOrderCodeRequest req)
        {
            var OrderCode = "";
            var res = OrderRepository.ConvertToOrderCode(req);
            if (res.Code == "00")
            {
                OrderCode = res.Data;
            }
            return OrderCode;
        }
        #endregion

        #region Tính quy đối khối lượng
        public static double ExchangeTransportWeight(ZTransportExchangeTransportWeightRequest req)
        {
            double ExchangeTransportWeight = req.TransportWeight;
            if (req.TransportSizeLength > 0 && req.TransportSizeWidth > 0 && req.TransportSizeHeight > 0)
            {
                double convertTransportWeight = 0;
                //LMP - LMS 
                //DEV: 581 - 601
                //PRO: 400 - 401
                if (req.TransportMainServiceID == 400 || req.TransportMainServiceID == 401)
                {
                    convertTransportWeight = Math.Round((req.TransportSizeLength * req.TransportSizeWidth * req.TransportSizeHeight) / 5);
                    double convertTransportWeightTemp = Math.Ceiling(convertTransportWeight / 1000) * 1000;
                    convertTransportWeight = convertTransportWeightTemp;
                }
                else
                {
                    convertTransportWeight = Math.Round((req.TransportSizeLength * req.TransportSizeWidth * req.TransportSizeHeight) / 6);
                }

                //TMD - TMD_B
                if (req.TransportMainServiceID == 141 || req.TransportMainServiceID == 360)
                {
                    List<double> ListLWH = new List<double>() { req.TransportSizeLength, req.TransportSizeWidth, req.TransportSizeHeight };
                    var getMax = ListLWH.Max();
                    if (getMax > 50 && convertTransportWeight > req.TransportWeight)
                    {
                        ExchangeTransportWeight = convertTransportWeight;
                    }
                }
                else
                {
                    if (convertTransportWeight > req.TransportWeight)
                    {
                        ExchangeTransportWeight = convertTransportWeight;
                    }
                }
            }
            return ExchangeTransportWeight;
        }
        #endregion

        #region COMMON
        public static bool CheckRegex(string type, string data)
        {
            if(string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(data))
                return false;

            string pattern;

            switch(type)
            {
                case "Address":
                    // Tiếng Việt + số + - / . , + khoảng trắng
                    pattern = @"^[a-zA-ZÀ-ỹ0-9\-\/\.,\s]+$";
                    break;

                case "MobileNumber":
                    /*
                     * - Di động: 03x,05x,07x,08x,09x (10 số)
                     * - Cố định: 02xxxxxxxxx (11 số)
                     * - +84 di động: +843xxxxxxxx
                     * - +84 cố định: +842xxxxxxxxx
                     * - Hotline: 1800/1900 + 4–6 số
                     */
                    pattern = @"^(" +
                              @"(\+84(3|5|7|8|9)[0-9]{8})|" +
                              @"(\+8469[0-9]{6,7})|" +
                              @"(\+842[0-9]{9})|" +
                              @"(0(3|5|7|8|9)[0-9]{8})|" +
                              @"(069[0-9]{6,7})|" +
                              @"(02[0-9]{9})|" +
                              @"((1800|1900)[0-9]{4,6})" +
                              @")$";
                    break;

                case "OrderCode":
                    // a-z A-Z 0-9 và ký tự đặc biệt
                    pattern = @"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?]+$";
                    break;

                case "Code":
                    // a-z A-Z 0-9 -
                    pattern = @"^[a-zA-Z0-9-]+$";
                    break;

                case "Vietnamese":
                    // Tiếng Việt + số + khoảng trắng
                    pattern = @"^[a-zA-ZÀ-ỹ0-9\s]+$";
                    break;

                default:
                    return false;
            }

            return Regex.IsMatch(
                data,
                pattern,
                RegexOptions.CultureInvariant | RegexOptions.Singleline
            );
        }
        public static string FormatFromDate(string FromDate)
        {
            try
            {
                if (string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 6);
                    FromDate = fromDate.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetFromDateDefault: " + ex.Message);
            }
            return FromDate;
        }
        public static string FormatToDate(string ToDate)
        {
            try
            {
                if (string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    ToDate = toDate.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "XCommon.GetFromDateDefault: " + ex.Message);
            }
            return ToDate;
        }
        public static string ToXML(DataTable table)
        {
            string[] columnNames = table.Columns.Cast<DataColumn>()
                                 .Where(c => c.DataType == typeof(DateTime))
                                 .Select(x => x.ColumnName)
                                 .ToArray();

            StringWriter writer = new StringWriter();
            table.WriteXml(new CustomXmlTextWriter(writer, columnNames), XmlWriteMode.IgnoreSchema, false);

            return writer.ToString();
        }
        public class CustomXmlTextWriter : XmlTextWriter
        {
            private string[] watchElement;
            private bool onWatch;

            public CustomXmlTextWriter(TextWriter writer, string[] columnNames) : base(writer)
            {
                this.watchElement = columnNames;
            }

            public override void WriteStartElement(string prefix, string localName, string ns)
            {
                base.WriteStartElement(prefix, localName, ns);
                if (watchElement.Contains(localName))
                {
                    onWatch = true;
                }
            }

            public override void WriteString(string text)
            {
                if (onWatch)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(text);
                        text = dt.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    catch (FormatException)
                    {
                        ;
                    }
                    onWatch = false;
                }
                base.WriteString(text);
            }
        }
        public static string ToXML(this object value)
        {
            try
            {
                using (var stringwriter = new ExtentedStringWriter())
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType(), new System.Xml.Serialization.XmlRootAttribute("Data"));
                    serializer.Serialize(stringwriter, value);
                    return stringwriter.ToString();
                };
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public sealed class ExtentedStringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }
        #endregion

        #region SqlInjectionChecker
        private static readonly char[] SpecialChars = { '\'', '\"', '\\', '/', ',', ';', '+', '-', '*', '%', ' ', '#', '&', '|', '<', '>', '=', '(', ')', '[', ']', '{', '}', ':' };

        // Hàm kiểm tra chuỗi đầu vào có chứa ký tự đặc biệt hay không
        public static bool ContainsSpecialChars( string input )
        {
            if ( string.IsNullOrEmpty(input) )
            {
                return false;
            }

            return input.Any(c => SpecialChars.Contains(c));
        }
        #endregion

        public static string CheckStyleColumnExcel(ICell cell)
        {
            string value = "";
            if(cell != null)
            {
                switch(cell.CellType)
                {
                    case CellType.Numeric:
                        if(HSSFDateUtil.IsCellDateFormatted(cell))
                        {
                            //DateFormat df = new SimpleDateFormat("dd/MM/yyyy");
                            //return df.Format(cell.getDateCellValue());
                            value = cell.DateCellValue.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            //DecimalFormat df = new DecimalFormat("##.###############");
                            //return df.Format(cell.NumericCellValue());
                            value = cell.NumericCellValue.ToString();
                        }
                        break;
                    case CellType.String:
                        value = Convert.ToString(cell.StringCellValue);
                        break;
                    case CellType.Formula:
                        if(cell.CachedFormulaResultType == CellType.String)
                        {
                            value = Convert.ToString(cell.StringCellValue);
                        }
                        else
                        {
                            value = Convert.ToString(cell.NumericCellValue);
                        }
                        break;
                    default:
                        value = Convert.ToString(cell.StringCellValue);
                        break;
                }
            }
            if(!string.IsNullOrEmpty(value))
            {
                value = ReplaceNonUnicodeCharacter(value.Trim());
            }
            return value;
        }
        #region Thay thế các non-unicode
        public static string ReplaceNonUnicodeCharacter(string Value)
        {
            return Regex.Replace(Value, @"[\u200c]", "");
            //return Regex.Replace(Value, @"[\u0000-\u0008\u000A-\u001F\u0100-\uFFFF]", "");
            //return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(Value));
            //return Regex.Replace(Value, @"[^\u0000-\u007F]+", string.Empty);
        }
        #endregion
    }
}