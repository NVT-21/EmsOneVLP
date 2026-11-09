
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class OrderRepository
    {
        public static ZResponse SearchProduct(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderSearchProductRequest>(data);
            return XHelper.Invoke("POST", "Order/SearchProduct", req);
        }

        public static ZResponse SearchListExcelImport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("Order/GetImportJobsByShopID?ShopID={0}",req.ShopID),null);
        }

        public static ZResponse OrderImportSearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderImportSearchListRequest>(data);
            return XHelper.Invoke("POST", "Order/OrderImportSearchList", req);
        }

        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Order/Add", req);
        }

        public static ZResponse AddOrderAndSendTransport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Order/AddOrderAndSendTransport", req);
        }

        public static ZResponse AddV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderAddRequest>(data);
            req.isSendTransport = "N";
            if (!string.IsNullOrEmpty(req.EMSOneCode))
            {
                return XHelper.Invoke("POST", "Order/Edit", req);
            }
            return XHelper.Invoke("POST", "Order/AddV2", req);
        }

        public static ZResponse ReturnOrderAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderAdd", req);
        }

        public static ZResponse AddOrderAndSendTransportV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderAddRequest>(data);
            req.isSendTransport = "Y";
            return XHelper.Invoke("POST", "Order/AddOrderAndSendTransportV2", req);
        }

        public static ZResponse OrderEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderEditRequest>(data);
            return XHelper.Invoke("POST", "Order/Edit", req);
        }

        public static ZResponse OrderSendTransport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderMultipleSendTransportRequest>(data);
            return XHelper.Invoke("POST", "Order/SendTransport", req);
        }

        //public static ZResponse OrderSendTransportWEB(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZOrderMultipleSendTransportRequest>(data);
        //    return XHelper.Invoke("POST", "Order/SendTransportWEB", req);
        //}
        
        public static ZResponse OrderCancelTransport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderMultipleCancelTransportRequest>(data);
            return XHelper.Invoke("POST", "Order/CancelTransport", req);
        }

        public static ZResponse FESearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderFESearchListRequest>(data);
            return XHelper.Invoke("POST", "Order/FESearchList", req);
        }

        public static ZResponse FEPaginationSearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderFEPaginationSearchListRequest>(data);
            return XHelper.Invoke("POST", "Order/FEPaginationSearchList", req);
        }

        public static ZResponse FEPaginationExpandSearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderFEPaginationSearchListRequest>(data);
            return XHelper.Invoke("POST", "Order/FEPaginationExpandSearchList", req);
        }

        public static ZResponse BESearchList(string data)
        {

            var req = JsonConvert.DeserializeObject<ZOrderBESearchListRequest>(data);
            if(XHelper.IsSqlInjection(req.OrderStatus) || XHelper.IsSqlInjection(req.OrderCode) || XHelper.IsSqlInjection(req.ShippingCode) || XHelper.IsSqlInjection(req.ShopCode) || XHelper.IsSqlInjection(req.CrmOrPaypostCode) || XHelper.IsSqlInjection(req.TransportExtraService) || XHelper.IsSqlInjection(req.PlaceOfCreateOrder) || XHelper.IsSqlInjection(req.Channel))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "Order/BESearchList", req);
        }

        //public static ZResponse ChangeStatus(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZOrderChangeStatusRequest>(data);
        //    return XHelper.Invoke("POST", "Order/ChangeStatus", req);
        //}

        public static ZResponse CancelStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderMultipleCancelStatusRequest>(data);
            return XHelper.Invoke("POST", "Order/CancelStatus", req);
        }

        public static ZResponse NewStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderMultipleNewStatusRequest>(data);
            return XHelper.Invoke("POST", "Order/NewStatus", req);
        }

        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("Order/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }

        public static ZResponse GetByShippingCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderGetByShippingCodeRequest>(data);
            return XHelper.Invoke("GET", string.Format("Order/GetByShippingCode?ShippingCode={0}&ShopID={1}", req.ShippingCode, req.ShopID), null);
        }

        public static ZResponse DetailByOrderID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderDetailByOrderIDRequest>(data);
            return XHelper.Invoke("POST", "Order/DetailByOrderID", req);
        }

        public static ZResponse DetailByShippingCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderDetailByShippingCodeRequest>(data);
            return XHelper.Invoke("POST", "Order/DetailByShippingCode", req);
        }

        public static ZResponse DetailByOrderCodeHM(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderGetByOrderCodeRequest>(data);
            return XHelper.Invoke("POST", "Order/DetailByOrderCodeHM", req);
        }

        //public static ZResponse CheckOrderTransport(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZOrderCheckTransportRequest>(data);
        //    return XHelper.Invoke("POST", "Order/CheckOrderTransport", req);
        //}

        public static ZResponse CoppyOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderCoppyRequest>(data);
            return XHelper.Invoke("POST", "Order/CoppyOrder", req);
        }

        //public static ZResponse SearchInfoSendRequestTransport(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZOrderSearchInfoSendRequestTransportRequest>(data);
        //    return XHelper.Invoke("POST", "Order/SearchInfoSendRequestTransport", req);
        //}

        //public static ZResponse CancelRequestTransport(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZOrderCancelRequestTransportRequest>(data);
        //    return XHelper.Invoke("POST", "Order/CancelRequestTransport", req);
        //}

        public static ZResponse SuccessHistory(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderSuccessHistoryRequest>(data);
            return XHelper.Invoke("POST", "Order/SuccessHistory", req);
        }

        public static ZResponse Print(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderPrintRequest>(data);
            return XHelper.Invoke("POST", "Order/Print", req);
        }

        public static ZResponse PrintSingle(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderSinglePrintRequest>(data);
            return XHelper.Invoke("POST", "Order/PrintSingle", req);
        }

        public static ZResponse SearchHistory(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderSearchHistoryRequest>(data);
            return XHelper.Invoke("POST", "Order/SearchHistory", req);
        }

        //public static ZResponse CheckOrderCode(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZOrderCheckCodeRequest>(data);
        //    return XHelper.Invoke("POST", "Order/CheckOrderCode", req);
        //}
        public static ZResponse UpdatePrintStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderUpdatePrintStatusRequest>(data);
            return XHelper.Invoke("POST", "Order/UpdatePrintStatus", req);
        }

        public static ZResponse EditReceiverAddress(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderEditReceiverAddressRequest>(data);
            return XHelper.Invoke("POST", "Order/EditReceiverAddress", req);
        }

        public static ZResponse Pagination(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderPaginationRequest>(data);
            return XHelper.Invoke("POST", "Order/Pagination", req);
        }

        public static ZResponse OrderImportExcel(HttpPostedFile file, string code, string data)
        {
            return XHelper.InvokeMultipart(
                "POST",
                "Order/OrderImportExcel",
                file,
                code,
                data
            );
        }
    }
}