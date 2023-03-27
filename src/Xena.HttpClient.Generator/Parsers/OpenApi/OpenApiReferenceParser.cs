using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public class OpenApiReferenceParser : OpenApiBaseParser
{
    public OpenApiReferenceParser(OpenApiBaseParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema)
    {
        return schema.Type == "object" && !string.IsNullOrWhiteSpace(schema.Reference.Id);
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema, OpenApiDocument openApiDocument)
    {
        return new ReferenceCodeModel(name, openApiSchema, openApiDocument);
    }
}