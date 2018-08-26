using Newtonsoft.Json;
using System;

namespace WebApi.Models
{
    public class RequestModel
    {
        [JsonProperty(PropertyName = "ix")]
        public int Index { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "visits")]
        public int? Visits { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
    }
}