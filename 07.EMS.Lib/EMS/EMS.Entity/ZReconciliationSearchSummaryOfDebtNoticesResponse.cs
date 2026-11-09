using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZReconciliationSearchSummaryOfDebtNoticesResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public ZReconciliationSearchSummaryOfDebtNoticesData Data { get; set; }
    }
    public class ZReconciliationSearchSummaryOfDebtNoticesData
    {
        [JsonProperty("ma_kh")]
        public string CustomerCode { get; set; }

        [JsonProperty("thang_no")]
        public int MonthYear { get; set; }

        [JsonProperty("thong_tin_chung")]
        public ZReconciliationSearchSummaryOfDebtNoticesDataInfo DataInfo { get; set; }

        [JsonProperty("thong_tin_chi_tiet")]
        public List<ZReconciliationSearchSummaryOfDebtNoticesDataInfoDetails> DataInfoDetails { get; set; }
    }
    public class ZReconciliationSearchSummaryOfDebtNoticesDataInfo
    {
        [JsonProperty("menu_ten_chi_nhanh")]
        public string BranchNameMenu { get; set; }

        [JsonProperty("menu_ten_ttdvkh")]
        public string CustomerServiceCenterNameMenu { get; set; }

        [JsonProperty("menu_thong_tin_thanh_toan")]
        public string PaymentInformationMenu { get; set; }
    }
    public class ZReconciliationSearchSummaryOfDebtNoticesDataInfoDetails
    {
        [JsonProperty("Ten_Khach_Hang")]
        public string CustomerName { get; set; }

        [JsonProperty("Ma_KH")]
        public string CustomerCode { get; set; }

        [JsonProperty("Ma_Buu_Cuc_Thanh_Toan")]
        public string PaymentPostCode { get; set; }

        [JsonProperty("Dien_Thoai_Ke_Toan_BC")]
        public string AccountantPhone { get; set; }

        [JsonProperty("Ho_Ten_Ke_Toan_BC")]
        public string AccountantName { get; set; }

        [JsonProperty("Dia_Chi_Buu_Cuc_Thanh_Toan")]
        public string PaymentPostAddress { get; set; }

        [JsonProperty("So_Hoa_Don")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("Quoc_Te_SL")]
        public long InterParcelQuantity { get; set; }

        [JsonProperty("Quoc_Te_DT_Truoc_VAT")]
        public long InterEBT { get; set; }

        [JsonProperty("Quoc_Te_DT_VAT")]
        public long InterVAT { get; set; }

        [JsonProperty("Quoc_Te_DT")]
        public long InterPAT { get; set; }

        [JsonProperty("Trong_Nuoc_SL")]
        public long DomesticParcelQuantity { get; set; }

        [JsonProperty("Trong_Nuoc_DT")]
        public long DomesticRevenue { get; set; }

        [JsonProperty("Trong_Nuoc_DT_Truoc_VAT")]
        public long DomesticEBT { get; set; }

        [JsonProperty("Trong_Nuoc_DT_VAT")]
        public long DomesticVAT { get; set; }

        [JsonProperty("Dich_Vu_EMS_SL")]
        public long EmsServiceQuantity { get; set; }

        [JsonProperty("Dich_Vu_EMS_DT_Truoc_VAT")]
        public long EmsServiceEBT { get; set; }

        [JsonProperty("Dich_Vu_EMS_DT_VAT")]
        public long EmsServiceVAT { get; set; }

        [JsonProperty("Dich_Vu_EMS_DT")]
        public long RevenueEmsService { get; set; }

        [JsonProperty("Chuyen_Hoan_SL")]
        public long ReturneParcelQuantity { get; set; }

        [JsonProperty("Chuyen_Hoan_DT_Truoc_VAT")]
        public long ReturnedParcelEBT { get; set; }

        [JsonProperty("Chuyen_Hoan_DT_VAT")]
        public long ReturnedParcelVAT { get; set; }

        [JsonProperty("Chuyen_Hoan_DT")]
        public long ReturnedParcelRevenue { get; set; }

        [JsonProperty("Cuoc_Dieu_Chinh_SL")]
        public long AdjustChargeQuantity { get; set; }

        [JsonProperty("Cuoc_Dieu_Chinh_DT_Truoc_VAT")]
        public long AdjustChargeEBT { get; set; }

        [JsonProperty("Cuoc_Dieu_Chinh_DT_VAT")]
        public long AdjustChargeVAT { get; set; }

        [JsonProperty("Cuoc_Dieu_Chinh_DT")]
        public long AdjustChargeRevenue { get; set; }

        [JsonProperty("Cuoc_Dieu_Chinh_SL_Extend")]
        public long AdjustChargeQuantityExtend { get; set; }

        [JsonProperty("Cuoc_Dieu_Chinh_Extend_DT_Truoc_VAT")]
        public long AdjustChargeQuantityExtendEBT { get; set; }

        [JsonProperty("Cuoc_Dieu_Chinh_Extend_DT_VAT")]
        public long AdjustChargeQuantityExtendVAT { get; set; }

        [JsonProperty("Cuoc_Dieu_Chinh_Extend_DT")]
        public long AdjustChargeQuantityExtendRevenue { get; set; }

        [JsonProperty("Buu_Kien_SL")]
        public long ParcelQuantity { get; set; }

        [JsonProperty("Buu_Kien_DT_Truoc_VAT")]
        public long ParcelEBT { get; set; }

        [JsonProperty("Buu_Kien_DT_VAT")]
        public long ParcelVAT { get; set; }

        [JsonProperty("Buu_Kien_DT")]
        public long ParcelRevenue { get; set; }

        [JsonProperty("Buu_Pham_Thuong_SL")]
        public long StdParcelQuantity { get; set; }

        [JsonProperty("Buu_Pham_Thuong_DT_Truoc_VAT")]
        public long StdParcelEBT { get; set; }

        [JsonProperty("Buu_Pham_Thuong_DT_VAT")]
        public long StdParcelVAT { get; set; }

        [JsonProperty("Buu_Pham_Thuong_DT")]
        public long StdParcelRevenue { get; set; }

        [JsonProperty("Buu_Pham_GS_SL")]
        public long GSParcelQuantity { get; set; }

        [JsonProperty("Buu_Pham_GS_DT_Truoc_VAT")]
        public long GSParcelEBT { get; set; }

        [JsonProperty("Buu_Pham_GS_DT_VAT")]
        public long GSParcelVAT { get; set; }

        [JsonProperty("Buu_Pham_GS_DT")]
        public long GSParcelRevenue { get; set; }

        [JsonProperty("Hoan_cuoc_COD_SL")]
        public long ReCODQuantity { get; set; }

        [JsonProperty("Hoan_Cuoc_CO_DT_Truoc_VAT")]
        public long ReCODEBT { get; set; }

        [JsonProperty("Hoan_Cuoc_COD_DT_VAT")]
        public long ReCODVAT { get; set; }

        [JsonProperty("Hoan_Cuoc_COD_DT")]
        public long ReCODRevenue { get; set; }

        [JsonProperty("Hoan_cuoc_COD_Extend_SL")]
        public long ReCODExtendQuantity { get; set; }

        [JsonProperty("Hoan_Cuoc_COD_Extend_DT_Truoc_VAT")]
        public long ReCODExtendEBT { get; set; }

        [JsonProperty("Hoan_Cuoc_COD_Extend_DT_VAT")]
        public long ReCODExtendVAT { get; set; }

        [JsonProperty("Hoan_Cuoc_COD_Extend_DT")]
        public long ReCODExtendRevenue { get; set; }

        [JsonProperty("Truy_Thu_SL")]
        public long ArrearsQuantity { get; set; }

        [JsonProperty("Truy_Thu_DT_Truoc_VAT")]
        public long ArrearsEBT { get; set; }

        [JsonProperty("Truy_Thu_DT_VAT")]
        public long ArrearsVAT { get; set; }

        [JsonProperty("Truy_Thu_DT")]
        public long ArrearsRevenue { get; set; }

        [JsonProperty("Truy_Thu_SL_Extend")]
        public long ArrearsExtendQuantity { get; set; }

        [JsonProperty("Truy_Thu_DT_Extend_Truoc_VAT")]
        public long ArrearsExtendEBT { get; set; }

        [JsonProperty("Truy_Thu_DT_Extend_VAT")]
        public long ArrearsExtendVAT { get; set; }

        [JsonProperty("Truy_Thu_DT_Extend")]
        public long ArrearsExtendRevenue { get; set; }

        [JsonProperty("Chiet_Khau_Thuong_Mai_SL")]
        public long TradeDiscontQuantity { get; set; }

        [JsonProperty("Chiet_Khau_Thuong_Mai_DT_Truoc_VAT")]
        public long TradeDiscountEBT { get; set; }

        [JsonProperty("Chiet_Khau_Thuong_Mai_DT_VAT")]
        public long TradeDiscountVAT { get; set; }

        [JsonProperty("Chiet_Khau_Thuong_Mai_DT")]
        public long TradeDiscountRevenue { get; set; }

        [JsonProperty("KM_Theo_CV_SL")]
        public long KM_Theo_CV_SL { get; set; }

        [JsonProperty("KM_Theo_CV_DT_Truoc_VAT")]
        public long KM_Theo_CV_DT_Truoc_VAT { get; set; }

        [JsonProperty("KM_Theo_CV_DT_VAT")]
        public long KM_Theo_CV_DT_VAT { get; set; }

        [JsonProperty("KM_Theo_CV_DT")]
        public long KM_Theo_CV_DT { get; set; }

        [JsonProperty("Doanh_Thu_Khac_SL")]
        public long OtherRevenueQuantity { get; set; }

        [JsonProperty("Doanh_Thu_Khac_DT_Truoc_VAT")]
        public long OtherRevenueEBT { get; set; }

        [JsonProperty("Doanh_Thu_Khac_DT_VAT")]
        public long OtherRevenueVAT { get; set; }

        [JsonProperty("Doanh_Thu_Khac_DT")]
        public long OtherRevenueIncomes { get; set; }

         [JsonProperty("Tong_SL")]
        public long TotalQuantity { get; set; }

        [JsonProperty("Tong_DT_Truoc_VAT")]
        public long TotalEBT { get; set; }

        [JsonProperty("Tong_DT_VAT")]
        public long TotalVAT { get; set; }

        [JsonProperty("Tong_DT")]
        public long TotalRevenue { get; set; }

    }
}
