using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Extensions;

public static class TypeResolver
{
    public static string Resolve(OpenApiSchema itemsSchema)
    {
        string ResolveOpenApiType(string typeToResolve, string format)
        {
            var resolvedType = typeToResolve switch
            {
                "text" or "string" when format == "date" => typeof(DateTime),
                "text" or "string" when format == "date-time" => typeof(DateTime),
                "text" or "string" when format == "url" => typeof(Uri),
                "text" or "string" => typeof(string),
                "number" when format == "double" => typeof(double),
                "number" when format == "float" => typeof(float),
                "number" => typeof(int),
                "integer" when format == "int64" => typeof(long),
                "integer" => typeof(int),
                _ => typeof(object)
            };

            return resolvedType.GetNiceName();
        }

        if (!string.IsNullOrWhiteSpace(itemsSchema.Reference?.Id) && itemsSchema.Items is null)
        {
            return itemsSchema.Reference.Id;
        }

        string resolvedType;

        if (itemsSchema.Items is not null)
        {
            var resolvedArrayType = Resolve(itemsSchema.Items);
            resolvedType = $"IReadOnlyList<{resolvedArrayType}>";
        }
        else
        {
            resolvedType = ResolveOpenApiType(itemsSchema.Type, itemsSchema.Format);
        }

        if (itemsSchema.Nullable)
        {
            resolvedType = $"{resolvedType}?";
        }

        return resolvedType;
    }
}