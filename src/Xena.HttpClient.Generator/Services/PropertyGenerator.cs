using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Services;

public class PropertyGenerator
{
    public ClientModelProperty Build(string name, OpenApiSchema propertySchema)
    {
        var propertyType = ResolveType(propertySchema);

        return new ClientModelProperty
        {
            Name = name,
            Nullable = propertySchema.Nullable,
            PropertyType = propertyType,
        };
    }

    private string ResolveType(OpenApiSchema schema)
    {
        return schema.Type switch
        {
            "string" => "string",
            "number" when schema.Format == "float" => "float",
            "number" when schema.Format == "double" => "float",
            "number" => "int",
            "integer" when schema.Format == "int64" => "long",
            "integer" => "int",
            "boolean" => "bool",
            "array" when !string.IsNullOrWhiteSpace(schema.Items?.Type) => $"IReadOnlyList<{schema.Items.Type}>",
            "array" => $"IReadOnlyList<object>",
            "object" when !string.IsNullOrWhiteSpace(schema.Reference?.Id) => schema.Reference.Id,
            "object" => "object",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}