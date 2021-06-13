using System.Collections.Generic;

namespace OpenApiParser.Models
{
    public record OpenApiModel
    {
        public Dictionary<string, string> Type { get; set; }
    }
}