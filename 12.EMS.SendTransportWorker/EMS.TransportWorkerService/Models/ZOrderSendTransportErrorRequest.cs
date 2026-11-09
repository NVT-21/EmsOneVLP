using System.Runtime.Serialization;

namespace EMS.TransportWorkerService;

[Serializable]
[DataContract]
public class ZOrderSendTransportErrorRequest
{
    [DataMember]
    public int OrderID { get; set; }
    [DataMember]
    public int ShopID { get; set; }
    [DataMember]
    public string AmndState { get; set; }
}
