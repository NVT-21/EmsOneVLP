using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStatisticalSearchProductRequest
    {
        public int ShopID { get; set; }
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class ZStatisticalSearchProductResponse
    {
        public List<ZStatisticalProductDataTable> DataTable { get; set; }
        public List<ZStatisticalProductDataChart> DataChart { get; set; }
    }
    public class ZStatisticalProductDataTable
    {
        public string ProductCode { get; set; }
        public long TotalProduct { get; set; }
        public long TotalOrderValue { get; set; }
        public double Percentage { get; set; }
    }
    public class ZStatisticalProductDataChart
    {
        public string ProductCode { get; set; }
        public long TotalProduct { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }

    // Version 2

    #region WEB
    public class ZStatisticalProductWeb
    {
        public List<ZStatisticalSearchProductSellingWeb> ProductSelling { get; set; }
        public List<ZStatisticalSearchProductRefundWeb> ProductRefund { get; set; }
    }
    public class ZStatisticalSearchProductSellingWeb
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long TotalProduct { get; set; }
        public long TotalCOD { get; set; }
    }
    public class ZStatisticalSearchProductRefundWeb
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long TotalProduct { get; set; }
        public long TotalCOD { get; set; }
    }
    #endregion

    #region ANDROID
    public class ZStatisticalProductAndroid
    {
        public List<ZStatisticalSearchProductSellingAndroid> ProductSelling { get; set; }
        public List<ZStatisticalSearchProductRefundAndroid> ProductRefund { get; set; }
    }
    public class ZStatisticalSearchProductSellingAndroid
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long TotalProduct { get; set; }
        public long TotalCOD { get; set; }
    }
    public class ZStatisticalSearchProductRefundAndroid
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long TotalProduct { get; set; }
        public long TotalCOD { get; set; }
    }
    #endregion

    #region IOS
    public class ZStatisticalProductIOS
    {
        public List<ZStatisticalSearchProductSellingIOS> ProductSelling { get; set; }
        public List<ZStatisticalSearchProductRefundIOS> ProductRefund { get; set; }
    }
    public class ZStatisticalSearchProductSellingIOS
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long TotalProduct { get; set; }
        public long TotalCOD { get; set; }
    }
    public class ZStatisticalSearchProductRefundIOS
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public long TotalProduct { get; set; }
        public long TotalCOD { get; set; }
    }
    #endregion
}
