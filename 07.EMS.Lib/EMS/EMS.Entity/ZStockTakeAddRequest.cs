using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZStockTakeAddRequest
    {
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int StockTakeID { get; set; }
        [DataMember]
        public List<ZStockTakeDetailAddRequest> ListDetailStockTake { get; set; }
    }
}
