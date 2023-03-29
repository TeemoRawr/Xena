using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Extensions;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public class EnumCodeModel : BaseCodeModel
{
    public EnumCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }

    protected override CodeModelGenerationResult GenerateInternal(CodeModelGenerateOptions options)
    {
        var enumType = $"{options.Prefix}{NormalizedName}";

        var enumMembers = Schema.Enum
            .Where(o => o is OpenApiString)
            .Cast<OpenApiString>()
            .Select(p => p.Value.ToPascalCase())
            .Select(SyntaxFactory.EnumMemberDeclaration)
            .ToList();

        var enumObjectMember = SyntaxFactory
            .EnumDeclaration(enumType)
            .WithModifiers(new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithMembers(SyntaxFactory.SeparatedList(enumMembers));

        var member = SyntaxFactory
            .PropertyDeclaration(
                SyntaxFactory.ParseTypeName(enumType),
                NormalizedName)
            .WithModifiers(new SyntaxTokenList { SyntaxFactory.Token(SyntaxKind.PublicKeyword) })
            .AddAccessorListAccessors(
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
            );
        
        return new CodeModelGenerationResult
        {
            Member = member,
            ExtraObjectMembers = new List<MemberDeclarationSyntax> { enumObjectMember }
        };
    }
}