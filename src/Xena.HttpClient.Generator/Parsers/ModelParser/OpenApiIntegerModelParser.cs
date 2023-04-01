using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.CodeModel;

namespace Xena.HttpClient.Generator.Parsers.ModelParser;

public class OpenApiIntegerModelParser : OpenApiBaseModelParser
{
    public OpenApiIntegerModelParser(OpenApiBaseModelParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, string name, OpenApiParserOptions options)
    {
        return !options.IsRoot && schema.Type == "integer";
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument, OpenApiParserOptions options)
    {
        return openApiSchema.Format switch
        {
            "int64" => new LongCodeModel(name, openApiSchema),
            _ => new IntCodeModel(name, openApiSchema)
        };
    }
}