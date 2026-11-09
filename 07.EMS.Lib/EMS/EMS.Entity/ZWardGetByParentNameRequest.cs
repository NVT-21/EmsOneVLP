using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZWardGetByParentNameRequest
    {
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
    }
    public class ZWardGetByParentNameResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
