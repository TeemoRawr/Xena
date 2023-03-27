using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.OpenApi.Models;

using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public static class ParserComposition
{
    public static readonly OpenApiBaseParser Parser = new OpenApiArrayParser(new OpenApiReferenceParser(new OpenApiObjectParser(new OpenApiNumberParser(new OpenApiIntegerParser(new OpenApiStringParser(null))))));
}

public class OpenApiParser
{
    public string Generate(OpenApiDocument document)
    {
        var codeModels = document.Components.Schemas
            .Select(p => ParserComposition.Parser.Parse(p.Key, p.Value, document))
            .Select(p => p.Generate())
            .ToList();

        var codeNamespace = SF.NamespaceDeclaration(SF.ParseName("Test"))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(codeModels))
            .AddUsings(
                SF.UsingDirective(SF.ParseName("System.Collections.Generic"))
            );

        var code = new StringWriter();

        codeNamespace.NormalizeWhitespace().WriteTo(code);

        return code.ToString();
    }
}