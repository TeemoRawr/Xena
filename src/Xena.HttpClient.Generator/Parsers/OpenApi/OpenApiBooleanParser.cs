using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public class OpenApiBooleanParser : OpenApiBaseParser
{
    public OpenApiBooleanParser(OpenApiBaseParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, OpenApiParserOptions options)
    {
        return !options.IsRoot && schema.Type == "boolean";
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument, OpenApiParserOptions options)
    {
        return new BoolCodeModel(name, openApiSchema);
    }
}