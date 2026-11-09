using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStatisticalSearchOrderRequest
    {
        public int ShopID { get; set; }
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class ZStatisticalSearchOrder
    {
        public string Status { get; set; }
        public long TotalOrderValue { get; set; }
        public long TotalCharge { get; set; }
    }
    public class ZStatisticalSearchOrderResponse
    {
        public List<ZStatisticalOrderDataTable> DataTable { get; set; }
        public List<ZStatisticalOrderDataChart> DataChart { get; set; }
        //public int RowIndex { get; set; }
        //public string StatusName { get; set; }
        //public string Status { get; set; }
        //public long AmountOrder { get; set; }
        //public long TotalOrderValue { get; set; }
        //public long TotalCharge { get; set; }
    }
    public class ZStatisticalOrderDataTable
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long AmountOrder { get; set; }
        public long TotalOrderValue { get; set; }
        public long TotalCharge { get; set; }
    }
    public class ZStatisticalOrderDataChart
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public double Percent { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }

    // Version 2

    #region WEB
    public class ZStatisticalSearchOrderWeb
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long Quantity { get; set; }
        public long TotalCOD { get; set; }
    }
    #endregion

    #region ANDROID
    public class ZStatisticalSearchOrderAndroid
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long Quantity { get; set; }
        public long TotalCOD { get; set; }
    }
    #endregion

    #region IOS
    public class ZStatisticalSearchOrderIOS
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long Quantity { get; set; }
        public long TotalCOD { get; set; }
    }
    #endregion
}

