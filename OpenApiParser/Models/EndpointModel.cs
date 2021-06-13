using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenApiParser.Models
{
    public class EndpointModel
    {
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("parameters")]
        public List<Parameter> Parameters { get; set; }

        [JsonProperty("requestBody")]
        public RequestBody RequestBody { get; set; }

        [JsonProperty("responses")]
        public Dictionary<string, ResponseBody> Responses { get; set; }
    }
}