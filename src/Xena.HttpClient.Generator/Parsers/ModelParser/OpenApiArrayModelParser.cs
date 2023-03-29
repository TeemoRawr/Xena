using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.CodeModel;

namespace Xena.HttpClient.Generator.Parsers.ModelParser;

public class OpenApiArrayModelParser : OpenApiBaseModelParser
{
    public OpenApiArrayModelParser(OpenApiBaseModelParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, OpenApiParserOptions options)
    {
        return !options.IsRoot && schema.Type == "array";
    }

    protected override BaseCodeModel InternalParse(
        string name,
        OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument,
        OpenApiParserOptions options)
    {
        return new ArrayCodeModel(name, openApiSchema);
    }
}