using System.Runtime.Serialization;

namespace EMS.TransportWorkerService;

[Serializable]
[DataContract]
public class ZBaseResponse
{
    [DataMember]
    public int ID { get; set; }
    [DataMember]
    public int Title { get; set; }
    [DataMember]
    public string Code { get; set; }
    [DataMember]
    public string Name { get; set; }
}
