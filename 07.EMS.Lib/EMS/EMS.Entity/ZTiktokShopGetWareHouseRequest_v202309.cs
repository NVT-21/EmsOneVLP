using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopGetWareHouseRequest_v202309
    {
    }

    public class ZTiktokShopGetWareHouseResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopGetWareHouseDataResponse_v202309 Data { get; set; }
    }
    public class ZTiktokShopGetWareHouseDataResponse_v202309
    {
        [JsonProperty("warehouses")]
        public List<ZTiktokShopGetWareHouseDataWareHousesResponse_v202309> WareHouses { get; set; }
    }
    public class ZTiktokShopGetWareHouseDataWareHousesResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("effect_status")]
        public string EffectStatus { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("sub_type")]
        public string SubType { get; set; }
        [JsonProperty("is_default")]
        public Boolean IsDefault { get; set; }
        [JsonProperty("address")]
        public ZTiktokShopGetWareHouseDataWareHousesAddressResponse_v202309 Address { get; set; }
    }
    public class ZTiktokShopGetWareHouseDataWareHousesAddressResponse_v202309
    {
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("district")]
        public string District { get; set; }
        [JsonProperty("town")]
        public string Town { get; set; }
        [JsonProperty("contact_person")]
        public string ContactPerson { get; set; }
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
        [JsonProperty("full_address")]
        public string FullAddress { get; set; }
        [JsonProperty("region_code")]
        public string RegionCode { get; set; }
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
