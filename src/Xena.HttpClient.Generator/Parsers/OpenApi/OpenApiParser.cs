using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.OpenApi.Models;

using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public static class ParserComposition
{
    public static readonly OpenApiBaseParser Parser = new OpenApiArrayParser(new OpenApiReferenceParser(new OpenApiObjectParser(new OpenApiBooleanParser(new OpenApiNumberParser(new OpenApiIntegerParser(new OpenApiStringParser(null)))))));
}

public class OpenApiParser
{
    public string Generate(OpenApiDocument document)
    {
        var parserOptions = new OpenApiParserOptions
        {
            IsRoot = true
        };
        
        var generationResults = document.Components.Schemas
            .Select(p => ParserComposition.Parser.Parse(p.Key, p.Value, document, parserOptions))
            .Select(p => p.Generate())
            .ToList();

        var extraMembers = generationResults.SelectMany(p => p.ExtraObjectMembers);
        var members = generationResults.Select(p => p.Memeber);

        var codeNamespace = SF.NamespaceDeclaration(SF.ParseName("Test"))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(members.Concat(extraMembers).ToList()))
            .AddUsings(
                SF.UsingDirective(SF.ParseName("System.Collections.Generic"))
            );

        var code = new StringWriter();
        
        codeNamespace.NormalizeWhitespace(elasticTrivia: true).WriteTo(code);

        return code.ToString();
    }
}