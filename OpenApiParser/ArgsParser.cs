using CommandLine;

namespace OpenApiParser
{
    public class ArgsParser<T> where T : class
    {
        public T Options { get; set; }

        public ArgsParser(string[] args)
        {
            Parser.Default.ParseArguments<T>(args)
                .WithParsed<T>(o => { this.Options = o; });
        }
    }
}