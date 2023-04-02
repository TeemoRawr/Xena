using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Extensions;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public class ArrayCodeModel : BaseCodeModel
{
    public ArrayCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }

    protected override CodeModelGenerationResult<MemberDeclarationSyntax> GenerateInternal(CodeModelGenerateOptions options)
    {
        var internalPropertyType = TypeResolver.Resolve(Schema.Items);

        var propertyType = $"IReadOnlyList<{internalPropertyType}>";

        var model = SF.PropertyDeclaration(SF.ParseTypeName(propertyType), NormalizedName)
            .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
            .AddAccessorListAccessors(
                SF.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)),
                SF.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken))
            );
        
        return new CodeModelGenerationResult<MemberDeclarationSyntax>
        {
            Member = model
        };
    }
}