using Dapper;
using Dapper.Oracle;
using EMS.TransportWorkerService.Models;
using EMS.TransportWorkerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Data.Common;
using System.Dynamic;

namespace EMS.TransportWorkerService.Repositories;

public class OrderRepository
{
    private readonly ILogger<OrderRepository> _logger;
    private readonly string _connectionString;
    private readonly EMSRepository _emsRepository;
    private readonly int _oracleCommandTimeoutSeconds;

    public OrderRepository(IConfiguration configuration, EMSRepository emsRepository,
        ILogger<OrderRepository> xlogger)
    {
        _logger = xlogger;
        _emsRepository = emsRepository;
        var cs = configuration.GetConnectionString("EMSONE") ?? configuration["ConnectionStrings:EMSONE"];
        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Connection string 'EMSONE' is not configured (ConnectionStrings:EMSONE).");

        var connectTimeout = configuration.GetValue("Worker:OracleConnectionTimeoutSeconds", 30);
        if (connectTimeout < 5)
            connectTimeout = 30;
        _connectionString = AppendOracleConnectionTimeoutIfMissing(cs.Trim(), connectTimeout);

        var t = configuration.GetValue("Worker:OracleCommandTimeoutSeconds", 180);
        _oracleCommandTimeoutSeconds = t < 5 ? 180 : t;
    }

    /// <summary>
    /// Sau thời gian idle, mở TCP tới Oracle có thể treo lâu nếu không có Connection Timeout.
    /// Chỉ bổ sung khi chuỗi kết nối chưa cấu hình sẵn.
    /// </summary>
    private static string AppendOracleConnectionTimeoutIfMissing(string connectionString, int seconds)
    {
        if (ConnectionStringHasConnectTimeout(connectionString))
            return connectionString;

        var sep = connectionString.EndsWith(';') ? "" : ";";
        return $"{connectionString}{sep}Connection Timeout={seconds}";
    }

    private static bool ConnectionStringHasConnectTimeout(string connectionString)
    {
        return connectionString.Contains("Connection Timeout", StringComparison.OrdinalIgnoreCase)
               || connectionString.Contains("CONNECT_TIMEOUT", StringComparison.OrdinalIgnoreCase)
               || connectionString.Contains("Connect Timeout", StringComparison.OrdinalIgnoreCase);
    }

    private void ApplyOracleCommandTimeout(OracleCommand cmd) =>
        cmd.CommandTimeout = _oracleCommandTimeoutSeconds;

