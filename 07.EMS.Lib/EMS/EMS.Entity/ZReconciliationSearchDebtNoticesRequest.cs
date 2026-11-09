using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZReconciliationSearchDebtNoticesV1Request
    {
        public string CustomerCode { get; set; }
        public int MonthYear { get; set; }
    }
    public class ZReconciliationSearchDebtNoticesRequest
    {
        public string ma_kh { get; set; }
        public int thang_no { get; set; }
    }
    public class ZReconciliationSearchDebtNoticesResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public ZReconciliationSearchDebtNoticesData Data { get; set; }
    }
    public class ZReconciliationSearchDebtNoticesData
    {
        [JsonProperty("ma_kh")]
        public string CustomerCode { get; set; }

        [JsonProperty("thang_no")]
        public int MonthYear { get; set; }

        [JsonProperty("thong_tin_chung")]
        public List<ZReconciliationSearchDebtNoticesDataInfo> DataInfo { get; set; }

        [JsonProperty("thong_tin_chi_tiet")]
        public List<ZReconciliationSearchDebtNoticesDataInfoDetails> DataInfoDetails { get; set; }
    }
    public class ZReconciliationSearchDebtNoticesDataInfo
    {
        [JsonProperty("Ma_KH")]
        public string CustomerCode { get; set; }
        [JsonProperty("Ten_Khach_Hang")]
        public string CustomerName { get; set; }
        [JsonProperty("Dia_Chi")]
        public string Address { get; set; }
        [JsonProperty("HTTT")]
        public string Payments { get; set; }
        [JsonProperty("MS_Thue")]
        public string TaxCode { get; set; }
        [JsonProperty("Ngay")]
        public string Date { get; set; }
        [JsonProperty("Quan")]
        public string District { get; set; }
        [JsonProperty("Dien_Thoai")]
        public string MobileNumber { get; set; }
        [JsonProperty("Ho_Ten_Ke_Toan_BC")]
        public string PostOfficeAccountantName { get; set; }
        [JsonProperty("Dia_Chi_Thanh_Toan")]
        public string PaymentAddress { get; set; }
        [JsonProperty("Dien_Thoai_Ke_Toan_BC")]
        public string PostOfficeAccountantPhone { get; set; }
        [JsonProperty("Ty_Le_CH")]
        public long ReturnRate { get; set; }
        [JsonProperty("Ty_Le_TT")]
        public long AgreementRate { get; set; }
        [JsonProperty("Ty_Le_TMDT")]
        public long EcommerceRate { get; set; }
        [JsonProperty("Cuoc_Khac")]
        public long OtherCharges { get; set; }
        [JsonProperty("Hoan_Cuoc_DT")]
        public long RefundRevenue { get; set; }
        [JsonProperty("Buu_Kien_DT")]
        public long ParcelRevenue { get; set; }
        [JsonProperty("Buu_Pham_Thuong_DT")]
        public long RegularParcelRevenue { get; set; }
        [JsonProperty("Buu_Pham_GS_DT")]
        public long RegisteredParcelRevenue { get; set; }
        [JsonProperty("Cuoc_Dieu_Chinh_DT")]
        public long AdjustmentFeeRevenue { get; set; }
        [JsonProperty("Cuoc_Dieu_Chinh_Khac_DT")]
        public long OtherAdjustmentChargesRevenue { get; set; }
        [JsonProperty("Truy_Thu_DT")]
        public long ArrearsRevenue { get; set; }
        [JsonProperty("DV_EMS_Cong_Them_DT")]
        public long EMSExtraServiceRevenue { get; set; }
        [JsonProperty("KM_Theo_CV_DT")]
        public long PromotionCVRevenue { get; set; }
        [JsonProperty("Quoc_Te_Priority_DT")]
        public long InternationalPriorityRevenue { get; set; }
        [JsonProperty("Trong_Nuoc_DT")]
        public long DomesticRevenue { get; set; }
        [JsonProperty("Quoc_Te_DT")]
        public long InternationalRevenue { get; set; }
        [JsonProperty("Chuyen_Hoan_DT")]
        public long PermuteRevenue { get; set; }
        [JsonProperty("Thoa_Thuan_DT")]
        public long NegotiableRevenue { get; set; }
        [JsonProperty("TMDT_DT")]
        public long EcommerceRevenue { get; set; }
        [JsonProperty("Ty_Le_TN")]
        public long DomesticRate { get; set; }
        [JsonProperty("Ty_Le_QT")]
        public long InternationalRate { get; set; }
        [JsonProperty("Ty_Le_Priority")]
        public long PriorityRate { get; set; }
        [JsonProperty("So_Hoa_Don")]
        public string InvoiceNumber { get; set; }
    }
    public class ZReconciliationSearchDebtNoticesDataInfoDetails
    {
        [JsonProperty("STT")]
        public int RowNumber { get; set; }
        [JsonProperty("Dia_Chi")]
        public string Address { get; set; }
        [JsonProperty("Khoi_Luong_QD")]
        public long ConversionVolume { get; set; }
        [JsonProperty("Noi_Phat")]
        public string Internal { get; set; }
        [JsonProperty("Nguoi_Nhan")]
        public string Receiver { get; set; }
        [JsonProperty("Ma_E1")]
        public string ItemCode { get; set; }
        [JsonProperty("Ngay_Phat_Hanh")]
        public string ReleaseDate { get; set; }
        [JsonProperty("Ma_Bc_Tra")]
        public string PostOfficeReturn { get; set; }
        [JsonProperty("Khoi_Luong")]
        public long Weight { get; set; }
        [JsonProperty("Ma_KH")]
        public string CustomerCode { get; set; }
        [JsonProperty("Ten_Khach_Hang")]
        public string CustomerName { get; set; }
        [JsonProperty("Cuoc_Chinh")]
        public long MainFee { get; set; }
        [JsonProperty("Cuoc_COD")]
        public long CODFee { get; set; }
        [JsonProperty("Cuoc_DV")]
        public long ServiceFee { get; set; }
        [JsonProperty("DcNhan")]
        public string ReceiverAddress { get; set; }
        [JsonProperty("Dien_Thoai_Nhan")]
        public string ReceiverTel { get; set; }
        [JsonProperty("Ghi_Chu")]
        public string Content { get; set; }
        [JsonProperty("Gia_Tri")]
        public long Value { get; set; }
        [JsonProperty("So_Tham_Chieu")]
        public string ReferenceNumber { get; set; }
        [JsonProperty("Cuoc_Dieu_Chinh")]
        public long AdjustmentFee { get; set; }
        [JsonProperty("Ma_Bc_Khai_Thac")]
        public string PostOfficeExploitCode { get; set; }
        [JsonProperty("BC_Khai_Thac")]
        public string PostOfficeExploit { get; set; }
    }
}
