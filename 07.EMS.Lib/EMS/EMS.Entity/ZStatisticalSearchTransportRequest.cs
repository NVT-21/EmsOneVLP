using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZStatisticalSearchTransportRequest
    {
        public int ShopID { get; set; }
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ZStatisticalTransport
    {
        public string Status { get; set; }
        public long TotalCOD { get; set; }
        public long TransportFee { get; set; }
    }

    public class ZStatisticalSearchTransportResponse
    {
        public List<ZStatisticalTransportDataTable> DataTable { get; set; }
        public List<ZStatisticalTransportDataChart> DataChart { get; set; }
        //public int RowIndex { get; set; }
        //public string StatusName { get; set; }
        //public string Status { get; set; }
        //public long AmountTracking { get; set; }
        //public long TotalCOD { get; set; }
        //public long TotalCharge { get; set; }
    }

    public class ZStatisticalTransportDataTable
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public long AmountTracking { get; set; }
        public long TotalCOD { get; set; }
        public long TotalCharge { get; set; }
    }
    public class ZStatisticalTransportDataChart
    {
        public int RowIndex { get; set; }
        public string StatusName { get; set; }
        public string Status { get; set; }
        public double Percent { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }

}
