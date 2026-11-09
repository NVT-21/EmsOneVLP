using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStatisticalSearchCashflowRequest
    {
        public int ShopID { get; set; }
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class ZStatisticalSearchCashflow
    {
        public string Status { get; set; }
        public long TotalCOD { get; set; }
        public long TotalTransportFee { get; set; }
    }
    public class ZStatisticalSearchCashflowResponse
    {
        public List<ZStatisticalCashflowDataTable> DataTable { get; set; }
        public List<ZStatisticalCashflowDataChart> DataChart { get; set; }
        //public int RowIndex { get; set; }
        //public string StatusName { get; set; }
        //public string Status { get; set; }
        //public long Amount { get; set; }
        //public long TotalCOD { get; set; }
        //public long TotalTransportFee { get; set; }
        //public double PercentCOD { get; set; }
    }

    public class ZStatisticalCashflowDataTable
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long Amount { get; set; }
        public long TotalCOD { get; set; }
        public long TotalTransportFee { get; set; }
        public double PercentCOD { get; set; }
    }
    public class ZStatisticalCashflowDataChart
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public double PercentCOD { get; set; }
        public long TotalCOD { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }

    // Version 2

    #region WEB
    public class ZStatisticalSearchCashflowWeb
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long TotalCOD { get; set; }
        public long TotalTransportFee { get; set; }
    }
    #endregion

    #region ANDROID
    public class ZStatisticalSearchCashflowAndroid
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long TotalCOD { get; set; }
        public long TotalTransportFee { get; set; }
    }
    #endregion

    #region IOS
    public class ZStatisticalSearchCashflowIOS
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long TotalCOD { get; set; }
        public long TotalTransportFee { get; set; }
    }
    #endregion
}
