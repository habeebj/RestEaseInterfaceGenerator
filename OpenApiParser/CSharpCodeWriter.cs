using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenApiParser.Extensions;
using OpenApiParser.Models;

namespace OpenApiParser
{
    public class CSharpCodeWriter : ICodeWriter
    {
        public string GetPropertyType(PropertySchema propertySchema)
        {
            if (propertySchema == null)
                return "object";

            if (!string.IsNullOrEmpty(propertySchema.Reference))
                return propertySchema.Reference.Split('/').Last();

            switch (propertySchema.Type)
            {
                case "array":
                {
                    // return array of something - check model.items
                    var prop = this.GetPropertyType(propertySchema.Items);
                    return $"List<{prop}>";
                }
                case "object":
                {
                    var prop = this.GetPropertyType(propertySchema.AdditionalPropertiesSchema);
                    return $"Dictionary<string, {prop}>";
                }
            }

            if (propertySchema.Format == "date-time")
                propertySchema.Type = "DateTime";

            if (propertySchema.Format == "binary")
                propertySchema.Type = "byte[]";

            if (propertySchema.Format == "int32" || propertySchema.Format == "int64")
                propertySchema.Type = "int";

            if (propertySchema.Format == "double" || propertySchema.Format == "float")
                propertySchema.Type = propertySchema.Format;

            // TODO: Handle Int, Double & Float

            if (propertySchema.IsNullable)
                propertySchema.Type += "?";

            return propertySchema.Type;
        }

        public StringBuilder WriteInterface(IEnumerable<InterfaceActionModel> actionModels)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"[SerializationMethods(Query = QuerySerializationMethod.Serialized)]");
            sb.AppendLine($"public interface I{Config.ServiceName()}Service \n{{");

            foreach (var model in actionModels)
            {
                sb.AppendLine($"\t{GetMethodAttribute(model)}");
                sb.AppendLine($"\t{GetMethodSignature(model)}");
                sb.AppendLine();
            }

            sb.AppendLine("}");

            return sb;
        }

        public StringBuilder WriteClassModel(IEnumerable<ClassModel> classModels)
        {
            var sb = new StringBuilder();
            if (classModels == null)
                return sb;
            
            foreach (var classModel in classModels)
            {
                var isEnum = classModel.Enums != null && classModel.Enums.Any();
                var objectType = isEnum ? "enum" : "class";

                sb.AppendLine($"public {objectType} {classModel.Name} \n{{");
                foreach (var classProperty in classModel.Properties)
                {
                    sb.AppendLine($"\tpublic {classProperty.Type} {classProperty.Name.ToPascalCase()} {{ get; set; }}");
                }

                if (classModel.Enums != null && classModel.Enums.Any())
                    sb.AppendLine($"\t{string.Join(", ", classModel.Enums)}");

                sb.AppendLine("}");
                sb.AppendLine();
            }

            return sb;
        }


        private bool isCSharpType(string s)
        {
            var keywords = new List<string>
            {
                "string", "int", "double", "float", "bool", "boolean", "dictionary", "list", "object"
            };
            return  keywords.Any(k => s.ToLower().Contains(k));
        }

        private string GetMethodAttribute(InterfaceActionModel model)
        {
            var path = "";
            if (!string.IsNullOrEmpty(Config.BasePath))
                path += $"{Config.BasePath.TrimStart('/')}/";
            path += model.Url.TrimStart('/');
            
            return $"[{model.HttpMethod}(\"{path}\")]";
        }

        private string GetParameters(IList<Parameter> parameters)
        {
            if (parameters == null)
                return string.Empty;

            var parameterString = string.Empty;
            for (var i = 0; i < parameters.Count; i++)
            {
                if (i > 0)
                    parameterString += ", ";
                // TODO: check if Query attrib is available
                var parameter = parameters[i];
                parameterString +=
                    $"[{parameter.ParameterLocation.ToString().ToPascalCase()}] {this.GetPropertyType(parameter.PropertySchema)} {parameter.Name} ";
            }

            return parameterString;
        }

        private string GetMethodSignature(InterfaceActionModel model)
        {
            var parameters = GetParameters((model.Parameters));
            var request = GetRequestBody(model.Request);
            if (!string.IsNullOrEmpty(parameters) && !string.IsNullOrEmpty(request))
                parameters += ", ";
            parameters += request;

            var returnType = string.IsNullOrEmpty(model.Response) ? "Task" : $"Task<{model.Response}>";

            return $"{returnType} {model.MethodName}({parameters});";
        }

        private string GetRequestBody(string request)
        {
            // TODO: check if requset is of csharp keyword
            if (string.IsNullOrEmpty(request))
                return string.Empty;

            var parameterName = request.Replace("?", "");
            if (isCSharpType(parameterName))
                parameterName = "prop";

            return $"[Body] {request} {parameterName.ToCamelCase()}";
        }
    }
}