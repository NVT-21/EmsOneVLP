namespace EMS.TransportWorkerService;

public class ZListItemInfoShippingRequest
{
    public string TRACKING_CODE { get; set; }
    public string SENDER_NAME { get; set; }
    public string SENDER_CODE { get; set; }
    public string SENDER_PHONE { get; set; }
    public string SENDER_STREET { get; set; }
    public string SENDER_ADDRESS { get; set; }
    public string SENDER_PROVINCE_ID { get; set; }
    public string SENDER_DISTRICT_ID { get; set; }
    public string SENDER_WARD_ID { get; set; }
    public string SENDER_EMAIL { get; set; }
    public string RECEIVER_NAME { get; set; }
    public string RECEIVER_PROVINCE_ID { get; set; }
    public string RECEIVER_PHONE { get; set; }
    public string RECEIVER_DISTRICT_ID { get; set; }
    public string RECEIVER_WARD_ID { get; set; }
    public string RECEIVER_EMAIL { get; set; }
    public string RECEIVER_STREET { get; set; }
    public string RECEIVER_ADDRESS { get; set; }
    public string PRODUCT_NAME { get; set; }
    public int PRODUCT_VALUE { get; set; }
    public string PRODUCT_CODE { get; set; }
    public string PRODUCT_DESCRIPTION { get; set; }
    public int PRODUCT_QUANTITY { get; set; }
    public int WEIGHT { get; set; }
    public string ADDITION_SERVICE { get; set; }
    public string SPECIAL_SERVICE { get; set; }
    public long TOTAL_AMOUNT { get; set; }
    public string CUSTOMER_CODE { get; set; }
    public string TO_COUNTRY { get; set; }
    public string PO_CREATE { get; set; }
    public int COMPLETE { get; set; }
    public string POSTAL_PROVINCE_CODE { get; set; }
    public string POSTAL_DISTRICT_CODE { get; set; }
    public string POSTAL_COMMUNE_CODE { get; set; }
    public string POSTAL_CODE { get; set; }
    public int SERVICE_TYPE { get; set; }
    public long SERVICE_FEE { get; set; }
    public string CLASSIFY { get; set; }
    public int IS_CONSIGNMENT { get; set; }
    public string CHANNEL { get; set; }
}
