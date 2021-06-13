using System.Collections.Generic;
using System.Linq;
using OpenApiParser.Extensions;

namespace OpenApiParser.Models
{
    public class InterfaceActionModel
    {
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public List<string> Tags { get; set; }
        public string Response { get; set; }
        public string Request { get; set; }

        public string MethodName
        {
            get
            {
                var methodPrefix = HttpMethod == "Put" ? "Update" : HttpMethod;
                var methodName = $"{methodPrefix}{Tags.FirstOrDefault().ToPascalCase()}";
                if (Parameters == null)
                    return methodName;
                foreach (var parameter in Parameters)
                {
                    if (Parameters.IndexOf(parameter) == 0)
                        methodName += "By";
                    else
                        methodName += "And";
                    methodName += parameter.Name.ToPascalCase();
                }
                return methodName;
            }
        }

        public List<Parameter> Parameters { get; set; }
    }
}