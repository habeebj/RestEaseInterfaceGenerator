using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using OpenApiParser.Builders;
using OpenApiParser.Extensions;
using OpenApiParser.Models;

namespace OpenApiParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // get arguments from args
            try
            {
                // Options
                var url = "https://localhost:5001";
                // url = "https://petstore.swagger.io/v2/swagger.json";
                url = "https://staging.naijavax.com/swagger/v1/swagger.json";
                url = url.EndsWith(".json") ? url : $"{url}/swagger/v1/swagger.json";

                var openApiParser = new OpenApiParser(url);

                var csharpWriter = new CSharpCodeWriter();
                openApiParser.AddCodeBuilder(new InterfaceBuilder(csharpWriter));
                openApiParser.AddCodeBuilder(new ClassBuilder(csharpWriter));

                var path = Path.Combine(Directory.GetCurrentDirectory(), $"{Config.ServiceName()}Service.cs");
                var fileWriter = new FileWriter(path);
                await openApiParser.GenerateAndSaveOutputAsync(fileWriter);

                Console.WriteLine($"File Created: {path}");
            }
            catch (HttpRequestException ex)
            {
                var msg = string.Empty;
                if (ex.StatusCode == null)
                    msg = "Seems your internet connection is down";
                Console.WriteLine(string.IsNullOrEmpty(msg) ? ex.Message : msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
    }
}