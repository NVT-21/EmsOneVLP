using System.Runtime.Serialization;

namespace EMS.TransportWorkerService;

[Serializable]
[DataContract]
public class ZOrderSendTransportDataRequest
{
    [DataMember]
    public string EMSOneCode { get; set; }
    [DataMember]
    public int OrderID { get; set; }
    [DataMember]
    public int ShopID { get; set; }
    [DataMember]
    public int UpdatedBy { get; set; }
}
