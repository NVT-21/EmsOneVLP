using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZGrabDeliveryOrderRequest
    {
        public string merchant_token { get; set; }
        public string ordercode { get; set; }
        public string serviceType { get; set; }
        public string customer_code { get; set; }
        public string promo_code { get; set; }
        public List<ZGrabDeliveryOrderOrigin> origin { get; set; }
        public List<ZGrabDeliveryOrderDestination> destination { get; set; }
    }

    public class ZGrabDeliveryOrderOrigin
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string address { get; set; }
        public string description { get; set; }
    }

    public class ZGrabDeliveryOrderDestination
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public int total_amount { get; set; }
        public int weight { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int length { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string address { get; set; }
        public int money_collect { get; set; }
        public int extra { get; set; }
    }

    public class ZGrabDeliveryOrderResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public ZGrabDeliveryOrderData data { get; set; }
    }

    public class ZGrabDeliveryOrderData
    {
        public string itemcode { get; set; }
        public string id { get; set; }
        public string ordercode { get; set; }
        public int total_freight { get; set; }
        public int distance { get; set; }
        public List<ZGrabDeliveryOrderDataEstimatedTimeLine> estimatedTimeLine { get; set; }
    }

    public class ZGrabDeliveryOrderDataEstimatedTimeLine
    {
        public string pickup { get; set; }
        public string dropoff { get; set; }
        public string completed { get; set; }
    }
}
