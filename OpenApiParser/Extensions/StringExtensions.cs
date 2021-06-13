namespace OpenApiParser.Extensions
{
    public static class StringExtensions
    {
        public static string ReformatReferenceKey(this string s) => s.Replace("$ref", "reference");

        public static string ToPascalCase(this string s)
        {
            var text = s;
            if (s.Length > 0)
                text = char.ToUpper(s[0]).ToString();

            if (s.Length > 1)
                text += s[1..];

            return text;
        }
        public static string ToCamelCase(this string s)
        {
            var text = s;
            if (s.Length > 0)
                text = char.ToLower(s[0]).ToString();

            if (s.Length > 1)
                text += s[1..];

            return text;
        }
    }
}