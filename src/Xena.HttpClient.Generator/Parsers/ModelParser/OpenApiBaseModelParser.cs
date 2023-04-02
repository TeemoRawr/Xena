using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.CodeModel;

namespace Xena.HttpClient.Generator.Parsers.ModelParser;

public abstract class OpenApiBaseModelParser
{
    private readonly OpenApiBaseModelParser? _nextParser;

    protected OpenApiBaseModelParser(OpenApiBaseModelParser? nextParser)
    {
        _nextParser = nextParser;
    }

    protected abstract bool CanParse(OpenApiSchema schema, string name, OpenApiParserOptions options);
    
    protected abstract BaseCodeModel InternalParse(
        string name, 
        OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument,
        OpenApiParserOptions options);

    public BaseCodeModel Parse(
        string name,
        OpenApiSchema openApiSchema,
        OpenApiDocument document,
        OpenApiParserOptions options)
    {
        if (options is null)
        {
            throw new NullReferenceException("Options cannot be null");
        }
        
        if (!CanParse(openApiSchema, name, options))
        {
            if (_nextParser is null)
            {
                throw new Exception($"Unsupported type to parse {openApiSchema.Type}");
            }

            return _nextParser.Parse(name, openApiSchema, document, options);
        }

        return InternalParse(name, openApiSchema, document, options);
    }
}

public class OpenApiParserOptions
{
    public bool IsRoot { get; set; }
}
