using System;
using System.Web;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class OrderRepository
    {
        public static ZResponse SearchListOrder(ZOrderFESearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchListOrder: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListOrderPagination(ZOrderFEPaginationSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_PAGINATION_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchListOrder: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListOrderExpandPagination(ZOrderFEPaginationSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_PAGINATION_EXPAND_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchListOrder: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListProduct(ZOrderSearchProductRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_SEARCH_PRODUCT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchListProduct: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetByID(ZOrderGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GetByID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse DetailByOrderID(ZOrderDetailByOrderIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_DETAIL_BY_ORDER_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.DetailByOrderID: " + ex.Message);
            }
            return res;
        }

        public static ZResponse DetailByShippingCode(ZOrderDetailByShippingCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_DETAIL_BY_SHIPPING_CODE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.DetailByShippingCode: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetByShippingCode(ZOrderGetByShippingCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_GET_BY_SHIPPING_CODE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GetByShippingCode: " + ex.Message);
            }
            return res;
        }

        public static ZResponse GetByOrderHM(ZOrderGetByOrderCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_GET_BY_ORDER_CODE_HM",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.GetByShippingCode: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddOrder(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrder: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddOrderV2(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_ADD_V2",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrder: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddOrderAndSendTransport(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_ADD_AND_SEND_TRANSPORT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrderAndSendTransport: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddReturnOrderAndSendTransport(ZPartnerOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "RETURN_ORDER_ADD",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrderAndSendTransport: " + ex.Message);
            }
            return res;
        }

        public static ZResponse AddOrderAndSendTransportV2(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_ADD_AND_SEND_TRANSPORT_V2",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.AddOrderAndSendTransport: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditOrder(ZOrderEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.EditOrder: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SendTransport(ZOrderMultipleSendTransportRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_SEND_TRANSPORT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransport: " + ex.Message);
            }
            return res;
        }

        //public static ZResponse SendTransportWEB(ZOrderMultipleSendTransportRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "ORDER_SEND_TRANSPORT_WEB",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SendTransportWEB: " + ex.Message);
        //    }
        //    return res;
        //}

        public static ZResponse CancelTransport(ZOrderMultipleCancelTransportRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_CANCEL_TRANSPORT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelTransport: " + ex.Message);
            }
            return res;
        }

        public static ZResponse NewStatus(ZOrderMultipleNewStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_NEW_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.NewStatus: " + ex.Message);
            }
            return res;
        }

        public static ZResponse CancelStatus(ZOrderMultipleCancelStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_CANCEL_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.CancelStatus: " + ex.Message);
            }
            return res;
        }

        public static ZResponse PrintOrder(ZOrderPrintRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_PRINT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.PrintOrder: " + ex.Message);
            }
            return res;
        }

        public static ZResponse PrintSingleOrder(ZOrderSinglePrintRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_PRINT_SINGLE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.PrintOrder: " + ex.Message);
            }
            return res;
        }

        public static ZResponse UpdatePrintStatus(ZOrderUpdatePrintStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_UPDATE_PRINT_STATUS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.UpdatePrintStatus: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchOrderSuccessHistory(ZOrderSuccessHistoryRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_SUCCESS_HISTORY",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchOrderSuccessHistory: " + ex.Message);
            }
            return res;
        }

        public static ZResponse EditOrderReceiverAddress(ZOrderEditReceiverAddressRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_EDIT_RECEIVER_ADDRESS",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.EditOrderReceiverAddress: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListOrderPagination(ZOrderPaginationRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_PAGINATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchListOrderPagination: " + ex.Message);
            }
            return res;
        }
        public static ZResponse ImportExcelOrder(HttpPostedFileBase file, ZShopGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeMultipart(file,"ORDER_IMPORT_EXCEL", JsonConvert.SerializeObject(req));
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchOrderSuccessHistory: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListImportExcel(ZOrderGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_GET_EXCEL_LIST_BY_SHOP_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchListOrderPagination: " + ex.Message);
            }
            return res;
        }

        public static ZResponse SearchListOrderImport(ZOrderImportSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_IMPORT_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch(Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderRepository.SearchListOrderPagination: " + ex.Message);
            }
            return res;
        }
    }
}