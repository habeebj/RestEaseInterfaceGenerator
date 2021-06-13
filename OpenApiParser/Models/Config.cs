namespace OpenApiParser.Models
{
    public class Config
    {
        public static InfoModel InfoModel { get; set; }
        public static string Version { get; set; }
        public static string BasePath { get; set; }

        public static string ServiceName()
        {
            if (string.IsNullOrEmpty(InfoModel?.Title))
                return "Api";
            return string.Join("", InfoModel.Title.Split(' '));
        }
    }
}