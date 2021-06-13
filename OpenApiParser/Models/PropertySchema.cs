using Newtonsoft.Json;

namespace OpenApiParser.Models
{
    public class PropertySchema
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("nullable")]
        public bool IsNullable { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("items")]
        public PropertySchema Items { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; } // TODO: sample date-time | double | int

        [JsonProperty("additionalProperties")]
        public PropertySchema AdditionalPropertiesSchema { get; set; }
    }
}