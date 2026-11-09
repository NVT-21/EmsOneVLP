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
using EMS.API.Core.Controllers;

namespace EMS.API.Core.Repositories
{
    public class GrabOrderRepository
    {
        public static ZResponse GrabDeliveryAddOrder(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                var resAddOrder = OrderRepository.AddOrderV2(req);
                if (resAddOrder.Code == "00")
                {
                    var OrderID = Convert.ToInt32(resAddOrder.Data);
                    var resSendReqGrabDelivery = XCommon.SendReqGrabDelivery(OrderID, req.ShopID, "I", req.PromoCode);
                    if (resSendReqGrabDelivery.Code == "00")
                    {
                        var reqUpdate = JsonConvert.DeserializeObject<GrabUpdateTransportSuccessRequest>(resSendReqGrabDelivery.Data);
                        var resGrabDeliveryUpdateSuccess = GrabDeliveryUpdateSuccess(OrderID, req.ShopID, req.CreatedBy, reqUpdate.itemcode, reqUpdate.id);
                        if (resGrabDeliveryUpdateSuccess.Code == "00")
                        {
                            res.Code = resSendReqGrabDelivery.Code;
                            res.Message = resSendReqGrabDelivery.Message;
                        }
                        else
                        {
                            res.Code = resGrabDeliveryUpdateSuccess.Code;
                            res.Message = resGrabDeliveryUpdateSuccess.Message;
                            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabOrderRepository.GrabDeliveryAddOrder_resSendTransportSuccess: request={0}||response={1}", OrderID, JsonConvert.SerializeObject(resGrabDeliveryUpdateSuccess)));
                        }
                    }
                    else
                    {
                        var resUpdateTransportError = OrderRepository.SendTransportError(new ZOrderSendTransportErrorRequest() { AmndState = "I", OrderID = OrderID, ShopID = req.ShopID });
                        if (resUpdateTransportError.Code == "00")
                        {
                            res.Code = resSendReqGrabDelivery.Code;
                            res.Message = resSendReqGrabDelivery.Message;
                        }
                        else
                        {
                            res.Code = resUpdateTransportError.Code;
                            res.Message = resUpdateTransportError.Message;
                            XHelper.LogToFile(LogFileType.MESSAGE, string.Format("GrabOrderRepository.GrabDeliveryAddOrder_resSendTransportError: request={0}||response={1}", OrderID, JsonConvert.SerializeObject(resUpdateTransportError)));
                        }
                    }
                }
                else
                {
                    res.Code = resAddOrder.Code;
                    res.Message = resAddOrder.Message;
                    res.Data = resAddOrder.Data;
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderRepository.GrabDeliveryAddOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse GrabDeliveryUpdateSuccess(int OrderID, int ShopID, int CreatedBy, string ItemCode, string ShippingID)
        {
            ZResponse res = new ZResponse();
            try
            {
                int result = -1;
                using (DbCommand dbCommand = XHelper.Database.GetStoredProcCommand(XHelper.Schema + "EMS_GRAB_ORDER_PKG.UPDATE_DELIVERY_SUCCESS", new object[]
                {
                    0,
                    OrderID,
                    ShopID,
                    CreatedBy,
                    ItemCode,
                    ShippingID
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
                                    res.Message = "Đơn hàng không tồn tại.";
                                    break;
                                case -2:
                                    res.Code = "02";
                                    res.Message = "Lưu lịch sử đơn hàng thất bại";
                                    break;
                                case -3:
                                    res.Code = "03";
                                    res.Message = "Phiếu xuất kho không tồn tại.";
                                    break;
                                case -4:
                                    res.Code = "04";
                                    res.Message = "Lưu lịch sử phiếu xuất kho thất bại.";
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
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderRepository.GrabDeliveryUpdateSuccess: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }

        public static ZResponse GrabDeliveryCancelOrder(ZOrderCancelStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                List<ZOrderCancelStatusResponse> ListError = new List<ZOrderCancelStatusResponse>();
                var resSearchInfoReqCancel = OrderRepository.CancelTransportInfo(new ZOrderCancelTransportInfoRequest()
                {
                    OrderID = req.OrderID,
                    ShopID = req.ShopID,
                    CrmOrPaypostCode = req.CrmOrPaypostCode,
                    OrderCode = req.OrderCode,
                    ShippingCode = req.ShippingCode
                });
                if (resSearchInfoReqCancel.Code == "00")
                {
                    var itemSearchInfoReqCancel = JsonConvert.DeserializeObject<ZOrderCancelTransportInfoResponse>(resSearchInfoReqCancel.Data);
                    ZOrderCancelStatusRequest reqOrderCancelStatus = new ZOrderCancelStatusRequest()
                    {
                        OrderID = itemSearchInfoReqCancel.OrderID,
                        ReasonCancel = req.ReasonCancel,
                        ShopID = req.ShopID == 0 ? itemSearchInfoReqCancel.ShopID : req.ShopID,
                        UpdatedBy = req.UpdatedBy == 0 ? itemSearchInfoReqCancel.EmployeeID : req.UpdatedBy
                    };
                    if (itemSearchInfoReqCancel.Status == "N" || itemSearchInfoReqCancel.Status == "W" || itemSearchInfoReqCancel.Status == "P")
                    {
                        ZGrabDeliveryCancelRequest reqCancelGrabDelivery = new ZGrabDeliveryCancelRequest()
                        {
                            id = itemSearchInfoReqCancel.ShippingID
                        };
                        var resSendReqCancelGrabDelivery = XCommon.SendReqCancelGrabDelivery(reqCancelGrabDelivery);

                        if (resSendReqCancelGrabDelivery.Code == "00")
                        {
                            var resCancelStatus = OrderRepository.CancelOrderStatusGrab(reqOrderCancelStatus);
                            if (resCancelStatus.Code == "00")
                            {
                                res.Code = "00";
                                res.Message = "Hủy đơn hàng thành công";
                            }
                            else
                            {
                                res.Code = resCancelStatus.Code;
                                res.Message = resCancelStatus.Message;
                                res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoReqCancel.OrderCode, Description = resCancelStatus.Message });
                            }
                        }
                        else
                        {
                            res.Code = resSendReqCancelGrabDelivery.Code;
                            res.Message = resSendReqCancelGrabDelivery.Message;
                            res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoReqCancel.OrderCode, Description = resSendReqCancelGrabDelivery.Message });
                        }
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Trạng thái đơn hàng không hợp lệ.";
                        res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = itemSearchInfoReqCancel.OrderCode, Description = string.Format("Đơn hàng đang ở trạng thái {0}.", XData.GetNameOrderStatus(itemSearchInfoReqCancel.Status)) });
                    }
                }
                else
                {
                    res.Code = resSearchInfoReqCancel.Code;
                    res.Message = resSearchInfoReqCancel.Message;
                    res.Data = JsonConvert.SerializeObject(new ZOrderCancelStatusResponse() { OrderCode = req.OrderCode, Description = "Không tồn tại dữ liệu" });
                }
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GrabDeliveryCancelOrder: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return res;
        }
    }
}