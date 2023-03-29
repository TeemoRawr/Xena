using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.CodeModel;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Parsers.ModelParser;

public class ReferenceCodeModel : BaseCodeModel
{
    private readonly OpenApiDocument _openApiDocument;

    public ReferenceCodeModel(string name, OpenApiSchema openApiSchema, OpenApiDocument openApiDocument) : base(name, openApiSchema)
    {
        _openApiDocument = openApiDocument;
    }

    protected override CodeModelGenerationResult GenerateInternal(CodeModelGenerateOptions options)
    {
        string propertyType;
        var extraObjectMembers = new List<MemberDeclarationSyntax>();
        
        if (string.IsNullOrWhiteSpace(Schema.Reference?.Id))
        {
            propertyType = $"{options.Prefix}{NormalizedName}";
            
            var parserOptions = new OpenApiParserOptions
            {
                IsRoot = false
            };
            
            var modelGenerationResults = Schema.Properties
                .Select(p => ParserComposition.ModelParser.Parse(p.Key, p.Value, _openApiDocument, parserOptions))
                .Select(p => p.Generate(new CodeModelGenerateOptions
                {
                    Prefix = NormalizedName
                }))
                .ToList();
            
            var extraObjectMembersFromGeneration = modelGenerationResults
                .SelectMany(p => p.ExtraObjectMembers)
                .ToList();

            var members = modelGenerationResults
                .Select(p => p.Memeber)
                .ToList(); 

            var extraClass = SyntaxFactory.ClassDeclaration(propertyType)
                .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
                .WithMembers(new SyntaxList<MemberDeclarationSyntax>(members));
            
            extraObjectMembers.AddRange(extraObjectMembersFromGeneration);
            extraObjectMembers.Add(extraClass);
        }
        else
        {
            propertyType = Schema.Reference!.Id;
        }

        var model = SF.PropertyDeclaration(SF.ParseTypeName(propertyType), NormalizedName)
            .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
            .AddAccessorListAccessors(
                SF.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)),
                SF.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken))
            );

        return new CodeModelGenerationResult
        {
            Memeber = model,
            ExtraObjectMembers = extraObjectMembers
        };
    }
}