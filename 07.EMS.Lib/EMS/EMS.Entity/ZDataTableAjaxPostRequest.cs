using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace EMS.Entity
{
    public class ZDataTableAjaxPostRequest
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }
        [JsonProperty("start")]
        public int Start { get; set; }
        [JsonProperty("length")]
        public int Length { get; set; }
        [JsonProperty("columns")]
        public List<ZDataTableAjaxPostColumnRequest> Columns { get; set; }
        [JsonProperty("search")]
        public ZDataTableAjaxPostSearchRequest Search { get; set; }
        [JsonProperty("order")]
        public List<ZDataTableAjaxPostOrderRequest> Order { get; set; }
    }
    public class ZDataTableAjaxPostColumnRequest
    {
        [JsonProperty("data")]
        public string Data { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("searchable")]
        public bool Searchable { get; set; }
        [JsonProperty("orderable")]
        public bool Orderable { get; set; }
        [JsonProperty("search")]
        public ZDataTableAjaxPostSearchRequest Search { get; set; }
    }
    public class ZDataTableAjaxPostSearchRequest
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("regex")]
        public string Regex { get; set; }
    }
    public class ZDataTableAjaxPostOrderRequest
    {
        [JsonProperty("column")]
        public int Column { get; set; }
        [JsonProperty("dir")]
        public string Dir { get; set; }
    }
}
