﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.CodeModel;

namespace Xena.HttpClient.Generator.Parsers.ModelParser;

public class OpenApiObjectModelParser : OpenApiBaseModelParser
{
    public OpenApiObjectModelParser(OpenApiBaseModelParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema, string name, OpenApiParserOptions options)
    {
        return options.IsRoot && (schema.Type == "object" || (!string.IsNullOrWhiteSpace(schema.Reference?.Id) && schema.Items is null));
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument, OpenApiParserOptions options)
    {
        return new ObjectCodeModel(name, openApiSchema, openApiDocument);
    }
}