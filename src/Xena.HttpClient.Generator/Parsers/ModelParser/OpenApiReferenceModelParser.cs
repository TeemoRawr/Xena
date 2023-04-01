using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.CodeModel;

namespace Xena.HttpClient.Generator.Parsers.ModelParser;

public class OpenApiReferenceModelParser : OpenApiBaseModelParser
{
    public OpenApiReferenceModelParser(OpenApiBaseModelParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, string name, OpenApiParserOptions options)
    {
        return !options.IsRoot && (schema.Type == "object" || !string.IsNullOrWhiteSpace(schema.Reference?.Id));
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument, OpenApiParserOptions options)
    {
        return new ReferenceCodeModel(name, openApiSchema, openApiDocument);
    }
}