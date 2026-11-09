using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStatisticalSearchCustomerRequest
    {
        public int ShopID { get; set; }
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class ZStatisticalSearchCustomerResponse
    {
        public List<ZStatisticalCustomerDataTable> DataTable { get; set; }
        public List<ZStatisticalCustomerDataChart> DataChart { get; set; }
        //public string ProvinceName { get; set; }
        //public long TotalOrderProvince { get; set; }
        //public long TotalOrderValue { get; set; }
        //public double Percentage { get; set; }
    }

    public class ZStatisticalCustomerDataTable
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalOrderValue { get; set; }
        public double Percentage { get; set; }
    }
    public class ZStatisticalCustomerDataChart
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }

    // Version 2

    #region WEB
    public class ZStatisticalCustomerWeb
    {
        public List<ZStatisticalSearchCustomerSellingWeb> CustomerSelling { get; set; }
        public List<ZStatisticalSearchCustomerRefundWeb> CustomerRefund { get; set; }
    }
    public class ZStatisticalSearchCustomerSellingWeb
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalCOD { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    public class ZStatisticalSearchCustomerRefundWeb
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalCOD { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    #endregion

    #region ANDROID
    public class ZStatisticalCustomerAndroid
    {
        public List<ZStatisticalSearchCustomerSellingAndroid> CustomerSelling { get; set; }
        public List<ZStatisticalSearchCustomerRefundAndroid> CustomerRefund { get; set; }
    }
    public class ZStatisticalSearchCustomerSellingAndroid
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalCOD { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    public class ZStatisticalSearchCustomerRefundAndroid
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalCOD { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    #endregion

    #region IOS
    public class ZStatisticalCustomerIOS
    {
        public List<ZStatisticalSearchCustomerSellingIOS> CustomerSelling { get; set; }
        public List<ZStatisticalSearchCustomerRefundIOS> CustomerRefund { get; set; }
    }
    public class ZStatisticalSearchCustomerSellingIOS
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalCOD { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    public class ZStatisticalSearchCustomerRefundIOS
    {
        public string ProvinceName { get; set; }
        public long TotalOrderProvince { get; set; }
        public long TotalCOD { get; set; }
        public double Percentage { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
    #endregion
}
