using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZVNPMapsSearchRequest
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public int Size { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZVNPMapsSearchResponse
    {
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public ZVNPMapsSearchDataResponse data { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZVNPMapsSearchDataResponse
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string License { get; set; }
        [DataMember]
        public List<double> bbox { get; set; }
        [DataMember]
        public List<ZVNPMapsSearchFeaturesResponse> features { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZVNPMapsSearchFeaturesResponse
    {
        [DataMember]
        public ZVNPMapsSearchGeometryResponse geometry { get; set; }
        [DataMember]
        public ZVNPMapsSearchPropertiesResponse properties { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public List<double> bbox { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZVNPMapsSearchGeometryResponse
    {
        [DataMember]
        public List<double> coordinates { get; set; }
        [DataMember]
        public string type { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZVNPMapsSearchPropertiesResponse
    {
        [DataMember]
        public string distance { get; set; }
        [DataMember]
        public string locality { get; set; }
        [DataMember]
        public string smartcode { get; set; }
        [DataMember]
        public string label { get; set; }
        [DataMember]
        public string region { get; set; }
        [DataMember]
        public string accuracy { get; set; }
        [DataMember]
        public string addendum { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string street { get; set; }
        [DataMember]
        public string county { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string postcode { get; set; }
        [DataMember]
        public string housenumber { get; set; }
        [DataMember]
        public string region_gid { get; set; }
        [DataMember]
        public string locality_gid { get; set; }
        [DataMember]
        public string layer { get; set; }
        [DataMember]
        public string county_gid { get; set; }
    }
}
