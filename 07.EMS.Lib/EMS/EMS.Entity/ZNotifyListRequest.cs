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
    public class ZNotifyListRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int RowLimit { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string NotifyType { get; set; }
        [DataMember]
        public string IsCheck { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZNotifyListResponse
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string NotifyType { get; set; }
        [DataMember]
        public string NotifyTypeName { get; set; }
        [DataMember]
        public string NotifyName { get; set; }
        [DataMember]
        public string NotifyContent { get; set; }
        [DataMember]
        public string NotifyRedirectUrl { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public string IsCheck { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZNotifyUpdateRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string StrID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZNotifyGetByIDRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int ID { get; set; }
    }
}
