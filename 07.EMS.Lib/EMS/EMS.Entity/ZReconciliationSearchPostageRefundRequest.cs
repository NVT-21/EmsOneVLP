using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZReconciliationSearchPostageRefundV1Request
    {
        public string CustomerCode { get; set; }
        public int MonthYear { get; set; }
    }
    public class ZReconciliationSearchPostageRefundRequest
    {
        public string ma_kh { get; set; }
        public int thang_no { get; set; }
    }
    public class ZReconciliationSearchPostageRefundResponse
    {

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public ZReconciliationSearchPostageRefundData Data { get; set; }
    }
    public class ZReconciliationSearchPostageRefundData
    {

        [JsonProperty("ma_kh")]
        public string CustomerCode { get; set; }

        [JsonProperty("thang_no")]
        public int MonthYear { get; set; }

        [JsonProperty("thong_tin_chi_tiet")]
        public List<ZReconciliationSearchPostageRefundDataDetails> DataDetails { get; set; }
    }
    public class ZReconciliationSearchPostageRefundDataDetails
    {
        [JsonProperty("STT")]
        public int RowNumber { get; set; }

        [JsonProperty("Ma_KH")]
        public string CustomerCode { get; set; }

        [JsonProperty("ma_e1")]
        public string ItemCode { get; set; }

        [JsonProperty("Ngay_CH")]
        public string RefundDate { get; set; }

        [JsonProperty("Ngay_Phat_Hanh")]
        public string ReleaseDate { get; set; }

        [JsonProperty("Ghi_Chu")]
        public string Note { get; set; }
        
        [JsonProperty("Tham_Chieu")]
        public string Reference { get; set; }

        [JsonProperty("Tien_Nho_Thu")]
        public double MoneyforCollection { get; set; }

        [JsonProperty("Cuoc_COD")]
        public long CODFee { get; set; }
        
        [JsonProperty("Ty_Le_giam_cuoc")]
        public long DiscountRate { get; set; }

        [JsonProperty("Cuoc_COD_CH")]
        public long CODFeeRefund { get; set; }

        [JsonProperty("So_Tien_Hoan_Cuoc")]
        public long RefundAmount { get; set; }
        
        [JsonProperty("Cuoc_E1_CH")]
        public long FeeE1TransferRefund { get; set; }
    }
}
