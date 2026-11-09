using EMS.TransportWorkerService.Models;
using EMS.TransportWorkerService.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EMS.TransportWorkerService;

public static class XCommon
{
    public static async Task<ZResponse> SendTransportOrder(
        string AmndState,
        int OrderID,
        int ShopID,
        ILogger logger,
        EMSRepository emsRepository,
        IConfiguration configuration,
        OrderRepository orderRepository,
        PNSVnpostRepository pnsVnpostRepository,
        CancellationToken cancellationToken = default)
    {
        ZResponse res = new ZResponse();
        try
        {
            ApplyDebugForceOrder(configuration, logger, ref OrderID, ref ShopID);

            logger.LogInformation(
                "SendTransportOrder: Oracle SendTransportInfoVNPOST bắt đầu. OrderID={OrderID}, ShopID={ShopID}",
                OrderID,
                ShopID);

            var resSearchInfoSendTransport = orderRepository.SendTransportInfoVNPOST(
                new ZOrderSendTransportInfoRequest()
                {
                    OrderID = OrderID,
                    ShopID = ShopID,
                    AmndState = AmndState
                },
                logger);

            logger.LogInformation(
                "SendTransportOrder: Oracle SendTransportInfoVNPOST xong. OrderID={OrderID}, Code={Code}, Message={Message}",
                OrderID,
                resSearchInfoSendTransport.Code,
                resSearchInfoSendTransport.Message);

            if (resSearchInfoSendTransport.Code != "00")
            {
                res.Code = resSearchInfoSendTransport.Code;
                res.Message = resSearchInfoSendTransport.Message;
                return res;
            }

            var data = JsonConvert.DeserializeObject<ZOrderSendTransportInfoVNPOSTResponse>(resSearchInfoSendTransport.Data);
            if (data == null)
            {
                res.Code = "99";
                res.Message = "Không đọc được dữ liệu đơn hàng.";
                return res;
            }

            if (data.SpecialService == "TMD_B" || data.SpecialService == "TMDHCM")
            {
                data.SpecialService = "TMD";
            }

            var pushVnpost = pnsVnpostRepository.IsEnabled();
            if (!pushVnpost)
            {
                logger.LogWarning(
                    "PNS Vnpost bị TẮT (thiếu PNSVnpost:ApiUrl/ApiKey hoặc Enabled=false). Chỉ đẩy EMS. OrderID={OrderID}.",
                    OrderID);
            }

            logger.LogInformation(
                "SendTransportOrder: đẩy PNS EMS{Vnpost} (song song). OrderID={OrderID}, ShopID={ShopID}",
                pushVnpost ? " + PNS Vnpost" : "",
                OrderID,
                ShopID);

            var emsTask = PushToPNSEMSAsync(data, emsRepository, configuration, logger, cancellationToken);
            var vnpostTask = pushVnpost
                ? PushToPNSVnpostAsync(data, pnsVnpostRepository, configuration, logger, cancellationToken)
                : Task.FromResult<ZResponse>(null);

            await Task.WhenAll(emsTask, vnpostTask).ConfigureAwait(false);

            var resEms = await emsTask.ConfigureAwait(false);
            var resVnpost = pushVnpost ? await vnpostTask.ConfigureAwait(false) : null;

            var emsOk = resEms.Code == "00" || resEms.Code == "01";
            var vnpostOk = !pushVnpost
                || resVnpost != null && (resVnpost.Code == "00" || resVnpost.Code == "01");

            if (emsOk && vnpostOk)
            {
                res.Code = "00";
                res.Message = pushVnpost
                    ? "Tạo đơn hàng và gửi vận chuyển PNS Vnpost + PNS EMS thành công"
                    : "Tạo đơn hàng và gửi vận chuyển thành công";
            }
            else if (!emsOk && pushVnpost && !vnpostOk)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối API EMS và PNS Vnpost, vui lòng liên hệ quản trị.";
            }
            else if (!emsOk)
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối API vận chuyển EMS, vui lòng liên hệ quản trị.";
            }
            else
            {
                res.Code = "99";
                res.Message = "Lỗi kết nối API PNS Vnpost, vui lòng liên hệ quản trị.";
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "XCommon.SendTransportOrder failed");
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        }

        return res;
    }

    /// <summary>
    /// Đẩy đơn sang PNS VNPOST (Partner/EMS/CreateOrder). Log Mongo: PushPNSVNPOST.
    /// </summary>
    public static async Task<ZResponse> PushToPNSVnpostAsync(
        ZOrderSendTransportInfoVNPOSTResponse data,
        PNSVnpostRepository pnsVnpostRepository,
        IConfiguration configuration,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        var orderPush = BuildOrderPush(data, configuration);

        logger.LogInformation(
            "PushToPNSVnpost: bắt đầu. ItemCode={ItemCode}",
            orderPush.ItemCode);

        var res = await pnsVnpostRepository.CreateOrderAsync(orderPush, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "PushToPNSVnpost: xong. ItemCode={ItemCode}, Code={Code}, Message={Message}",
            orderPush.ItemCode,
            res.Code,
            res.Message);

        logger.LogTrace(
            "PushToPNSVnpost: request={Request}||response={Response}",
            JsonConvert.SerializeObject(orderPush.ToApiBody()),
            JsonConvert.SerializeObject(res));

        return res;
    }

    /// <summary>
    /// Đẩy đơn sang PNS EMS (api/Pickup/CreatePickupv1). Log Mongo: PushPNSEMS.
    /// </summary>
    public static async Task<ZResponse> PushToPNSEMSAsync(
        ZOrderSendTransportInfoVNPOSTResponse data,
        EMSRepository emsRepository,
        IConfiguration configuration,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        var reqPickupCreate = BuildEmsPickupRequest(data, configuration);
        var requestJson = JsonConvert.SerializeObject(reqPickupCreate);

        logger.LogInformation(
            "PushToPNSEMS: bắt đầu. TrackingCode={TrackingCode}",
            data.TrackingCode);

        var res = await emsRepository.EMSSendRequestTransportAsync(requestJson, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation(
            "PushToPNSEMS: xong. TrackingCode={TrackingCode}, Code={Code}",
            data.TrackingCode,
            res.Code);

        logger.LogTrace(
            "PushToPNSEMS: request={Request}||response={Response}",
            requestJson,
            JsonConvert.SerializeObject(res));

        return res;
    }

    private static void ApplyDebugForceOrder(
        IConfiguration configuration,
        ILogger logger,
        ref int orderId,
        ref int shopId)
    {
        if (!configuration.GetValue("Worker:DebugForceOrder:Enabled", false))
            return;

        var debugOrderId = configuration.GetValue("Worker:DebugForceOrder:OrderID", 0);
        var debugShopId = configuration.GetValue("Worker:DebugForceOrder:ShopID", 0);
        if (debugOrderId <= 0 || debugShopId <= 0)
            return;

        logger.LogWarning(
            "DebugForceOrder: ghi đè OrderID/ShopID từ config. Message OrderID={MsgOrderID}, ShopID={MsgShopID} → Config OrderID={OrderID}, ShopID={ShopID}",
            orderId,
            shopId,
            debugOrderId,
            debugShopId);

        orderId = debugOrderId;
        shopId = debugShopId;
    }

    private static OrderPush BuildOrderPush(
        ZOrderSendTransportInfoVNPOSTResponse data,
        IConfiguration configuration)
    {
        var customerCode = data.CustomerCode ?? data.SenderCode ?? "";
        var productCode = data.ProductCode ?? data.OrderCode ?? "";
        var productDescription = data.ProductDescription ?? "";

        return new OrderPush
        {
            ItemCode = data.TrackingCode ?? "",
            SenderProvince = data.SenderProvinceId.ToString(),
            SenderDistrict = data.SenderDistrictId.ToString(),
            SenderWard = data.SenderWardId.ToString(),
            SenderAddress = data.SenderAddress ?? "",
            SenderName = data.SenderName ?? "",
            SenderEmail = data.SenderEmail ?? "",
            SenderTel = data.SenderPhone ?? "",
            SenderDesc = productDescription,
            Description = productDescription,
            ExtendData = "",
            ReceiverName = data.ReceiverName ?? "",
            ReceiverProvince = data.ReceiverProvinceId.ToString(),
            ReceiverDistrict = data.ReceiveDistrictId.ToString(),
            ReceiverWard = data.ReceiveWardId,
            ReceiverAddress = data.ReceiveAddress ?? "",
            ReceiverEmail = data.ReceiverEmail ?? "",
            ReceiverTel = data.ReceiverPhone ?? "",
            CustomerCode = customerCode,
            PosIdCollect = data.SenderProvinceId.ToString(),
            FlagConfig = data.FlagConfig == "2" ? "1" : "2",
            PartnerId = data.Id.ToString(),
            PartnerSourceId = data.Code,
            Weight = data.Weight.ToString(),
            OrderNumber = data.OrderCode ?? "",
            CODAmount = data.Cod.ToString(),
            AdditionService = data.AdditionService ?? "",
            SpecialService = data.SpecialService ?? "",
            ServiceFee = data.Service_Fee == 0 ? null : data.Service_Fee,
            PickupDate = 0,
            PickupHours = 0,
            ValueAddedList = data.ValueAddedList ?? "",
            ServiceCode = data.ServiceCode ?? "",
            ReceiverCode = data.ReceiverCode ?? "",
            RecContractNumber = data.CONTRACT_CODE_C ?? ""
        };
    }

    private static ZTransportInfoShippingApiRequest BuildEmsPickupRequest(
        ZOrderSendTransportInfoVNPOSTResponse data,
        IConfiguration configuration)
    {
        var now = DateTime.Now;
        var pickupDate = now.ToString("yyyyMMdd");
        var customerCode = data.CustomerCode ?? data.SenderCode ?? "";
        var productCode = data.ProductCode ?? data.OrderCode ?? "";
        var productDescription = data.ProductDescription ?? "";
        var additionService = data.AdditionService ?? "";
        var weight = data.Weight > int.MaxValue ? int.MaxValue : (int)data.Weight;

        var listItem = new List<ZListItemInfoShippingRequest>
        {
            new ZListItemInfoShippingRequest()
            {
                TRACKING_CODE = data.TrackingCode,
                SENDER_NAME = data.SenderName,
                SENDER_CODE = data.SenderCode ?? customerCode,
                SENDER_PHONE = data.SenderPhone,
                SENDER_STREET = data.SenderAddress,
                SENDER_ADDRESS = data.SenderAddress,
                SENDER_PROVINCE_ID = data.SenderProvinceId.ToString(),
                SENDER_DISTRICT_ID = data.SenderDistrictId.ToString(),
                SENDER_WARD_ID = data.SenderWardId.ToString(),
                SENDER_EMAIL = data.SenderEmail,
                RECEIVER_NAME = data.ReceiverName,
                RECEIVER_PROVINCE_ID = data.ReceiverProvinceId.ToString(),
                RECEIVER_PHONE = data.ReceiverPhone,
                RECEIVER_DISTRICT_ID = data.ReceiveDistrictId.ToString(),
                RECEIVER_WARD_ID = data.ReceiveWardId.ToString(),
                RECEIVER_EMAIL = data.ReceiverEmail ?? "",
                RECEIVER_STREET = data.ReceiveAddress,
                RECEIVER_ADDRESS = data.ReceiveAddress,
                PRODUCT_NAME = data.ProductName,
                PRODUCT_VALUE = Convert.ToInt32(data.ProductValue),
                PRODUCT_CODE = productCode,
                PRODUCT_DESCRIPTION = productDescription,
                PRODUCT_QUANTITY = data.ProductQuantity,
                WEIGHT = weight,
                ADDITION_SERVICE = additionService,
                SPECIAL_SERVICE = additionService,
                TOTAL_AMOUNT = data.TotalAmount,
                CUSTOMER_CODE = customerCode,
                TO_COUNTRY = string.IsNullOrEmpty(data.ToCountryCode) ? "VN" : data.ToCountryCode,
                PO_CREATE = string.IsNullOrEmpty(data.Po_Create) ? "0100" : data.Po_Create,
                COMPLETE = data.Complete,
                POSTAL_PROVINCE_CODE = data.ReceiverProvinceId.ToString(),
                POSTAL_DISTRICT_CODE = data.ReceiveDistrictId.ToString(),
                POSTAL_COMMUNE_CODE = data.ReceiveWardId.ToString(),
                POSTAL_CODE = data.ToZipCode ?? "",
                SERVICE_TYPE = data.ServiceType,
                SERVICE_FEE = data.Service_Fee,
                CLASSIFY = "",
                IS_CONSIGNMENT = data.IsConsorShip ? 1 : 0,
                CHANNEL = string.IsNullOrEmpty(data.Channel) ? "ONE" : data.Channel
            }
        };

        return new ZTransportInfoShippingApiRequest()
        {
            PICKUP_CODE_API = "",
            PO_CODE = data.Po_Create ?? "",
            DESCRIPTION = "",
            CUSTOMER_CODE = customerCode,
            CUSTOMER_PHONE = data.CustomerPhone ?? data.SenderPhone,
            CUSTOMER_NAME = data.CustomerName ?? data.SenderName,
            CUSTOMER_EMAIL = data.CustomerEmail ?? data.SenderEmail,
            STOREID = data.StoreId > 0 ? data.StoreId.ToString() : "",
            PICKUP_PROVINCE_ID = data.SenderProvinceId,
            PICKUP_DISTRICT_ID = data.SenderDistrictId,
            PICKUP_WARD_ID = data.SenderWardId,
            PICKUP_STREET = data.SenderAddress,
            PICKUP_DATE = pickupDate,
            PICKUP_HOURS = now.ToString("HHmm"),
            CONTACT_NAME = data.SenderName,
            CONTACT_PHONE = data.SenderPhone,
            PICKUP_POSTMAN = "",
            PICKUP_POCODE = data.Po_Create ?? "",
            PICKUP_CONFIG = data.FlagConfig == "2" ? "1" : "2",
            SignatureCode = Security.SHA256(customerCode + (configuration["EMS:TransportKey"] ?? string.Empty)).ToUpper(),
            LIST_ITEM = listItem
        };
    }
}
