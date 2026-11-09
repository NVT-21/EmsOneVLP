using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.TransportWorkerService.Models;

public class ZResponse
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Data { get; set; }
    //public int TotalData { get; set; }
    public string Time { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    public string Signature { get; set; }
}