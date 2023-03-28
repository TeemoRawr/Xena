namespace Xena.HttpClient.Generator.Extensions;

public static class StringExtensions
{
    public static string ToPascalCase(this string s)
    {
        return (char.ToUpperInvariant(s[0]) + s.Substring(1)).Replace("_", string.Empty);
    }
}