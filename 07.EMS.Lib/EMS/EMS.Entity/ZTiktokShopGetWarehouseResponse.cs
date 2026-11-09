using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopGetWareHouseResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopGetWareHouseDataResponse Data { get; set; }
    }
    public class ZTiktokShopGetWareHouseDataResponse
    {
        [JsonProperty("warehouse_list")]
        public List<ZTiktokShopGetWareHouseDataWarehouseListResponse> WareHouseList { get; set; }
    }
    public class ZTiktokShopGetWareHouseDataWarehouseListResponse
    {
        [JsonProperty("is_default")]
        public Boolean IsDefault { get; set; }
        [JsonProperty("warehouse_address")]
        public ZTiktokShopGetWareHouseDataWareHouseListWareHouseAddressResponse WareHouseAddress { get; set; }
        [JsonProperty("warehouse_effect_status")]
        public int WareHouseEffectStatus { get; set; }
        [JsonProperty("warehouse_id")]
        public string WareHouseID { get; set; }
        [JsonProperty("warehouse_name")]
        public string WareHouseName { get; set; }
        [JsonProperty("warehouse_status")]
        public int WareHouseStatus { get; set; }
        [JsonProperty("warehouse_sub_type")]
        public int WareHouseSubType { get; set; }
        [JsonProperty("warehouse_type")]
        public int WareHouseType { get; set; }
    }
    public class ZTiktokShopGetWareHouseDataWareHouseListWareHouseAddressResponse
    {
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("contact_person")]
        public string ContactPerson { get; set; }
        [JsonProperty("district")]
        public string District { get; set; }
        [JsonProperty("full_address")]
        public string FullAddress { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("region_code")]
        public string RegionCode { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("town")]
        public string Town { get; set; }
        [JsonProperty("zipcode")]
        public string ZipCode { get; set; }
    }
}
