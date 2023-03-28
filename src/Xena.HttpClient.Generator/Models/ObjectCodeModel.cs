using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Parsers.OpenApi;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Models;

public class ObjectCodeModel : BaseCodeModel
{
    private readonly OpenApiDocument _openApiDocument;

    public ObjectCodeModel(string name, OpenApiSchema openApiSchema, OpenApiDocument openApiDocument) : base(name, openApiSchema)
    {
        _openApiDocument = openApiDocument;
    }

    protected override CodeModelGenerationResult GenerateInternal(CodeModelGenerateOptions options)
    {
        var parserOptions = new OpenApiParserOptions
        {
            IsRoot = false
        };
        
        var modelGenerationResults = Schema.Properties
            .Select(p => ParserComposition.Parser.Parse(p.Key, p.Value, _openApiDocument, parserOptions))
            .Select(p => p.Generate(new CodeModelGenerateOptions
            {
                Prefix = NormalizedName
            }))
            .ToList();

        var extraObjectMembers = modelGenerationResults
            .SelectMany(p => p.ExtraObjectMembers)
            .ToList();

        var members = modelGenerationResults
            .Select(p => p.Memeber)
            .ToList(); 

        var classClient = SF.ClassDeclaration(SF.Identifier(NormalizedName))
            .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(members));

        return new CodeModelGenerationResult
        {
            Memeber = classClient,
            ExtraObjectMembers = extraObjectMembers
        };
    }
}