using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTikConfigProductRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string PageID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZTikConfigProductResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string IsSyncProduct { get; set; }
        [DataMember]
        public string IsSyncProductPrice { get; set; }
        [DataMember]
        public string IsSyncProductStock { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTikConfigProductChangeRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
