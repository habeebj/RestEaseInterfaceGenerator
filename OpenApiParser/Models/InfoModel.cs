using Newtonsoft.Json;

namespace OpenApiParser.Models
{
    public class InfoModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}