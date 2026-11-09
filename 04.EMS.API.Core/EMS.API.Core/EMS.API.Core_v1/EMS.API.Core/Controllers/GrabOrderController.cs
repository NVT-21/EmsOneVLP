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
    [RoutePrefix("GrabOrder")]
    public class GrabOrderController : ApiController
    {
        [Route("GrabDeliveryAddOrder")]
        public ZResponse GrabDeliveryAddOrder(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = GrabOrderRepository.GrabDeliveryAddOrder(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderController.GrabDeliveryAddOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GrabDeliveryCancelOrder")]
        public ZResponse GrabDeliveryCancelOrder(ZOrderCancelStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = GrabOrderRepository.GrabDeliveryCancelOrder(req);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderController.GrabDeliveryCancelOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
        [Route("GrabDeliveryListOrder")]
        public ZResponse GrabDeliveryListOrder(ZGrabOrderSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                List<ZGrabOrderSearchListResponse> listItem = new List<ZGrabOrderSearchListResponse>();
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_GRAB_ORDER_PKG.GRAB_ORDER_SEARCH_LIST", new object[]
                {
                    req.FromDate,           //P_FROM_DATE                   VARCHAR 
                    req.ToDate,             //P_TO_DATE                     VARCHAR
                    req.ListCustomerCode,   //P_LIST_CRM_OR_PAYPOST_CODE    VARCHAR
                    req.OrderStatus,        //P_ORDER_STATUS                VARCHAR
                    req.SearchKey,          //P_SEARCH_KEY                  VARCHAR
                    req.ShopID,             //P_SHOP_ID                     NUMBER
                    0,                      //P_RETURN_CODE                 OUT NUMBER
                    0                       //P_OUT_CURSOR                  OUT T_CURSOR
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
                                    listItem.Add(new ZGrabOrderSearchListResponse()
                                    {
                                        OrderID = Convert.ToInt32(dr["ORDER_ID"]),
                                        EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                        OrderCode = dr["ORDER_CODE"].ToString(),
                                        SenderName = dr["SENDER_NAME"].ToString(),
                                        SenderPhone = dr["SENDER_PHONE"].ToString(),
                                        SenderProvinceID = Convert.ToInt32(dr["SENDER_PROVINCE_ID"]),
                                        SenderDistrictID = Convert.ToInt32(dr["SENDER_DISTRICT_ID"]),
                                        SenderWardID = Convert.ToInt32(dr["SENDER_WARD_ID"]),
                                        SenderAddress = dr["SENDER_STREET"].ToString(),
                                        SenderFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        ReceiverPhone = dr["RECEIVER_PHONE"].ToString(),
                                        ReceiverProvinceID = Convert.ToInt32(dr["RECEIVER_PROVINCE_ID"]),
                                        ReceiverProvinceName = dr["RECEIVER_PROVINCE_NAME"].ToString(),
                                        ReceiverDistrictID = Convert.ToInt32(dr["RECEIVER_DISTRICT_ID"]),
                                        ReceiverDistrictName = dr["RECEIVER_DISTRICT_NAME"].ToString(),
                                        ReceiverWardID = Convert.ToInt32(dr["RECEIVER_WARD_ID"]),
                                        ReceiverWardName = dr["RECEIVER_WARDS_NAME"].ToString(),
                                        ReceiverAddress = dr["RECEIVER_ADDRESS"].ToString(),
                                        ReceiverFullAddress = dr["RECEIVER_FULL_ADDRESS"].ToString(),
                                        Message = dr["MESSAGE"].ToString(),
                                        TotalCOD = Convert.ToInt64(dr["TOTAL_COD"]),
                                        TotalOrderValue = Convert.ToInt64(dr["TOTAL_ORDER_VALUE"]),
                                        TotalProductValue = Convert.ToInt64(dr["TOTAL_PRODUCT_VALUE"]),
                                        OrderStatus = dr["ORDER_STATUS"].ToString(),
                                        OrderStatusName = XData.GetNameOrderStatus(dr["ORDER_STATUS"].ToString()),
                                        CreatedDate = dr["CREATED_DATE"].ToString(),
                                        Transport = dr["TRANSPORT"].ToString(),
                                        TransportOrderName = dr["TRANSPORT_ORDER_NAME"].ToString(),
                                        TransportFee = Convert.ToInt64(dr["TRANSPORT_FEE"]),
                                        TransportWeight = Convert.ToInt64(dr["TRANSPORT_WEIGHT"]),
                                        TransportMainServiceCode = dr["TRANSPORT_MAIN_SERVICE_CODE"].ToString(),
                                        TransportMainServiceName = dr["TRANSPORT_MAIN_SERVICE_NAME"].ToString(),
                                        TransportExtraServiceCode = dr["TRANSPORT_EXTRA_SERVICE_CODE"].ToString(),
                                        TransportExtraServiceName = dr["TRANSPORT_EXTRA_SERVICE_NAME"].ToString(),
                                        PrintStatus = XData.GetNamePrintStatus(dr["PRINT_STATUS"].ToString()),

                                        ShopID = Convert.ToInt32(dr["SHOP_ID"]),
                                        ShopCode = dr["SHOP_CODE"].ToString(),
                                        CrmOrPaypostCode = dr["CRM_OR_PAYPOST_CODE"].ToString(),

                                        CustomerCode = dr["CUSTOMER_CODE"].ToString(),
                                        CustomerName = dr["CUSTOMER_NAME"].ToString(),
                                        CustomerPhone = dr["CUSTOMER_PHONE"].ToString(),
                                        CustomerFullAddress = dr["CUSTOMER_FULL_ADDRESS"].ToString(),

                                        UpdatedName = dr["UPDATED_NAME"].ToString(),

                                        ShippingCode = dr["SHIPPING_CODE"].ToString(),

                                        PaymentFreightVat = Convert.ToInt64(dr["PAYMENT_FREIGHT_VAT"]),
                                        ShippingID = dr["SHIPPING_ID"].ToString()
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderController.GrabDeliveryListOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}