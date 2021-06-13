using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenApiParser.Models
{
    public class ResponseBody
    {
        [JsonProperty("content")]
        public Dictionary<string, ContentBody> Content { get; set; }
    }
}