﻿using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public class OpenApiStringParser : OpenApiBaseParser
{
    public OpenApiStringParser(OpenApiBaseParser? nextParser) : base(nextParser)
    {
    }

    protected override bool CanParse(OpenApiSchema schema)
    {
        return schema.Type == "string";
    }

    protected override BaseCodeModel InternalParse(string name, OpenApiSchema openApiSchema,
        OpenApiDocument openApiDocument)
    {
        return openApiSchema.Format switch
        {
            "date" or "date-time" => new DateTimeCodeModel(name, openApiSchema),
            _ => new StringCodeModel(name, openApiSchema)
        };
    }
}