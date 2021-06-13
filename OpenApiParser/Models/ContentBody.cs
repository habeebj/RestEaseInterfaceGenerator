using Newtonsoft.Json;

namespace OpenApiParser.Models
{
    public class ContentBody
    {
        [JsonProperty("schema")]
        public PropertySchema Schema { get; set; }
    }
}