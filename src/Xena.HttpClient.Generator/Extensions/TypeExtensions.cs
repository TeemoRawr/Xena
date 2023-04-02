using TypeNameFormatter;

namespace Xena.HttpClient.Generator.Extensions;

public static class TypeExtensions
{
    public static string GetNiceName(this Type type)
    {
        var formattedName = type.GetFormattedName();

        if (type.IsAssignableTo(typeof(Attribute)))
        {
            formattedName = formattedName.Replace(nameof(Attribute), string.Empty);
        }

        return formattedName;
    }
}