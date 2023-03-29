using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Parsers.ClientParser;
using Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;
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

        var extraMembers = generationResults
            .SelectMany(p => p.ExtraObjectMembers)
            .ToList();
        
        var modelMembers = generationResults
            .Select(p => p.Member)
            .ToList();

        var openApiClientParser = new OpenApiClientParser(new MultipleClientFromFirstTag(new ModelStrategyOptions
        {
            ClientPatternName = "I{0}ApiService"
        }));
        
        var clientMembers = openApiClientParser.Parse(document.Paths)
            .Select(p => p.Generate())
            .ToList();

        var memberList = new List<MemberDeclarationSyntax>();
        memberList.AddRange(clientMembers);
        memberList.AddRange(modelMembers);
        memberList.AddRange(extraMembers);
        
        var codeNamespace = SF.NamespaceDeclaration(SF.ParseName("Test"))
            .WithMembers(SF.List(memberList))
            .AddUsings(
                SF.UsingDirective(SF.ParseName("System.Collections.Generic")),
                SF.UsingDirective(SF.ParseName("System.ComponentModel.DataAnnotations"))
            );

        var code = new StringWriter();

        var workSpace = new AdhocWorkspace();
        
        workSpace.AddSolution(
            SolutionInfo.Create(SolutionId.CreateNewId("formatter"), 
                VersionStamp.Default)
        );
        
        Formatter.Format(codeNamespace.NormalizeWhitespace(elasticTrivia: true), workSpace).WriteTo(code);
        
        return code.ToString();
    }
}