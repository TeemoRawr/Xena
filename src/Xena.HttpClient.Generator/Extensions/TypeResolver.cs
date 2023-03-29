using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Extensions;

public static class TypeResolver
{
    public static string Resolve(OpenApiSchema itemsSchema) => 
        Resolve(itemsSchema.Reference, itemsSchema.Type, itemsSchema.Format, itemsSchema.Nullable);
    
    public static string Resolve(OpenApiReference? itemsSchemaReference, string type, string? format, bool nullable)
    {
        if (!string.IsNullOrWhiteSpace(itemsSchemaReference?.Id))
        {
            return itemsSchemaReference.Id;
        }
        
        var typeAsString = type switch
        {
            "text" or "string" when format == "date" => "DateTime",
            "text" or "string" when format == "date-time" => "DateTime",
            "text" or "string" when format == "url" => "Uri",
            "text" or "string" => "string",
            "number" when format == "double" => "double",
            "number" when format == "float" => "double",
            "number" => "int",
            "integer" when format == "int64" => "long",
            "integer" => "int",
            _ => "object"
        };

        return $"{typeAsString}{(nullable ? "?" : "")}";
    }
}