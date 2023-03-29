using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Parsers.ModelParser;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Parsers;

public static class ParserComposition
{
    public static readonly OpenApiBaseModelParser ModelParser = new OpenApiArrayModelParser(new OpenApiReferenceModelParser(new OpenApiObjectModelParser(new OpenApiBooleanModelParser(new OpenApiNumberModelParser(new OpenApiIntegerModelParser(new OpenApiStringModelParser(null)))))));
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
            .Select(p => ParserComposition.ModelParser.Parse(p.Key, p.Value, document, parserOptions))
            .Select(p => p.Generate())
            .ToList();

        var extraMembers = generationResults.SelectMany(p => p.ExtraObjectMembers);
        var members = generationResults.Select(p => p.Memeber);

        var codeNamespace = SF.NamespaceDeclaration(SF.ParseName("Test"))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(members.Concat(extraMembers).ToList()))
            .AddUsings(
                SF.UsingDirective(SF.ParseName("System.Collections.Generic")),
                SF.UsingDirective(SF.ParseName("System.ComponentModel.DataAnnotations"))
            );

        var code = new StringWriter();
        
        codeNamespace.NormalizeWhitespace(elasticTrivia: true).WriteTo(code);

        return code.ToString();
    }
}