using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZStockTakeEditRequest
    {
        [DataMember]
        public int StockTakeID { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public List<ZStockTakeDetailEditRequest> ListDetailStockTake { get; set; }
    }
}
