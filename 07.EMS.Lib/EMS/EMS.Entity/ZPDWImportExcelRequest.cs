using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZPDWImportExcelRequest
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RegionCode { get; set; }
        public string ProvinceListCode { get; set; }
        public string ProvinceCode { get; set; }
        public int ZoneType { get; set; }
        public int ZoneTypeTT { get; set; }
        public string ImportCode { get; set; }
    }
}
