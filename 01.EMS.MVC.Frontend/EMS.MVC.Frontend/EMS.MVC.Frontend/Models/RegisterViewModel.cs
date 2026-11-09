using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class RegisterViewModel
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ProductBusiness { get; set; }
        [DataMember]
        public string TypeBusiness { get; set; }
        [DataMember]
        public int TypeShop { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public List<ProvinceModel> ListProvince { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public List<DistrictModel> ListDistrict { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public List<WardModel> ListWard { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public bool Condition { get; set; }
        [DataMember]
        public string CRMOrPaypostCode { get; set; }
        [DataMember]
        public int CollectionPostOfficeID { get; set; }
        [DataMember]
        public List<CollectionPostOfficeModel> ListCollectionPostOffice { get; set; }
    }
}