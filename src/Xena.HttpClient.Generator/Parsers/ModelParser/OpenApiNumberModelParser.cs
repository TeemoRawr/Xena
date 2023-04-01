using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.CodeModel;

namespace Xena.HttpClient.Generator.Parsers.ModelParser;

public class OpenApiNumberModelParser : OpenApiBaseModelParser
{
    public OpenApiNumberModelParser(OpenApiBaseModelParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, string name, OpenApiParserOptions options)
    {
        return !options.IsRoot &&  schema.Type == "number";
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument, OpenApiParserOptions options)
    {
        return openApiSchema.Format switch
        {
            "double" => new DoubleCodeModel(name, openApiSchema),
            "float" => new FloatCodeModel(name, openApiSchema),
            _ => new IntCodeModel(name, openApiSchema)
        };
    }
}