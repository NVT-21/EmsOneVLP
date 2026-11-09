using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCustomerImportExcelRequest
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string BirthDay { get; set; }
        [DataMember]
        public string TaxCode { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string CustomerStickerID { get; set; }
        [DataMember]
        public string IsUpdate { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}
