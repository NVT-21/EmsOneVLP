using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZCMSAddRequestSupportRequest
    {
        public List<ZCMSAddRequestSupportItemRequest> items { get; set; }
    }
    public class ZCMSAddRequestSupportResponse
    {
        public string Object { get; set; }
        public List<ZCMSAddRequestSupportItemRequest> rows { get; set; }
        public XCallApiRequestSupportError item { get; set; }
    }
    public class ZCMSAddRequestSupportItemRequest
    {
        public string Id { get; set; }
        public string AcountID { get; set; }
        public string Employee__c { get; set; }
        public string Nhan_vien_CSKH__c { get; set; }
        public string Tai_Lieu_Dinh_Kem__c { get; set; }
        public string CaseNumber { get; set; }
        public string Description { get; set; }
        public string So_phieu_khieu_nai__c { get; set; }
        public string Li_do_khieu_nai__c { get; set; }
        public string Priority { get; set; }
        public string Origin { get; set; }
        public string Nhom_dich_vu__c { get; set; }
        public string Location_Type__c { get; set; }
        public string Service__c { get; set; }
        public string Status { get; set; }
        public string So_Ho_So__c { get; set; }
        public string POS__c { get; set; }
        public string Ng_y_ti_p_nh_n__c { get; set; }
        public string Sales_Order__c { get; set; }
        public string SuppliedName { get; set; }
        public string SuppliedPhone { get; set; }
        public string SuppliedEmail { get; set; }
        public string Contact_Address__c { get; set; }
        public string Contact_province__c { get; set; }
        public string Contact_district__c { get; set; }
        public string Contact_commune__c { get; set; }
        public string CreatedDate { get; set; }
        public string LastModifiedDate { get; set; }
        public string Files { get; set; }
        public string Attachments { get; set; }
        public string error { get; set; }
    }
    public class XCallApiRequestSupportError
    {
        public string errorCode { get; set; }
        public XCallApiRequestSupportDetailError errorMessage { get; set; }
    }
    public class XCallApiRequestSupportDetailError
    {
        public string timestamp { get; set; }
        public string status { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public string path { get; set; }
    }
}
