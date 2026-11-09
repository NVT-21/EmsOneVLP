using System.Runtime.Serialization;

namespace EMS.TransportWorkerService;

[Serializable]
[DataContract]
public class ZOrderAddShipmentInfoRequest
{
    [DataMember]
    public string ReceiverName { get; set; }
    [DataMember]
    public string MobileNumber { get; set; }
    [DataMember]
    public string Channel { get; set; }
    [DataMember]
    public string Email { get; set; }
    [DataMember]
    public int ProvinceID { get; set; }
    [DataMember]
    public string ProvinceName { get; set; }
    [DataMember]
    public int DistrictID { get; set; }
    [DataMember]
    public string DistrictName { get; set; }
    [DataMember]
    public int WardID { get; set; }
    [DataMember]
    public string WardName { get; set; }
    [DataMember]
    public string Address { get; set; }
    [DataMember]
    public string FullAddress { get; set; }
    [DataMember]
    public int PickUpAddressID { get; set; }

    [DataMember]
    public string IsReceiverNewAddress { get; set; }
    [DataMember]
    public string PickupFullName { get; set; }
    [DataMember]
    public string PickupMobileNumber { get; set; }
    [DataMember]
    public int PickupProvinceID { get; set; }
    [DataMember]
    public string PickupProvinceName { get; set; }
    [DataMember]
    public int PickupDistrictID { get; set; }
    [DataMember]
    public string PickupDistrictName { get; set; }
    [DataMember]
    public int PickupWardID { get; set; }
    [DataMember]
    public string PickupWardName { get; set; }
    [DataMember]
    public string PickupStreet { get; set; }
    [DataMember]
    public string PickupFullAddress { get; set; }
}
