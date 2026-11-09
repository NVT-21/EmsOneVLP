using System.Runtime.Serialization;

namespace EMS.TransportWorkerService;

[Serializable]
[DataContract]
public class ZRequest
{
    [DataMember]
    public string Code { get; set; }
    [DataMember]
    public string Data { get; set; }
    [DataMember]
    public string Time { get; set; }
}
