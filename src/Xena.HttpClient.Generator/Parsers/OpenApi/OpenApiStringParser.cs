using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public class OpenApiStringParser : OpenApiBaseParser
{
    public OpenApiStringParser(OpenApiBaseParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, OpenApiParserOptions options)
    {
        return !options.IsRoot && schema.Type == "string";
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument, OpenApiParserOptions options)
    {
        return openApiSchema switch
        {
            { Format: "date" or "date-time" } => new DateTimeCodeModel(name, openApiSchema),
            { Enum.Count: > 0 } => new EnumCodeModel(name, openApiSchema),
            _ => new StringCodeModel(name, openApiSchema)
        };
    }
}