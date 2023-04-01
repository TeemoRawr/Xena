using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.CodeModel;

namespace Xena.HttpClient.Generator.Parsers.ModelParser;

public class EmptyApiModelParser : OpenApiBaseModelParser
{
    public EmptyApiModelParser() : base(null)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, string name, OpenApiParserOptions options) => true;

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema, OpenApiDocument openApiDocument, OpenApiParserOptions options)
    {
        return new EmptyCodeModel(name, openApiSchema);
    }
}