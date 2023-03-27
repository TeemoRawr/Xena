using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public abstract class OpenApiBaseParser
{
    private readonly OpenApiBaseParser? _nextParser;

    protected OpenApiBaseParser(OpenApiBaseParser? nextParser)
    {
        _nextParser = nextParser;
    }

    protected abstract bool CanParse(OpenApiSchema schema);
    protected abstract BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema, OpenApiDocument openApiDocument);

    public BaseCodeModel Parse(string name, OpenApiSchema openApiSchema, OpenApiDocument document)
    {
        if (!CanParse(openApiSchema))
        {
            if (_nextParser is null)
            {
                throw new Exception($"Unsupported type to parse {openApiSchema.Type}");
            }

            return _nextParser.Parse(name, openApiSchema, document);
        }

        return InternalParse(name, openApiSchema, document);
    }
}

public class OpenApiParserOptions
{
}
