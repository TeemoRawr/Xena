using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public class OpenApiArrayParser : OpenApiBaseParser
{
    public OpenApiArrayParser(OpenApiBaseParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema)
    {
        return schema.Type == "array";
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema, OpenApiDocument openApiDocument)
    {
        return new ArrayCodeModel(name, openApiSchema);
    }
}