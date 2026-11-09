using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class PartnerRepository
    {
        public static ZResponse OrderAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderAdd", req);
        }

        public static ZResponse OrderAddV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderAddV2Request>(data);
            return XHelper.Invoke("POST", "Partner/OrderAddV2", req);
        }

        public static ZResponse VIBOrderAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerVIBOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/VIBOrderAdd", req);
        }
        
        public static ZResponse AnalysticOrderAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerAnalysticOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/AnalysticOrderAdd", req);
        }
        public static ZResponse VinaphoneOrderAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerVinaphoneOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderAddProductBySKUVinaphone", req);
        }
        public static ZResponse InternationalOrderAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerInternalOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/InternationalOrderAdd", req);
        }
        public static ZResponse HMOrderAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerAnalysticOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/HMOrderAdd", req);
        }

        public static ZResponse TemuOrderAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerVIBOrderAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/TemuOrderAdd", req);
        }

        public static ZResponse VLPStatusAdd(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerVLPOrderStatusAddRequest>(data);
            return XHelper.Invoke("POST", "Partner/VLPStatusAdd", req);
        }


        //public static ZResponse OrderAdd(string data)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        var req = JsonConvert.DeserializeObject<ZPartnerOrderAddRequest>(data);
        //        ZOrderAddRequest finalRequest = new ZOrderAddRequest();
        //        List<object> messageError = new List<object>();
        //        var isError = XCommon.CheckValidateOrder(req, out messageError, out finalRequest);
        //        if (isError == true)
        //        {
        //            res.Code = "01";
        //            res.Message = "Dữ liệu không hợp lệ.";
        //            res.Data = JsonConvert.SerializeObject(messageError);
        //        }
        //        else
        //        {
        //            if (req.IsSendTransport == "N")
        //            {
        //                finalRequest.isSendTransport = "N";
        //                //res = XHelper.Invoke("POST", "Partner/OrderAdd", finalRequest);
        //                res = XHelper.Invoke("POST", "Order/AddV2", finalRequest);
        //            }
        //            else if (req.IsTransport == "Y" && req.IsSendTransport == "Y")
        //            {
        //                finalRequest.isSendTransport = "Y";
        //                //res = XHelper.Invoke("POST", "Partner/OrderAddAndSendTransport", finalRequest);
        //                res = XHelper.Invoke("POST", "Order/AddOrderAndSendTransportV2", finalRequest);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, string.Format("OrderRepository.OrderAdd: ex={0}||req={1}", ex.Message, data));
        //    }
        //    return res;
        //}

        public static ZResponse OrderCancel(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderCancelRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderCancel", req);
        }

        public static ZResponse OrderInfo(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderInfoRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderInfo", req); 
        }
        public static ZResponse OrderDeliveryInfo(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderInfoRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderDeliveryInfor", req);
        }

        public static ZResponse OrderPrint(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderPrintRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderPrint", req);
        }

        public static ZResponse OrderPrint_V2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderPrintRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderPrint_V2", req);
        }

        public static ZResponse OrderPrintEMS(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderPrintRequest>(data);
            return XHelper.Invoke("POST", "Partner/OrderPrint_V2", req);
        }

        public static ZResponse CreateShippingCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerOrderCreateShippingCodeRequest>(data);
            return XHelper.Invoke("POST", "Partner/CreateShippingCode", req);
        }
    }
}