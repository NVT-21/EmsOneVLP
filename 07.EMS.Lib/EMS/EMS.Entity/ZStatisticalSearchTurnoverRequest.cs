using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTurnoverRequest
    {
        public string PPCRMCode { get; set; }
        public int ShopID { get; set; }
        public string Date { get; set; }
    }

    public class ZTurnover
    {
        public long TurnoverToday { get; set; }
        public long TurnoverWeek { get; set; }
        public long TurnoverMonth { get; set; }
    }
    public class ZTurnoverResponse
    {
        public string Type { get; set; }
        public string Lable { get; set; }
        public long Value { get; set; }
    }
    public class ZStatisticalSearchTurnoverRequest
    {
        public int ShopID { get; set; }
        public string PPCRMCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ZStatisticalSearchTurnoverResponse
    {
        public List<ZTurnoverResponse> DataLable { get; set; }
        public List<ZStatisticalTurnoverDataChart> DataChart { get; set; }
    }

    public class ZStatisticalTurnoverDataChart
    {
        public int RowIndex { get; set; }
        public string Date { get; set; }
        public long TotalOrderValue { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
    }
}
