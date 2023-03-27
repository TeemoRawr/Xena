using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Parsers.OpenApi;

public class ReferenceCodeModel : BaseCodeModel
{

    public ReferenceCodeModel(string name, OpenApiSchema openApiSchema, OpenApiDocument openApiDocument) : base(name, openApiSchema)
    {
    }

    protected override MemberDeclarationSyntax GenerateInternal()
    {
        var propertyType = Schema.Reference.Id;

        var model = SF.PropertyDeclaration(SF.ParseTypeName(propertyType), NormalizedName)
            .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
            .AddAccessorListAccessors(
                SF.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)),
                SF.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken))
            );

        return model;
    }
}