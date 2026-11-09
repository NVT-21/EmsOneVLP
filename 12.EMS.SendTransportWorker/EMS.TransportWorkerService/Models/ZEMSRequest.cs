using System.Runtime.Serialization;

namespace EMS.TransportWorkerService;

[Serializable]
[DataContract]
public class ZEMSRequest
{
    [DataMember]
    public string Method { get; set; }
    [DataMember]
    public string RequestURL { get; set; }
    [DataMember]
    public string RequestURI { get; set; }
    [DataMember]
    public string APIKeyName { get; set; }
    [DataMember]
    public string APIKeyValue { get; set; }
    [DataMember]
    public string AuthorizationName { get; set; }
    [DataMember]
    public string AuthorizationValue { get; set; }
    [DataMember]
    public object Value { get; set; }
    [DataMember]
    public List<object> ListValue { get; set; }
}

[Serializable]
[DataContract]
public class ZEMSResponse
{
    [DataMember]
    public string Code { get; set; }
    [DataMember]
    public string Message { get; set; }
    [DataMember]
    public string Value { get; set; }
    [DataMember]
    public List<object> ListValue { get; set; }
}
