using Newtonsoft.Json;

namespace OpenApiParser.Models
{
    public class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("in")]
        public ParameterLocation ParameterLocation { get; set; }

        [JsonProperty("schema")]
        public PropertySchema PropertySchema { get; set; }
    }
}