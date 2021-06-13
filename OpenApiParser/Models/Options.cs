using CommandLine;

namespace OpenApiParser.Models
{
    public class Options
    {
        [Option('u', "url", Required = true, HelpText = "Set the swagger json URL.")]
        public string Url { get; set; }
        [Option('o', "output", Required = false, HelpText = "Output Directory.", Default = "")]
        public string Output { get; set; }
    }
}