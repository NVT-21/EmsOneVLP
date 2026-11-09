namespace EMS.TransportWorkerService;

public class ZTransportInfoShippingApiRequest
{
    public string PICKUP_CODE_API { get; set; }
    public string PO_CODE { get; set; }
    public string DESCRIPTION { get; set; }
    public string CUSTOMER_CODE { get; set; }
    public string CUSTOMER_PHONE { get; set; }
    public string CUSTOMER_NAME { get; set; }
    public string CUSTOMER_EMAIL { get; set; }
    public string STOREID { get; set; }
    public int PICKUP_PROVINCE_ID { get; set; }
    public int PICKUP_DISTRICT_ID { get; set; }
    public int PICKUP_WARD_ID { get; set; }
    public string PICKUP_STREET { get; set; }
    public string PICKUP_DATE { get; set; }
    public string PICKUP_HOURS { get; set; }
    public string CONTACT_NAME { get; set; }
    public string CONTACT_PHONE { get; set; }
    public string PICKUP_POSTMAN { get; set; }
    public string PICKUP_POCODE { get; set; }
    public string PICKUP_CONFIG { get; set; }
    public string SignatureCode { get; set; }
    public List<ZListItemInfoShippingRequest> LIST_ITEM { get; set; }
}
