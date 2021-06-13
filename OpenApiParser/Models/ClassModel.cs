using System.Collections.Generic;

namespace OpenApiParser.Models
{
    public record ClassModel
    {
        public string Name { get; set; }
        public IList<Property> Properties { get; set; }
        public IEnumerable<string> Enums { get; set; }
    }
}