    public ZResponse SendTransportInfo(ZOrderSendTransportInfoRequest req, ILogger logger)
    {
        ZResponse res = new ZResponse();
        try
        {
            ZOrderSendTransportInfoResponse item = null;
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using var cmd = connection.CreateCommand();
                ApplyOracleCommandTimeout(cmd);
                cmd.CommandText = "EMS_ORDER_PKG.SEND_TRANSPORT_INFO";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                cmd.Parameters.Add("P_ORDER_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.OrderID;
                cmd.Parameters.Add("P_SHOP_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.ShopID;
                cmd.Parameters.Add("P_AMND_STATE", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                    string.IsNullOrEmpty(req.AmndState) ? "A" : req.AmndState;

                var pReturn = new OracleParameter("P_RETURN_CODE", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(pReturn);

                var pCursor = new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                cmd.Parameters.Add(pCursor);

                cmd.ExecuteNonQuery();

                var result = GetOracleInt32Output(pReturn);
                if (result > 0)
                {
                    if (pCursor.Value is OracleRefCursor refCur)
                    {
                        using (var reader = refCur.GetDataReader())
                        {
                            var rows = ReadRowsAsDynamic(reader);
                            if (rows.Count > 0)
                            {
                                dynamic dr = rows[0];
                                item = new ZOrderSendTransportInfoResponse()
                                {
                                    CrmOrPayPostCode = dr.CRM_OR_PAYPOST_CODE?.ToString(),
                                    ShopPhone = dr.SHOP_PHONE?.ToString(),
                                    ShopStoreName = dr.SHOP_STORE_NAME?.ToString(),
                                    ShopFullName = dr.SHOP_FULL_NAME?.ToString(),
                                    ShopEmail = dr.SHOP_EMAIL?.ToString(),
                                    ShopID = string.IsNullOrEmpty(dr.SHOP_ID?.ToString()) ? 0 : Convert.ToInt32(dr.SHOP_ID),
                                    CollectionPostOfficeCode = dr.COLLECTION_POST_OFFICE_CODE?.ToString(),

                                    OrderID = string.IsNullOrEmpty(dr.ORDER_ID?.ToString()) ? 0 : Convert.ToInt32(dr.ORDER_ID),
                                    EMSOneCode = dr.EMSONE_CODE?.ToString(),
                                    SenderName = dr.SENDER_NAME?.ToString(),
                                    SenderPhone = dr.SENDER_PHONE?.ToString(),
                                    SenderStreet = dr.SENDER_STREET?.ToString(),
                                    SenderFullAddress = dr.SENDER_FULL_ADDRESS?.ToString(),
                                    SenderProvinceID = string.IsNullOrEmpty(dr.SENDER_PROVINCE_ID?.ToString()) ? 0 : Convert.ToInt32(dr.SENDER_PROVINCE_ID),
                                    SenderDistrictID = string.IsNullOrEmpty(dr.SENDER_DISTRICT_ID?.ToString()) ? 0 : Convert.ToInt32(dr.SENDER_DISTRICT_ID),
                                    SenderWardID = string.IsNullOrEmpty(dr.SENDER_WARD_ID?.ToString()) ? 0 : Convert.ToInt32(dr.SENDER_WARD_ID),
                                    SenderEmail = dr.SENDER_EMAIL?.ToString(),
                                    ReceiverName = dr.RECEIVER_NAME?.ToString(),
                                    ReceiverProvinceID = string.IsNullOrEmpty(dr.RECEIVER_PROVINCE_ID?.ToString()) ? 0 : Convert.ToInt32(dr.RECEIVER_PROVINCE_ID),
                                    ReceiverPhone = dr.RECEIVER_PHONE?.ToString(),
                                    ReceiverDistrictID = string.IsNullOrEmpty(dr.RECEIVER_DISTRICT_ID?.ToString()) ? 0 : Convert.ToInt32(dr.RECEIVER_DISTRICT_ID),
                                    ReceiverWardID = string.IsNullOrEmpty(dr.RECEIVER_WARD_ID?.ToString()) ? 0 : Convert.ToInt32(dr.RECEIVER_WARD_ID),
                                    ReceiverStreet = dr.RECEIVER_STREET?.ToString(),
                                    ReceiverFullAddress = dr.RECEIVER_FULL_ADDRESS?.ToString(),
                                    ProductName = dr.PRODUCT_NAME?.ToString(),
                                    ProductValue = string.IsNullOrEmpty(dr.PRODUCT_VALUE?.ToString()) ? 0 : Convert.ToInt64(dr.PRODUCT_VALUE),
                                    ProductItem = dr.PRODUCT_ITEM?.ToString(),
                                    ProductCode = dr.PRODUCT_CODE?.ToString(),
                                    ProductDescription = dr.PRODUCT_DESCRIPTION?.ToString(),
                                    ProductNote = dr.PRODUCT_NOTE?.ToString(),
                                    ProductQuantity = string.IsNullOrEmpty(dr.PRODUCT_QUANTITY?.ToString()) ? 0 : Convert.ToInt32(dr.PRODUCT_QUANTITY),
                                    Weight = string.IsNullOrEmpty(dr.WEIGHT?.ToString()) ? 0 : Convert.ToInt32(dr.WEIGHT),
                                    TotalAmount = string.IsNullOrEmpty(dr.TOTAL_AMOUNT?.ToString()) ? 0 : Convert.ToInt64(dr.TOTAL_AMOUNT),
                                    ServiceFee = string.IsNullOrEmpty(dr.SERVICE_FEE?.ToString()) ? 0 : Convert.ToInt64(dr.SERVICE_FEE),
                                    TransportFee = string.IsNullOrEmpty(dr.TRANSPORT_FEE?.ToString()) ? 0 : Convert.ToInt64(dr.TRANSPORT_FEE),
                                    OrderStatus = dr.ORDER_STATUS?.ToString(),
                                    CollectionType = string.IsNullOrEmpty(dr.COLLECTION_TYPE?.ToString()) ? 0 : Convert.ToInt32(dr.COLLECTION_TYPE),
                                    Transport = dr.IS_TRANSPORT?.ToString(),
                                    CreatedBy = string.IsNullOrEmpty(dr.CREATED_BY?.ToString()) ? 0 : Convert.ToInt32(dr.CREATED_BY),

                                    EspeciallyServiceCode = dr.ESPECIALLY_SERVICE_CODE?.ToString(),
                                    EspeciallyServiceType = string.IsNullOrEmpty(dr.ESPECIALLY_SERVICE_TYPE?.ToString()) ? 0 : Convert.ToInt32(dr.ESPECIALLY_SERVICE_TYPE),
                                    Classification = new ZBaseResponse()
                                    {
                                        Code = dr.CLASSIFICATION?.ToString(),
                                        Name = XData.GetNameListTransportServiceClassification(dr.CLASSIFICATION?.ToString()),
                                    },

                                    AdditionService = dr.ADDITION_SERVICE?.ToString(),

                                    POCode = dr.PO_CODE?.ToString(),

                                    ShippingCode = dr.SHIPPING_CODE?.ToString(),
                                    Consignment = new ZBaseResponse()
                                    {
                                        Code = dr.IS_CONSIGNMENT?.ToString(),
                                        Name = XData.GetNameConsignment(dr.IS_CONSIGNMENT?.ToString()),
                                    },

                                    TotalCOD = Convert.ToInt64(dr.TOTAL_COD),
                                    TransportSizeHeight = Convert.ToInt32(dr.TRANSPORT_SIZE_HEIGHT),
                                    TransportSizeWidth = Convert.ToInt32(dr.TRANSPORT_SIZE_WIDTH),
                                    TransportSizeLength = Convert.ToInt32(dr.TRANSPORT_SIZE_LENGTH),
                                    Note = dr.NOTE?.ToString(),
                                    TransportPayer = Convert.ToInt32(dr.TRANSPORT_PAYER),
                                };
                            }
                        }
                    }
                    else
                    {
                        logger.LogWarning(
                            "SEND_TRANSPORT_INFO: P_RETURN_CODE={Code} nhưng P_OUT_CURSOR không đọc được (Value={ValueType}).",
                            result,
                            pCursor.Value?.GetType().FullName ?? "null");
                    }
                    if (item != null)
                    {
                        if (item.OrderStatus != "N" && item.OrderStatus != "L")
                        {
                            var notifyStatus = XData.GetNameOrderStatus(item.OrderStatus);
                            res.Code = "02";
                            res.Message = "Đơn hàng đang ở trạng thái " + notifyStatus + ".";
                        }
                        else if (item.Transport == "N")
                        {
                            res.Code = "03";
                            res.Message = "Đơn hàng không được chọn vận chuyển.";
                        }
                        else if (string.IsNullOrEmpty(item.ShippingCode))
                        {
                            res.Code = "04";
                            res.Message = "Mã bưu gửi bị trống.";
                        }
                        else
                        {
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(item);
                        }
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Thất bại";
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "OrderRepository.SendTransportInfo failed");
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        }
        res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        return res;
    }

    public ZResponse SendTransportInfoVNPOST(ZOrderSendTransportInfoRequest req, ILogger logger)
    {
        ZResponse res = new ZResponse();
        try
        {
            ZOrderSendTransportInfoVNPOSTResponse item = null;
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using var cmd = connection.CreateCommand();
                ApplyOracleCommandTimeout(cmd);
                cmd.CommandText = "PRC_GET_EMS_ORDER_DETAIL";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                cmd.Parameters.Add("P_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.OrderID;
                cmd.Parameters.Add("P_SHOP_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.ShopID;
                /* cmd.Parameters.Add("P_AMND_STATE", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                    string.IsNullOrEmpty(req.AmndState) ? "A" : req.AmndState; */

                var pReturn = new OracleParameter("P_RETURN_CODE", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(pReturn);

                var pCursor = new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                cmd.Parameters.Add(pCursor);

                cmd.ExecuteNonQuery();

                var result = GetOracleInt32Output(pReturn);
                if (result > 0)
                {
                    if (pCursor.Value is OracleRefCursor refCur)
                    {
                        using (var reader = refCur.GetDataReader())
                        {
                            var rows = ReadRowsAsDynamic(reader);
                            if (rows.Count > 0)
                            {
                                var dr = (IDictionary<string, object>)rows[0];
                                item = new ZOrderSendTransportInfoVNPOSTResponse()
                                {
                                    FlagConfig = GetRowString(dr, "FLAGCONFIG"),
                                    Transport = "Y",
                                    Id = int.Parse("0" + GetRowString(dr, "ID")),
                                    Code = GetRowString(dr, "CODE"),
                                    OrderCode = GetRowString(dr, "ORDER_CODE"),
                                    TrackingCode = GetRowString(dr, "TRACKING_CODE"),
                                    Ref_Code = GetRowString(dr, "REF_CODE"),
                                    StoreId = int.Parse("0" + GetRowString(dr, "STORE_ID", "0")),
                                    SenderCode = GetRowString(dr, "SENDER_CODE"),
                                    SenderName = GetRowString(dr, "SENDER_NAME"),
                                    SenderPhone = GetRowString(dr, "SENDER_PHONE"),
                                    SenderEmail = GetRowString(dr, "SENDER_EMAIL"),
                                    SenderProvinceId = int.Parse("0" + GetRowString(dr, "SENDER_PROVINCE_ID", "0")),
                                    SenderDistrictId = int.Parse("0" + GetRowString(dr, "SENDER_DISTRICT_ID", "0")),
                                    SenderWardId = int.Parse("0" + GetRowString(dr, "SENDER_WARD_ID", "0")),
                                    SenderHamletId = int.Parse("0" + GetRowString(dr, "SENDER_HAMLET_ID", "0")),
                                    SenderAddress = GetRowString(dr, "SENDER_ADDRESS"),
                                    ReceiverName = GetRowString(dr, "RECEIVER_NAME"),
                                    ReceiverEmail = GetRowString(dr, "RECEIVER_EMAIL"),
                                    ReceiverPhone = GetRowString(dr, "RECEIVER_PHONE"),
                                    ReceiverProvinceId = int.Parse("0" + GetRowString(dr, "RECEIVER_PROVINCE_ID", "0")),
                                    ReceiveDistrictId = int.Parse("0" + GetRowString(dr, "RECEIVER_DISTRICT_ID", "0")),
                                    ReceiveWardId = int.Parse("0" + GetRowString(dr, "RECEIVER_WARD_ID", "0")),
                                    ReceiveStreet = GetRowString(dr, "RECEIVER_STREET"),
                                    ReceiveAddress = GetRowString(dr, "RECEIVER_ADDRESS"),
                                    ProductQuantity = int.Parse("0" + GetRowString(dr, "PRODUCT_QUANTITY", "0")),
                                    ProductName = GetRowString(dr, "PRODUCT_NAME"),
                                    ProductDescription = GetRowString(dr, "PRODUCT_DESCRIPTION"),
                                    //StatusCode = GetRowString(dr, "STATUS_CODE"),
                                    StatusCode = "N",
                                    TotalAmount = long.Parse("0" + GetRowString(dr, "TOTAL_AMOUNT", "0")),
                                    Weight = long.Parse("0" + GetRowString(dr, "WEIGHT", "0")),
                                    ServiceType = int.Parse("0" + GetRowString(dr, "SERVICE_TYPE", "0")),
                                    StatusDescription = GetRowString(dr, "STATUS_DESCRIPTION"),
                                    CustomerCode = GetRowString(dr, "CUSTOMER_CODE"),
                                    CustomerName = GetRowString(dr, "GENERAL_FULL_NAME"),
                                    CustomerPhone = GetRowString(dr, "CUSTOMER_PHONE"),
                                    CustomerEmail = GetRowString(dr, "CUSTOMER_EMAIL"),
                                    // PRODUCT_CODE đẩy API lấy theo ORDER_CODE (không lấy cột PRODUCT_CODE / OrderID debug).
                                    ProductCode = GetRowString(dr, "ORDER_CODE"),
                                    ProductValue = long.Parse("0" + GetRowString(dr, "PRODUCT_VALUE", "0")),
                                    OrderNumber = GetRowString(dr, "ORDER_NUMBER"),
                                    AdditionService = GetRowString(dr, "ADDITION_SERVICE"),
                                    SpecialService = GetRowString(dr, "SPECIAL_SERVICE"),
                                    ServiceCode = FirstNonEmpty(
                                        GetRowString(dr, "SERVICE_CODE"),
                                        GetRowString(dr, "SERVICECODE")),
                                    Service_Fee = int.Parse("0" + GetRowString(dr, "SERVICE_FEE", "0")),
                                    Cod = int.Parse("0" + FirstNonEmpty(GetRowString(dr, "COD"), GetRowString(dr, "TOTAL_COD"), "0")),
                                    Po_Create = FirstNonEmpty(GetRowString(dr, "PO_CREATE"), GetRowString(dr, "PO_CODE")),
                                    Channel = GetRowString(dr, "CHANNEL"),
                                    ToCountryCode = FirstNonEmpty(GetRowString(dr, "TO_COUNTRY_CODE"), "VN"),
                                    ToZipCode = GetRowString(dr, "TO_ZIP_CODE"),
                                    Complete = int.Parse("0" + GetRowString(dr, "COMPLETE", "0")),
                                    IsConsorShip = GetRowString(dr, "IS_CONSIGNMENT", "0") == "1",
                                    CONTRACT_CODE_C = GetRowString(dr, "CONTRACT_CODE_C"),
                                    SubCustomerCode = GetRowString(dr, "SUB_CUSTOMER_CODE"),
                                    ValueAddedList = GetRowString(dr, "VALUE_ADDED_LIST"),
                                    ReceiverCode = FirstNonEmpty(
                                        GetRowString(dr, "RECEIVER_CODE"),
                                        GetRowString(dr, "SUB_CUSTOMER_CODE"))
                                };
                            }
                        }
                    }
                    else
                    {
                        logger.LogWarning(
                            "SEND_TRANSPORT_INFO: P_RETURN_CODE={Code} nhưng P_OUT_CURSOR không đọc được (Value={ValueType}).",
                            result,
                            pCursor.Value?.GetType().FullName ?? "null");
                    }
                    if (item != null)
                    {
                        if (item.StatusCode != "N" && item.StatusCode != "L")
                        {
                            var notifyStatus = XData.GetNameOrderStatus(item.StatusCode);
                            res.Code = "02";
                            res.Message = "Đơn hàng đang ở trạng thái " + notifyStatus + ".";
                        }
                        else if (item.Transport == "N")
                        {
                            res.Code = "03";
                            res.Message = "Đơn hàng không được chọn vận chuyển.";
                        }
                        else if (string.IsNullOrEmpty(item.TrackingCode))
                        {
                            res.Code = "04";
                            res.Message = "Mã bưu gửi bị trống.";
                        }
                        else
                        {
                            res.Code = "00";
                            res.Message = "Lấy dữ liệu thành công";
                            res.Data = JsonConvert.SerializeObject(item);
                        }
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu.";
                    }
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Thất bại";
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "OrderRepository.SendTransportInfoVNPOST failed");
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        }
        res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        return res;
    }

    public ZResponse SendTransportData(ZOrderSendTransportDataRequest req, ILogger logger)
    {
        ZResponse res = new ZResponse();
        try
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                var parameters = new OracleDynamicParameters();
                parameters.Add("P_RETURN_CODE", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("P_EMSONE_CODE", req.EMSOneCode);
                parameters.Add("P_ORDER_ID", req.OrderID);
                parameters.Add("P_SHOP_ID", req.ShopID);
                parameters.Add("P_UPDATED_BY", req.UpdatedBy);
                parameters.Add("P_OUT_ORDER_ID", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
                parameters.Add("P_OUT_PRODUCT_VARIANT_NAME", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);

                connection.Execute(
                    "EMS_ORDER_PKG.SEND_TRANSPORT_DATA",
                    parameters,
                    commandTimeout: _oracleCommandTimeoutSeconds,
                    commandType: CommandType.StoredProcedure);

                var result = parameters.Get<int>("P_RETURN_CODE");
                if (result > 0)
                {
                    res.Code = "00";
                    res.Message = "Thành công.";
                    res.Data = parameters.Get<int>("P_OUT_ORDER_ID").ToString();

                    var resOrderGetByID = GetByID(
                        new ZOrderGetByIDRequest() { ID = Convert.ToInt32(res.Data), ShopID = req.ShopID },
                        logger);

                    if (resOrderGetByID.Code == "00")
                    {
                        var itemById = JsonConvert.DeserializeObject<ZOrderGetByIDResponse>(resOrderGetByID.Data);
                        _ = Task.Run(async () =>
                        {
                            try
                            {

                                ZSendReceiverInforRequest reqSendReceiverInfor = new ZSendReceiverInforRequest()
                                {
                                    ItemCode = itemById.ShippingCode,
                                    OrderCode = itemById.EMSOneCode,
                                    CustomerCode = itemById.CrmOrPayPostCode,
                                    SenderName = itemById.ShipmentDetails.PickupFullName,
                                    SenderPhone = itemById.ShipmentDetails.PickupMobileNumber,
                                    SenderAddress = itemById.ShipmentDetails.PickupStreet,
                                    ReceiverName = itemById.ShipmentDetails.ReceiverName,
                                    ReceiverPhone = itemById.ShipmentDetails.MobileNumber,
                                    ReceiverAddress = itemById.ShipmentDetails.FullAddress,
                                    Channel = "EMSONE",
                                    Extend_1 = itemById.Message,

                                };
                                _logger.LogInformation($"SendDcsData: {JsonConvert.SerializeObject(reqSendReceiverInfor)}");
                                await _emsRepository.SendReceiverInforAsync(reqSendReceiverInfor, CancellationToken.None).ConfigureAwait(false);
                            }
                            catch (Exception innerEx)
                            {
                                logger.LogError(innerEx, "OrderRepository.SendTransportData: SendReceiverInfor background task failed");
                            }
                        });
                    }
                }
                else
                {
                    var ProductVariantName = parameters.Get<string>("P_OUT_PRODUCT_VARIANT_NAME");
                    switch (result)
                    {
                        case -1:
                            res.Code = "01";
                            res.Message = "Cửa hàng không tồn tại.";
                            break;
                        case -2:
                            res.Code = "02";
                            res.Message = "Đơn hàng không tồn tại.";
                            break;
                        case -3:
                            res.Code = "03";
                            res.Message = "Trạng thái không hợp lệ.";
                            break;
                        case -4:
                            res.Code = "04";
                            res.Message = "Sản phẩm không tồn tại.";
                            break;
                        case -5:
                            res.Code = "05";
                            res.Message = string.Format("Sản phẩm {0} không tồn tại trong kho.", ProductVariantName);
                            break;
                        case -6:
                            res.Code = "06";
                            res.Message = string.Format("Sản phẩm {0} trong kho không đủ.", ProductVariantName);
                            break;
                        case -7:
                            res.Code = "07";
                            res.Message = string.Format("Sản phẩm {0} trong kho đã hết.", ProductVariantName);
                            break;
                        case -8:
                            res.Code = "08";
                            res.Message = "Tạo mã bưu gửi thất bại.";
                            break;
                        default:
                            res.Code = "99";
                            res.Message = "Thất bại";
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "OrderRepository.SendTransportData failed");
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        }
        res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        return res;
    }

    public ZResponse SendTransportSuccess(ZOrderSendTransportSuccessRequest req, ILogger logger)
    {
        ZResponse res = new ZResponse();
        try
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using var cmd = connection.CreateCommand();
                ApplyOracleCommandTimeout(cmd);
                // FUNCTION SEND_TRANSPORT_SUCCESS(..., P_CREATED_BY) RETURN NUMBER
                cmd.CommandText = "EMS_ORDER_PKG.SEND_TRANSPORT_SUCCESS";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                var pReturn = new OracleParameter
                {
                    ParameterName = "RETURN_VALUE",
                    OracleDbType = OracleDbType.Int32,
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(pReturn);

                cmd.Parameters.Add("P_ORDER_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.OrderID;
                cmd.Parameters.Add("P_SHOP_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.ShopID;
                cmd.Parameters.Add("P_AMND_STATE", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                    (object?)req.AmndState ?? DBNull.Value;
                cmd.Parameters.Add("P_CREATED_BY", OracleDbType.Int32, ParameterDirection.Input).Value = req.UpdatedBy;

                cmd.ExecuteNonQuery();

                var result = GetOracleInt32Output(pReturn);
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
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "OrderRepository.SendTransportSuccess failed");
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        }
        res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        return res;
    }

    public ZResponse SendTransportError(ZOrderSendTransportErrorRequest req, ILogger logger)
    {
        ZResponse res = new ZResponse();
        try
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using var cmd = connection.CreateCommand();
                ApplyOracleCommandTimeout(cmd);
                // FUNCTION SEND_TRANSPORT_ERROR(P_ORDER_ID, P_SHOP_ID, P_AMND_STATE) RETURN NUMBER
                cmd.CommandText = "EMS_ORDER_PKG.SEND_TRANSPORT_ERROR";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                var pReturn = new OracleParameter
                {
                    ParameterName = "RETURN_VALUE",
                    OracleDbType = OracleDbType.Int32,
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(pReturn);

                cmd.Parameters.Add("P_ORDER_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.OrderID;
                cmd.Parameters.Add("P_SHOP_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.ShopID;
                cmd.Parameters.Add("P_AMND_STATE", OracleDbType.Varchar2, ParameterDirection.Input).Value =
                    (object?)req.AmndState ?? DBNull.Value;

                cmd.ExecuteNonQuery();

                var result = GetOracleInt32Output(pReturn);
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
                        default:
                            res.Code = "99";
                            res.Message = "Thất bại";
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "OrderRepository.SendTransportError failed");
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        }
        res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        return res;
    }

    public ZResponse GetByID(ZOrderGetByIDRequest req, ILogger logger)
    {
        ZResponse res = new ZResponse();
        try
        {
            ZOrderGetByIDResponse item = null;
            using (var connection = new OracleConnection(_connectionString))
            {
                connection.Open();
                using var cmd = connection.CreateCommand();
                ApplyOracleCommandTimeout(cmd);
                cmd.CommandText = "EMS_ORDER_PKG.GET_BY_ID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;

                cmd.Parameters.Add("P_ORDER_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.ID;
                cmd.Parameters.Add("P_SHOP_ID", OracleDbType.Int32, ParameterDirection.Input).Value = req.ShopID;

                var pReturn = new OracleParameter("P_RETURN_CODE", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(pReturn);

                var pCursor = new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                cmd.Parameters.Add(pCursor);

                cmd.ExecuteNonQuery();

                var result = GetOracleInt32Output(pReturn);
                if (result > 0)
                {
                    if (pCursor.Value is OracleRefCursor refCur)
                    {
                        using (var reader = refCur.GetDataReader())
                        {
                            var rows = ReadCursorRows(reader);
                            if (rows.Count > 0)
                            {
                                var dr = rows[0];
                                item = new ZOrderGetByIDResponse()
                                {
                                    ID = Convert.ToInt32(dr["ID"]),
                                    EMSOneCode = dr["EMSONE_CODE"].ToString(),
                                    OrderCode = dr["ORDER_CODE"].ToString(),
                                    BatchCode = dr["BATCH_CODE"].ToString(),
                                    ShippingCode = dr["SHIPPING_CODE"].ToString(),
                                    Status = dr["STATUS"].ToString(),
                                    StatusName = XData.GetNameOrderStatus(dr["STATUS"].ToString()),
                                    WareHouseID = string.IsNullOrEmpty(dr["WAREHOUSE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["WAREHOUSE_ID"].ToString()),
                                    WareHouseName = dr["WAREHOUSE_NAME"].ToString(),

                                    TotalAmount = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_AMOUNT"].ToString()) ? dr["TOTAL_AMOUNT"].ToString() : "0"),
                                    TotalDiscount = Convert.ToInt32(!string.IsNullOrEmpty(dr["TOTAL_DISCOUNT"].ToString()) ? dr["TOTAL_DISCOUNT"].ToString() : "0"),
                                    TransportFee = string.IsNullOrEmpty(dr["PROVISIONAL_TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt32(dr["PROVISIONAL_TRANSPORT_FEE"].ToString()),
                                    ActualTransportFee = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_FEE"].ToString()) ? 0 : Convert.ToInt32(dr["ACTUAL_TRANSPORT_FEE"].ToString()),
                                    TotalProductValue = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_PRODUCT_VALUE"].ToString()) ? dr["TOTAL_PRODUCT_VALUE"].ToString() : "0"),
                                    TotalOrderValue = Convert.ToInt64(!string.IsNullOrEmpty(dr["TOTAL_ORDER_VALUE"].ToString()) ? dr["TOTAL_ORDER_VALUE"].ToString() : "0"),
                                    TotalCOD = string.IsNullOrEmpty(dr["TOTAL_COD"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_COD"].ToString()),
                                    ActualTotalCOD = string.IsNullOrEmpty(dr["ACTUAL_TOTAL_COD"].ToString()) ? 0 : Convert.ToInt64(dr["ACTUAL_TOTAL_COD"].ToString()),
                                    TotalPHU = string.IsNullOrEmpty(dr["TOTAL_PHU"].ToString()) ? 0 : Convert.ToInt64(dr["TOTAL_PHU"].ToString()),
                                    Transport = dr["TRANSPORT"].ToString(),
                                    Channel = dr["CHANNEL"].ToString(),
                                    Note = dr["NOTE"].ToString(),
                                    Message = dr["MESSAGE"].ToString(),
                                    ShipmentDetails = new ZOrderAddShipmentInfoRequest()
                                    {
                                        ReceiverName = dr["RECEIVER_NAME"].ToString(),
                                        MobileNumber = dr["RECEIVER_MOBILE_NUMBER"].ToString(),
                                        ProvinceID = Convert.ToInt32(!string.IsNullOrEmpty(dr["PROVINCE_ID"].ToString()) ? dr["PROVINCE_ID"].ToString() : "0"),
                                        ProvinceName = dr["PROVINCE_NAME"].ToString(),
                                        DistrictID = Convert.ToInt32(!string.IsNullOrEmpty(dr["DISTRICT_ID"].ToString()) ? dr["DISTRICT_ID"].ToString() : "0"),
                                        DistrictName = dr["DISTRICT_NAME"].ToString(),
                                        WardID = Convert.ToInt32(!string.IsNullOrEmpty(dr["WARD_ID"].ToString()) ? dr["WARD_ID"].ToString() : "0"),
                                        WardName = dr["WARD_NAME"].ToString(),
                                        Address = dr["ADDRESS"].ToString(),
                                        FullAddress = dr["FULL_ADDRESS"].ToString(),
                                        PickUpAddressID = string.IsNullOrEmpty(dr["PICK_UP_ADDRESS_ID"].ToString()) ? 0 : Convert.ToInt32(dr["PICK_UP_ADDRESS_ID"].ToString()),
                                        PickupFullName = dr["SENDER_NAME"].ToString(),
                                        PickupMobileNumber = dr["SENDER_PHONE"].ToString(),
                                        PickupProvinceID = string.IsNullOrEmpty(dr["SENDER_PROVINCE_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_PROVINCE_ID"].ToString()),
                                        PickupProvinceName = dr["SENDER_PROVINCE_NAME"].ToString(),
                                        PickupDistrictID = string.IsNullOrEmpty(dr["SENDER_DISTRICT_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_DISTRICT_ID"].ToString()),
                                        PickupDistrictName = dr["SENDER_DISTRICT_NAME"].ToString(),
                                        PickupWardID = string.IsNullOrEmpty(dr["SENDER_WARD_ID"].ToString()) ? 0 : Convert.ToInt32(dr["SENDER_WARD_ID"].ToString()),
                                        PickupWardName = dr["SENDER_WARD_NAME"].ToString(),
                                        PickupStreet = dr["SENDER_STREET"].ToString(),
                                        PickupFullAddress = dr["SENDER_FULL_ADDRESS"].ToString(),
                                    },
                                    ReasonCancel = dr["REASON_CANCEL"].ToString(),
                                    UpdatedDate = dr["UPDATED_DATE"].ToString(),
                                    UpdatedName = dr["UPDATED_NAME"].ToString(),
                                    TransportOrderName = dr["TRANSPORT_ORDER_NAME"].ToString(),
                                    TransportWeight = string.IsNullOrEmpty(dr["PROVISIONAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["PROVISIONAL_TRANSPORT_WEIGHT"].ToString()),
                                    ActualTransportWeight = string.IsNullOrEmpty(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["ACTUAL_TRANSPORT_WEIGHT"].ToString()),
                                    ExchangeTransportWeight = string.IsNullOrEmpty(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["EXCHANGE_TRANSPORT_WEIGHT"].ToString()),
                                    TransportSizeLength = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_LENGTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_LENGTH"].ToString()),
                                    TransportSizeWidth = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_WIDTH"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_WIDTH"].ToString()),
                                    TransportSizeHeight = string.IsNullOrEmpty(dr["TRANSPORT_SIZE_HEIGHT"].ToString()) ? 0 : Convert.ToDouble(dr["TRANSPORT_SIZE_HEIGHT"].ToString()),
                                    TransportQuantity = string.IsNullOrEmpty(dr["TRANSPORT_QUANTITY"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_QUANTITY"].ToString()),
                                    TransportOrderValue = string.IsNullOrEmpty(dr["TRANSPORT_ORDER_VALUE"].ToString()) ? 0 : Convert.ToInt64(dr["TRANSPORT_ORDER_VALUE"].ToString()),
                                    TransportMainService = string.IsNullOrEmpty(dr["TRANSPORT_MAIN_SERVICE"].ToString()) ? 0 : Convert.ToInt32(dr["TRANSPORT_MAIN_SERVICE"].ToString()),
                                    TransportMainServiceName = dr["TRANSPORT_MAIN_SERVICE_NAME"].ToString(),
                                    TransportExtraService = dr["TRANSPORT_EXTRA_SERVICE"].ToString(),
                                    TransportExtraServiceName = dr["TRANSPORT_EXTRA_SERVICE_NAMES"].ToString(),
                                    TransportPayer = string.IsNullOrEmpty(dr["TRANSPORT_PAYER"].ToString()) ? 0 : Convert.ToInt32(dr["TRANSPORT_PAYER"].ToString()),
                                    TransportPayerName = string.IsNullOrEmpty(dr["TRANSPORT_PAYER"].ToString()) ? "" : XData.GetNameTransportPayer(dr["TRANSPORT_PAYER"].ToString()),
                                    CollectionType = new ZBaseResponse()
                                    {
                                        ID = Convert.ToInt32(dr["COLLECTION_TYPE"]),
                                        Code = dr["COLLECTION_TYPE"].ToString(),
                                        Name = XData.GetNameCollectionType(dr["COLLECTION_TYPE"].ToString())
                                    },
                                    TypeShop = string.IsNullOrEmpty(dr["TYPE_SHOP"].ToString()) ? 0 : Convert.ToInt32(dr["TYPE_SHOP"].ToString()),
                                    AcceptancePosCode = dr["ACCEPTANCE_POSCODE"].ToString(),
                                    DeviceName = dr["DEVICE_NAME"].ToString(),
                                    CrmOrPayPostCode = dr["CUSTOMER_CODE"].ToString(),
                                };
                            }
                        }
                    }
                    else
                    {
                        logger.LogWarning(
                            "GET_BY_ID: P_RETURN_CODE={Code} nhưng P_OUT_CURSOR không đọc được (Value={ValueType}).",
                            result,
                            pCursor.Value?.GetType().FullName ?? "null");
                    }
                    if (item != null)
                    {
                        res.Code = "00";
                        res.Message = "Lấy dữ liệu thành công";
                        res.Data = JsonConvert.SerializeObject(item);
                    }
                    else
                    {
                        res.Code = "01";
                        res.Message = "Không tồn tại dữ liệu";
                    }
                }
                else
                {
                    res.Code = "99";
                    res.Message = "Thất bại";
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "OrderRepository.GetByID failed");
            res.Code = "99";
            res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        }
        res.Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        return res;
    }

    private static int GetOracleInt32Output(OracleParameter p)
    {
        var v = p.Value;
        if (v is null or DBNull)
            return 0;
        // ODP.NET thường trả OracleDecimal cho OUT NUMBER / Int32
        if (v is OracleDecimal od)
            return od.IsNull ? 0 : od.ToInt32();
        if (v is int i)
            return i;
        if (v is long l)
            return checked((int)l);
        if (v is decimal d)
            return (int)d;
        return Convert.ToInt32(Convert.ToDecimal(v, System.Globalization.CultureInfo.InvariantCulture));
    }

    private static List<Dictionary<string, object?>> ReadCursorRows(DbDataReader reader)
    {
        var rows = new List<Dictionary<string, object?>>();
        while (reader.Read())
        {
            var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < reader.FieldCount; i++)
                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
            rows.Add(row);
        }
        return rows;
    }

    private static List<ExpandoObject> ReadRowsAsDynamic(DbDataReader reader)
    {
        var list = new List<ExpandoObject>();
        while (reader.Read())
        {
            IDictionary<string, object> row = new ExpandoObject();
            for (var i = 0; i < reader.FieldCount; i++)
                row[reader.GetName(i)] = reader.IsDBNull(i) ? null! : reader.GetValue(i);
            list.Add((ExpandoObject)row);
        }
        return list;
    }

    private static string GetRowString(IDictionary<string, object> row, string key, string defaultValue = "")
    {
        if (row.TryGetValue(key, out var value) && value != null)
            return value.ToString() ?? defaultValue;

        foreach (var kv in row)
        {
            if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase) && kv.Value != null)
                return kv.Value.ToString() ?? defaultValue;
        }

        return defaultValue;
    }

    private static string FirstNonEmpty(params string[] values)
    {
        foreach (var v in values)
        {
            if (!string.IsNullOrEmpty(v))
                return v;
        }

        return "";
    }
}