using EMS.API.PartnerGateway.Repositories;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMS.API.PartnerGateway.Controllers
{
    public class GatewayController : ApiController
    {
        [HttpPost]
        [Route("Execute")]
        public ZResponse Execute(ZRequest req)
        {
            string traceId = Guid.NewGuid().ToString("N");
            Stopwatch watch = Stopwatch.StartNew();

            string url = "/Gateway/Execute";
            string requestBody = string.Empty;
            string collectionName = "EmsOneLog";
            bool shouldLog = false;
            int statusCode = 200;
            Exception capturedException = null;

            ZResponse res = new ZResponse();

            try
            {
                requestBody = JsonConvert.SerializeObject(req);

                if(req != null && !string.IsNullOrWhiteSpace(req.Code))
                {
                    shouldLog = XMongoLogHelper.ShouldLog(req.Code);
                    if(shouldLog)
                    {
                        collectionName = XMongoLogHelper.GetCollectionName(req.Code);
                    }
                }

                if(req == null)
                {
                    res.Code = "01";
                    res.Message = "Request không hợp lệ.";
                    return res;
                }

                if(req.Channel != "TUNGKX")
                {
                    var signature = Security.SHA256(req.Code + req.Data + XHelper.EMSOneSecretKey);
                    if(signature != req.Signature)
                    {
                        res.Code = "02";
                        res.Message = "Chữ ký số không hợp lệ";
                        return res;
                    }
                }
                
                

                switch(req.Code)
                {
                    #region PARTNER_ORDER
                    case "PARTNER_ORDER_ADD":
                        res = PartnerRepository.OrderAdd(req.Data);
                        break;

                    case "PARTNER_VIB_ORDER_ADD":
                        res = PartnerRepository.VIBOrderAdd(req.Data);
                        break;

                    case "PARTNER_BIDV_ORDER_ADD":
                        res = PartnerRepository.VIBOrderAdd(req.Data);
                        break;

                    case "PARTNER_ANALYSTIC_ORDER_ADD":
                        res = PartnerRepository.AnalysticOrderAdd(req.Data);
                        break;

                    case "PARTNER_VINAPHONE_ORDER_ADD":
                        res = PartnerRepository.VinaphoneOrderAdd(req.Data);
                        break;
                    case "PARTNER_INTERNATIONAL_ORDER_ADD":
                        res = PartnerRepository.InternationalOrderAdd(req.Data);
                        break;

                    case "PARTNER_TEMU_ORDER_ADD":
                        res = PartnerRepository.TemuOrderAdd(req.Data);
                        break;

                    case "PARTNER_VLP_ORDER_STATUS_ADD":
                        res = PartnerRepository.VLPStatusAdd(req.Data);
                        break;

                    case "PARTNER_ORDER_CANCEL":
                        res = PartnerRepository.OrderCancel(req.Data);
                        break;

                    case "PARTNER_ORDER_INFO":
                        res = PartnerRepository.OrderInfo(req.Data);
                        break;

                    case "PARTNER_ORDER_DELIVERY_INFO":
                        res = PartnerRepository.OrderDeliveryInfo(req.Data);
                        break;
                    case "PARTNER_ORDER_PRINT":
                        res = PartnerRepository.OrderPrint(req.Data);
                        break;

                    case "PARTNER_ORDER_PRINT_V2":
                        res = PartnerRepository.OrderPrint_V2(req.Data);
                        break;

                    case "PARTNER_ORDER_CREATE_SHIPPING_CODE":
                        res = PartnerRepository.CreateShippingCode(req.Data);
                        break;
                    #endregion

                    #region PARTNER_TRANSPORT_SERVICE
                    case "PARTNER_TRANSPORT_SERVICE_SEARCH_LIST":
                        res = PartnerTransportServiceRepository.SearchList(req.Data);
                        break;
                    #endregion

                    #region PARTNER_TRANSPORT_POSTAGE
                    case "PARTNER_TRANSPORT_POSTAGE_CHARGED":
                        res = TransportPostageRepository.Charged(req.Data);
                        break;
                    #endregion

                    #region PARTNER_WAREHOUSE
                    case "PARTNER_WAREHOUSE_SEARCH_LIST":
                        res = WareHouseRepository.SearchList(req.Data);
                        break;

                    case "PARTNER_WAREHOUSE_ADD":
                        res = WareHouseRepository.Add(req.Data);
                        break;
                    #endregion

                    #region PARTNER_PRODUCT
                    case "PARTNER_PRODUCT_SEARCH_LIST":
                        res = ProductRepository.SearchList(req.Data);
                        break;
                    #endregion

                    #region PARTNER_ADDRESS
                    case "PARTNER_ADDRESS_SEARCH":
                        res = AddressRepository.SearchAddress(req.Data);
                        break;
                    #endregion

                    #region PARTNER_PROVINCE
                    case "PARTNER_PROVINCE_SEARCH_LIST":
                        res = ProvinceRepository.SearchListProvince(req.Data);
                        break;
                    #endregion

                    #region PARTNER_DISTRICT
                    case "PARTNER_DISTRICT_GET_BY_PROVINCE_ID":
                        res = DistrictRepository.GetByProvinceID(req.Data);
                        break;
                    #endregion

                    #region PARTNER_WARD
                    case "PARTNER_WARD_GET_BY_DISTRICT_ID":
                        res = WardRepository.GetByDistrictID(req.Data);
                        break;

                    case "PARTNER_WARD_GET_BY_PROVINCE_ID":
                        res = WardRepository.GetByProvinceID(req.Data);
                        break;
                    #endregion

                    default:
                        res.Code = "01";
                        res.Message = "Không tìm thấy thông tin mã hàm.";
                        break;
                }

                return res;
            }
            catch(Exception ex)
            {
                capturedException = ex;
                statusCode = 500;

                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                return res;
            }
            finally
            {
                watch.Stop();

                if(shouldLog)
                {
                    try
                    {
                        XMongoLogHelper.Enqueue(new ZApiLogRequest
                        {
                            CollectionName = collectionName,
                            TraceId = traceId,
                            Method = "POST",
                            Url = url,
                            Code = req != null ? req.Code : null,
                            RequestBody = requestBody,
                            ResponseBody = JsonConvert.SerializeObject(res),
                            StatusCode = statusCode,
                            IsSuccess = string.Equals(res?.Code, "00"),
                            DurationMs = watch.ElapsedMilliseconds,
                            ExceptionMessage = capturedException != null ? capturedException.ToString() : null,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                    catch
                    {
                        // Không để lỗi log ảnh hưởng đến API chính
                    }
                }
            }
        }

        [HttpPost]
        [Route("ExecuteV2")]
        public ZResponse ExecuteV2(ZRequestV2 req)
        {
            ZResponse res = new ZResponse();
            string traceId = Guid.NewGuid().ToString("N");
            Stopwatch watch = Stopwatch.StartNew();

            string url = "/Gateway/ExecuteV2";
            string collectionName = "EmsOneLog";
            bool shouldLog = false;
            string requestBody = string.Empty;
            Exception capturedException = null;
            int statusCode = 200;

            try
            {
                requestBody = JsonConvert.SerializeObject(req);

                shouldLog = req != null && !string.IsNullOrWhiteSpace(req.Code) && XMongoLogHelper.ShouldLog(req.Code);
                if(shouldLog)
                {
                    collectionName = XMongoLogHelper.GetCollectionName(req.Code);
                }

                if(req == null)
                {
                    res.Code = "01";
                    res.Message = "Request không hợp lệ.";
                    return res;
                }

                if(req.Code == "PARTNER_GET_ACCESS")
                {
                    var shopInformation = ShopRepository.SearchByToken(req.PublicKey);
                    if(shopInformation == null)
                    {
                        res.Code = "403";
                        res.Message = "PublicKey không tồn tại";
                        return res;
                    }

                    var reqData = JsonConvert.DeserializeObject<ZEmployeeLoginRequest>(req.Data);
                    var shopInformationDetail = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(shopInformation.Data);

                    if(reqData == null || shopInformationDetail == null)
                    {
                        res.Code = "01";
                        res.Message = "Dữ liệu không hợp lệ.";
                        return res;
                    }

                    if(reqData.MobileNumber != shopInformationDetail.MobileNumber)
                    {
                        res.Code = "403";
                        res.Message = "PublicKey không tồn tại";
                        return res;
                    }

                    string error;
                    if(!XSecurity.LoginVerify(
                            req.Data,
                            req.PublicKey,
                            req.TimeStamp,
                            req.Signature,
                            XHelper.EMSOneSecretKey,
                            out error))
                    {
                        res.Code = "02";
                        res.Message = "Chữ ký số không hợp lệ";
                        return res;
                    }

                    res = EmployeeRepository.EmployeeLogin(req.Data);
                    return res;
                }

                switch(req.Code)
                {
                    #region PARTNER_ORDER
                    case "PARTNER_ORDER_ADD":
                        res = PartnerRepository.OrderAdd(req.Data);
                        break;

                    case "PARTNER_VIB_ORDER_ADD":
                        res = PartnerRepository.VIBOrderAdd(req.Data);
                        break;

                    case "PARTNER_HM_ORDER_ADD":
                        res = PartnerRepository.HMOrderAdd(req.Data);
                        break;

                    case "PARTNER_BIDV_ORDER_ADD":
                        res = PartnerRepository.VIBOrderAdd(req.Data);
                        break;

                    case "PARTNER_VINAPHONE_ORDER_ADD":
                        res = PartnerRepository.VinaphoneOrderAdd(req.Data);
                        break;

                    case "PARTNER_TEMU_ORDER_ADD":
                        res = PartnerRepository.TemuOrderAdd(req.Data);
                        break;

                    case "PARTNER_ORDER_CANCEL":
                        res = PartnerRepository.OrderCancel(req.Data);
                        break;

                    case "PARTNER_ORDER_INFO":
                        res = PartnerRepository.OrderInfo(req.Data);
                        break;

                    case "PARTNER_ORDER_PRINT":
                        res = PartnerRepository.OrderPrint(req.Data);
                        break;

                    case "PARTNER_ORDER_PRINT_V2":
                        res = PartnerRepository.OrderPrint_V2(req.Data);
                        break;

                    case "PARTNER_ORDER_CREATE_SHIPPING_CODE":
                        res = PartnerRepository.CreateShippingCode(req.Data);
                        break;
                    #endregion

                    #region PARTNER_TRANSPORT_SERVICE
                    case "PARTNER_TRANSPORT_SERVICE_SEARCH_LIST":
                        res = PartnerTransportServiceRepository.SearchList(req.Data);
                        break;
                    #endregion

                    #region PARTNER_TRANSPORT_POSTAGE
                    case "PARTNER_TRANSPORT_POSTAGE_CHARGED":
                        res = TransportPostageRepository.Charged(req.Data);
                        break;
                    #endregion

                    #region PARTNER_WAREHOUSE
                    case "PARTNER_WAREHOUSE_SEARCH_LIST":
                        res = WareHouseRepository.SearchList(req.Data);
                        break;

                    case "PARTNER_WAREHOUSE_ADD":
                        res = WareHouseRepository.Add(req.Data);
                        break;
                    #endregion

                    #region PARTNER_PRODUCT
                    case "PARTNER_PRODUCT_SEARCH_LIST":
                        res = ProductRepository.SearchList(req.Data);
                        break;
                    #endregion

                    #region PARTNER_ADDRESS
                    case "PARTNER_ADDRESS_SEARCH":
                        res = AddressRepository.SearchAddress(req.Data);
                        break;
                    #endregion

                    #region PARTNER_PROVINCE
                    case "PARTNER_PROVINCE_SEARCH_LIST":
                        res = ProvinceRepository.SearchListProvince(req.Data);
                        break;
                    #endregion

                    #region PARTNER_DISTRICT
                    case "PARTNER_DISTRICT_GET_BY_PROVINCE_ID":
                        res = DistrictRepository.GetByProvinceID(req.Data);
                        break;
                    #endregion

                    #region PARTNER_WARD
                    case "PARTNER_WARD_GET_BY_DISTRICT_ID":
                        res = WardRepository.GetByDistrictID(req.Data);
                        break;

                    case "PARTNER_WARD_GET_BY_PROVINCE_ID":
                        res = WardRepository.GetByProvinceID(req.Data);
                        break;
                    #endregion

                    default:
                        res.Code = "01";
                        res.Message = "Không tìm thấy thông tin mã hàm.";
                        break;
                }

                return res;
            }
            catch(Exception ex)
            {
                capturedException = ex;
                statusCode = 500;

                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                return res;
            }
            finally
            {
                watch.Stop();

                if(shouldLog)
                {
                    try
                    {
                        XMongoLogHelper.Enqueue(new ZApiLogRequest
                        {
                            CollectionName = collectionName,
                            TraceId = traceId,
                            Method = "POST",
                            Url = url,
                            Code = req != null ? req.Code : null,
                            RequestBody = requestBody,
                            ResponseBody = JsonConvert.SerializeObject(res),
                            StatusCode = statusCode,
                            IsSuccess = string.Equals(res?.Code, "00"),
                            DurationMs = watch.ElapsedMilliseconds,
                            ExceptionMessage = capturedException != null ? capturedException.ToString() : null,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                    catch
                    {
                        // Không throw ngược lỗi log để tránh ảnh hưởng luồng API chính
                    }
                }
            }
        }

    }
}
