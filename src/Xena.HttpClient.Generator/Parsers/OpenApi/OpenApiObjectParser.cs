using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public class OpenApiObjectParser : OpenApiBaseParser
{
    public OpenApiObjectParser(OpenApiBaseParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, OpenApiParserOptions options)
    {
        return options.IsRoot && (schema.Type == "object" || !string.IsNullOrWhiteSpace(schema.Reference?.Id));
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument, OpenApiParserOptions options)
    {
        return new ObjectCodeModel(name, openApiSchema, openApiDocument);
    }
}