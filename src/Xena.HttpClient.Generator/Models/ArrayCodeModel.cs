using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Models;

public class ArrayCodeModel : BaseCodeModel
{
    public ArrayCodeModel(string name, OpenApiSchema schema) : base(name, schema)
    {
    }

    protected override MemberDeclarationSyntax GenerateInternal()
    {
        var internalPropertyType = ResolveType(Schema.Items);

        var propertyType = $"IReadOnlyList<{internalPropertyType}>";

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

    private string ResolveType(OpenApiSchema itemsSchema)
    {
        if (!string.IsNullOrWhiteSpace(itemsSchema.Reference?.Id))
        {
            return itemsSchema.Reference.Id;
        }

        return itemsSchema.Type switch
        {
            "text" when itemsSchema.Format == "date" => "DateTime",
            "text" when itemsSchema.Format == "date-time" => "DateTime",
            "text" => "string",
            "number" when itemsSchema.Format == "double" => "double",
            "number" when itemsSchema.Format == "float" => "double",
            "number" => "int",
            "integer" when itemsSchema.Format == "int64" => "long",
            "integer" => "int",
            _ => "object"
        };
    }
}