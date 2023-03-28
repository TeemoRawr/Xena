using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models;

public abstract class BasicTypeCodeModel<TType> : BaseCodeModel
{
    protected BasicTypeCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }

    protected override CodeModelGenerationResult GenerateInternal(CodeModelGenerateOptions options)
    {
        var propertyType = typeof(TType).FullName;

        var model = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(propertyType), NormalizedName)
            .WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .AddAccessorListAccessors(
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
            );

        return new CodeModelGenerationResult
        {
            Memeber = model
        };
    }
}