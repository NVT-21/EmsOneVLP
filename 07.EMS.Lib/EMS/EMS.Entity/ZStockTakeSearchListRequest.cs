using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZStockTakeSearchListRequest
    {
        [DataMember]
        public string DateType { get; set; }
        [DataMember]
        public string StockTakeCode { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZStockTakeSearchListResponse
    {
        [DataMember]
        public int StockTakeID { get; set; }
        [DataMember]
        public string StockTakeCode { get; set; }
        [DataMember]
        public string WareHouseName { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public int TotalProduct { get; set; }
        [DataMember]
        public long TotalInventory { get; set; }
        [DataMember]
        public long TotalActual { get; set; }
        [DataMember]
        public long TotalDifferent { get; set; }
        [DataMember]
        public long TotalMoneyDifferent { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedName { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
    }
}
