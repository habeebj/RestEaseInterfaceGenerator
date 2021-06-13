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
            var argsParser = new ArgsParser<Options>(args);

            #region Sample swagger json

            // "https://localhost:5001";
            // "https://petstore.swagger.io/v2/swagger.json";
            // "https://staging.naijavax.com/swagger/v1/swagger.json";

            #endregion

            try
            {
                var openApiParser = new OpenApiParser(argsParser.Options);
                var csharpWriter = new CSharpCodeWriter();
                openApiParser.AddCodeBuilder(new InterfaceBuilder(csharpWriter));
                openApiParser.AddCodeBuilder(new ClassBuilder(csharpWriter));

                var path = Path.Combine(Directory.GetCurrentDirectory(), argsParser.Options.Output);
                var filename = $"{Config.ServiceName()}Service.cs";
                var fileWriter = new FileWriter(path, filename);
                await openApiParser.GenerateAndSaveOutputAsync(fileWriter);
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