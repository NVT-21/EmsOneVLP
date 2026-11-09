using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    //Bỏ
    public class XValueModel
    {
        public string Checked { get; set; }   // Y - N
        public int Unit { get; set; }     // 1(VNĐ) - 2(%)
        public double Value { get; set; }
        public double Percent { get; set; }
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public string InOfProvince { get; set; }
        public string OutOfProvince { get; set; }
    }
}
