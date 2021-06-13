using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenApiParser.Builders;
using OpenApiParser.Models;

namespace OpenApiParser
{
    public class OpenApiParser
    {
        private readonly HttpClient _httpClient;
        private JObject _jObject;
        private readonly IList<ICodeBuilder> _codeBuilders = new List<ICodeBuilder>();

        // options
        public OpenApiParser(string url)
        {
            _httpClient = new HttpClient();
            _jObject = GetJObjectFromUrlAsync(url).GetAwaiter().GetResult();
        }

        public void AddCodeBuilder(ICodeBuilder codeBuilder)
        {
            _codeBuilders.Add(codeBuilder);
        }

        public async Task GenerateAndSaveOutputAsync(IOutputWriter outputWriter)
        {
            var content = GenerateOutput();
            await outputWriter.SaveAsync(content);
        }

        private async Task<JObject> GetJObjectFromUrlAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
                throw new Exception(response.ReasonPhrase);

            var json = await response.Content.ReadAsStringAsync();
            return GetJObject(json);
        }
        private string GenerateOutput()
        {
            var content = GetStringOutputFromJObject(_jObject);
            return content;
        }

        private JObject GetJObject(string jsonString)
        {
            using (var jsonReader = new JsonTextReader(new StringReader(jsonString)))
            {
                var json = JToken.ReadFrom(jsonReader);
                if (json is JArray)
                    throw new Exception("Invalid json file");

                if (json is not JObject jObject)
                    throw new Exception("Invalid json file");

                jObject.TryGetValue("swagger", out var version);
                if (version == null)
                    jObject.TryGetValue("openapi", out version);

                Config.Version = version?.ToString();
                
                jObject.TryGetValue("basePath", out var basPath);
                Config.BasePath = basPath?.ToString();

                try
                {
                    jObject.TryGetValue("info", out var apiInfo);
                    Config.InfoModel = JsonConvert.DeserializeObject<InfoModel>(apiInfo.ToString());
                }
                catch
                {
                    // ignored
                }

                return jObject;
            }
        }

        private string GetStringOutputFromJObject(JObject jObject)
        {
            var sb = new StringBuilder();
            foreach (var codeBuilder in _codeBuilders)
            {
                sb.AppendLine(codeBuilder.GetOutputString(jObject));
            }

            return sb.ToString();
        }
    }
}