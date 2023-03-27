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

    protected override MemberDeclarationSyntax GenerateInternal()
    {
        var classProperties = Schema.Properties
            .Select(p => ParserComposition.Parser.Parse(p.Key, p.Value, _openApiDocument))
            .Select(p => p.Generate())
            .ToList();

        var classClient = SF.ClassDeclaration(SF.Identifier(NormalizedName))
            .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(classProperties));

        return classClient;
    }
}