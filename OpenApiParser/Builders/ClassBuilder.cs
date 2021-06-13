using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenApiParser.Extensions;
using OpenApiParser.Models;

namespace OpenApiParser.Builders
{
    public class ClassBuilder : CodeBuilderBase<IEnumerable<ClassModel>>, ICodeBuilder
    {
        public ClassBuilder(ICodeWriter codeWriter) : base(codeWriter)
        {
        }

        protected override IEnumerable<ClassModel> Build(JObject jObject)
        {
            var version = int.Parse(Config.Version.Split('.').First());
            var classDefinitionKey = version == 2 ? "definitions" : "components";

            jObject.TryGetValue(classDefinitionKey, out var children);
            if (children is not JObject components)
                throw new Exception("Invalid json. Component key not found.");

            var classes = new List<ClassModel>();

            JToken schemas = null;

            switch (version)
            {
                case 2:
                    schemas = components;
                    break;
                case >= 3:
                {
                    components.TryGetValue("schemas", out schemas);
                    if (schemas == null)
                        throw new Exception("Invalid Open API json file. Schema key not found");
                    break;
                }
            }

            if (schemas == null)
                throw new Exception("Invalid Open API json file. Schema key not found");
            
            foreach (var (className, value) in (JObject) schemas)
            {
                var classModel = new ClassModel
                {
                    Name = className,
                    Properties = new List<Property>()
                };


                if (value.SelectToken("enum") is JArray enumeration)
                {
                    var enums = enumeration.Cast<JValue>().ToArray();
                    classModel.Enums = enums.Select(e => e.Value?.ToString());
                    classes.Add(classModel);
                    continue;
                }

                if (value.SelectToken("properties") is not JObject properties)
                    continue;

                foreach (var property in properties)
                {
                    properties.TryGetValue(property.Key, out var type);
                    try
                    {
                        if (type == null)
                            continue;
                        var propertyObject =
                            JsonConvert.DeserializeObject<PropertySchema>(type.ToString().ReformatReferenceKey());
                        var propertyType = CodeWriter.GetPropertyType(propertyObject);
                        classModel.Properties.Add(new Property(Name: property.Key, Type: propertyType));
                    }
                    catch
                    {
                        // ignored
                    }
                }

                classes.Add(classModel);
            }

            return classes;
        }

        public string GetOutputString(JObject jObject)
        {
            var models = Build(jObject);
            return CodeWriter.WriteClassModel(models).ToString();
        }
    }
}