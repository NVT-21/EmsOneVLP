using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZStockTakeGetByIDRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZStockTakeGetByIDResponse
    {
        [DataMember]
        public int StockTakeID { get; set; }
        [DataMember]
        public string StockTakeCode { get; set; }
        [DataMember]
        public ZWareHouseResponse WareHouse { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedName { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public List<ZStockTakeDetailResponse> ListStockTakeDetail { get; set; }
    }
}
