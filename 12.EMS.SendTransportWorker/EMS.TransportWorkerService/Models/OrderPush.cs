using Newtonsoft.Json;

namespace EMS.TransportWorkerService.Models;

/// <summary>
/// Payload đẩy đơn sang PNS Vnpost: Partner/EMS/CreateOrder.
/// Chỉ các cột trong request example — không thêm field khác.
/// </summary>
public class OrderPush
{
    [JsonProperty("ItemCode")]
    public string ItemCode { get; set; }
    [JsonProperty("SenderProvince")]
    public string SenderProvince { get; set; }
    [JsonProperty("SenderDistrict")]
    public string SenderDistrict { get; set; }
    [JsonProperty("SenderWard")]
    public string SenderWard { get; set; }
    [JsonProperty("SenderAddress")]
    public string SenderAddress { get; set; }
    [JsonProperty("SenderName")]
    public string SenderName { get; set; }
    [JsonProperty("SenderEmail")]
    public string SenderEmail { get; set; }
    [JsonProperty("SenderTel")]
    public string SenderTel { get; set; }
    [JsonProperty("SenderDesc")]
    public string SenderDesc { get; set; }
    [JsonProperty("Description")]
    public string Description { get; set; }
    [JsonProperty("ExtendData")]
    public string ExtendData { get; set; }
    [JsonProperty("ReceiverName")]
    public string ReceiverName { get; set; }
    [JsonProperty("ReceiverProvince")]
    public string ReceiverProvince { get; set; }
    [JsonProperty("ReceiverDistrict")]
    public string ReceiverDistrict { get; set; }
    [JsonProperty("ReceiverWard")]
    public int ReceiverWard { get; set; }
    [JsonProperty("ReceiverAddress")]
    public string ReceiverAddress { get; set; }
    [JsonProperty("ReceiverEmail")]
    public string ReceiverEmail { get; set; }
    [JsonProperty("ReceiverTel")]
    public string ReceiverTel { get; set; }
    [JsonProperty("CustomerCode")]
    public string CustomerCode { get; set; }
    [JsonProperty("PosIdCollect")]
    public string PosIdCollect { get; set; }
    [JsonProperty("FlagConfig")]
    public string FlagConfig { get; set; }
    [JsonProperty("PartnerId")]
    public string PartnerId { get; set; }
    [JsonProperty("PartnerSourceId")]
    public string PartnerSourceId { get; set; }
    [JsonProperty("Weight")]
    public string Weight { get; set; }
    [JsonProperty("OrderNumber")]
    public string OrderNumber { get; set; }
    [JsonProperty("CODAmount")]
    public string CODAmount { get; set; }
    [JsonProperty("AdditionService")]
    public string AdditionService { get; set; }
    [JsonProperty("SpecialService")]
    public string SpecialService { get; set; }
    [JsonProperty("ServiceFee")]
    public long? ServiceFee { get; set; }
    [JsonProperty("PickupDate")]
    public int PickupDate { get; set; }
    [JsonProperty("PickupHours")]
    public int PickupHours { get; set; }
    [JsonProperty("ValueAddedList")]
    public string ValueAddedList { get; set; }
    [JsonProperty("ServiceCode")]
    public string ServiceCode { get; set; }
    [JsonProperty("ReceiverCode")]
    public string ReceiverCode { get; set; }
    [JsonProperty("RecContractNumber")]
    public string RecContractNumber { get; set; }

    /// <summary>
    /// Body JSON chỉ gồm đúng các cột request example.
    /// </summary>
    public object ToApiBody() => new
    {
        ItemCode,
        SenderProvince,
        SenderDistrict,
        SenderWard,
        SenderAddress,
        SenderName,
        SenderEmail,
        SenderTel,
        SenderDesc,
        Description,
        ExtendData,
        ReceiverName,
        ReceiverProvince,
        ReceiverDistrict,
        ReceiverWard,
        ReceiverAddress,
        ReceiverEmail,
        ReceiverTel,
        CustomerCode,
        PosIdCollect,
        FlagConfig,
        PartnerId,
        PartnerSourceId,
        Weight,
        OrderNumber,
        CODAmount,
        AdditionService,
        SpecialService,
        ServiceFee,
        PickupDate,
        PickupHours,
        ValueAddedList,
        ServiceCode,
        ReceiverCode,
        RecContractNumber
    };
}
