using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Services.ModelGenerator;

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
            IsCollection = propertySchema.Type == "array"
        };
    }

    private string ResolveType(OpenApiSchema schema)
    {
        string GetArrayType()
        {
            var referenceId = schema.Items.Reference?.Id;
            if(!string.IsNullOrWhiteSpace(referenceId))
            {
                return referenceId;
            }

            var itemType = schema.Items.Type;
            if (!string.IsNullOrWhiteSpace(itemType))
            {
                return itemType;
            }

            return "object";
        }
        
        return schema.Type switch
        {
            "string" => typeof(string).FullName,
            "number" when schema.Format == "float" => typeof(float).FullName,
            "number" when schema.Format == "double" => typeof(double).FullName,
            "number" => typeof(int).FullName,
            "integer" when schema.Format == "int64" => typeof(long).FullName,
            "integer" => typeof(int).FullName,
            "boolean" => typeof(bool).FullName,
            "array" => $"IReadOnlyList<{GetArrayType()}>",
            "object" when !string.IsNullOrWhiteSpace(schema.Reference?.Id) => schema.Reference.Id,
            "object" => typeof(object).FullName,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}