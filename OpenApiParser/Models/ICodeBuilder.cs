using Newtonsoft.Json.Linq;

namespace OpenApiParser.Models
{
    public interface ICodeBuilder
    {
        string GetOutputString(JObject jObject);
    }
}