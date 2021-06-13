using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenApiParser.Extensions;
using OpenApiParser.Models;

namespace OpenApiParser.Builders
{
    public class InterfaceBuilder : CodeBuilderBase<IEnumerable<InterfaceActionModel>>, ICodeBuilder
    {
        public InterfaceBuilder(ICodeWriter codeWriter) : base(codeWriter)
        {
        }
        private IEnumerable<InterfaceActionModel> GetActionModels(JObject jObject)
        {
            jObject.TryGetValue("paths", out var children);
            var paths = new Dictionary<string, Dictionary<string, EndpointModel>>();
            if (children is not JObject pathsObject)
                throw new ArgumentException("Paths key not found");

            foreach (var path in pathsObject)
            {
                // Console.WriteLine(path.Key);
                if (path.Value is not JObject pathObjects) continue;
                var pathContent = new Dictionary<string, EndpointModel>();
                foreach (var pathObject in pathObjects)
                {
                    // Console.WriteLine(pathObject.Key);
                    var jsonPath = path.Value.ToString().ReformatReferenceKey();
                    try
                    {
                        pathContent = JsonConvert.DeserializeObject<Dictionary<string, EndpointModel>>(jsonPath);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                paths.Add(path.Key, pathContent);
            }

            var models = new List<InterfaceActionModel>();

            foreach (var path in paths)
            {
                var url = path.Key;
                foreach (var endpoints in path.Value)
                {
                    var method = endpoints.Key;

                    var responseType = string.Empty;
                    var requestType = string.Empty;
                    
                    var key = "application/json";

                    foreach (var response in
                        endpoints.Value.Responses.Where(response => response.Value.Content != null))
                    {
                        response.Value.Content.TryGetValue(key, out var content);
                        if(content == null)
                            continue;
                        responseType =
                            this.CodeWriter.GetPropertyType(content.Schema);
                        break; // get first schema
                    }

                    if (endpoints.Value.RequestBody != null)
                    {
                        endpoints.Value.RequestBody.Content.TryGetValue(key, out var content);
                        if(content == null)
                            endpoints.Value.RequestBody.Content.TryGetValue("multipart/form-data", out content);
                        
                        if(content == null)
                            continue;
                            
                        requestType =
                            CodeWriter.GetPropertyType(content.Schema);
                    }

                    var model = new InterfaceActionModel
                    {
                        HttpMethod = method.ToPascalCase(),
                        Url = url,
                        Tags = endpoints.Value.Tags,
                        Parameters = endpoints.Value.Parameters,
                        Response = responseType,
                        Request = requestType
                    };
                    models.Add(model);
                }
            }

            return models;
        }

        protected override IEnumerable<InterfaceActionModel> Build(JObject jObject)
        {
             return GetActionModels(jObject);
        }

        public string GetOutputString(JObject jObject)
        {
            var models = Build(jObject);
            return CodeWriter.WriteInterface(models).ToString();
        }
    }
}