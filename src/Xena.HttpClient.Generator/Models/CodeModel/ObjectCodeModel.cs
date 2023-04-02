using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using TypeNameFormatter;
using Xena.HttpClient.Generator.Extensions;
using Xena.HttpClient.Generator.Parsers;
using Xena.HttpClient.Generator.Parsers.ModelParser;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Models.CodeModel;

public class ObjectCodeModel : BaseCodeModel
{
    private readonly OpenApiDocument _openApiDocument;

    public ObjectCodeModel(string name, OpenApiSchema openApiSchema, OpenApiDocument openApiDocument) : base(name, openApiSchema)
    {
        _openApiDocument = openApiDocument;
    }

    protected override CodeModelGenerationResult<MemberDeclarationSyntax> GenerateInternal(CodeModelGenerateOptions options)
    {
        var parserOptions = new OpenApiParserOptions
        {
            IsRoot = false
        };
        
        var modelGenerationResults = Schema.Properties
            .Select(p => new 
            {
                CodeModel = ParserComposition.ModelParser.Parse(p.Key, p.Value, _openApiDocument, parserOptions),
                Name = p.Key
            })
            .Select(p => p.CodeModel.Generate(new CodeModelGenerateOptions
            {
                Prefix = NormalizedName,
                IsRequired = Schema.Required.Contains(p.Name) 
            }))
            .ToList();

        var extraObjectMembers = modelGenerationResults
            .SelectMany(p => p.ExtraObjectMembers)
            .ToList();

        var members = modelGenerationResults
            .Where(p => p.Member is not null)
            .Select(p => p.Member!)
            .ToList();

        var generatedCodeAttributeSyntax = SF.Attribute(
            SF.ParseName(typeof(GeneratedCodeAttribute).GetNiceName())
        ).WithArgumentList(
            SF.AttributeArgumentList(
                SF.SeparatedList(new List<AttributeArgumentSyntax>
                {
                    SF.AttributeArgument(
                        SF.LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            SF.Literal("test")
                        )
                    ),
                    SF.AttributeArgument(
                        SF.LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            SF.Literal("test")
                        )
                    )
                })    
            )
        );

        var modelDeclaration = SF.ClassDeclaration(SF.Identifier(NormalizedName))
            .WithModifiers(new SyntaxTokenList(SF.Token(SyntaxKind.PublicKeyword)))
            .WithAttributeLists(SF.List(new List<AttributeListSyntax>
            {
                SF.AttributeList(SF.SeparatedList(new List<AttributeSyntax>
                {
                    generatedCodeAttributeSyntax
                }))
            }))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(members));

        return new CodeModelGenerationResult<MemberDeclarationSyntax>
        {
            Member = modelDeclaration,
            ExtraObjectMembers = extraObjectMembers
        };
    }
}