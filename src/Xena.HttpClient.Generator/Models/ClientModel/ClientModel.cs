using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CodeDom.Compiler;
using Xena.HttpClient.Generator.Extensions;
using Xena.HttpClient.Generator.Models.CodeModel;
using Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;

namespace Xena.HttpClient.Generator.Models.ClientModel;

public class ClientModel
{
    private readonly IReadOnlyList<ClientModelOperations> _clientModelOperationsList;

    public ClientModel(
        string clientName,
        IReadOnlyList<ClientModelOperations> clientModelOperationsList)
    {
        _clientModelOperationsList = clientModelOperationsList;
        ClientName = clientName;
    }

    public string ClientName { get; }
    public string NormalizedClientName => ClientName.ToPascalCase();

    public CodeModelGenerationResult<MemberDeclarationSyntax> Generate()
    {
        var generatedCodeAttributeSyntax = SyntaxFactory.Attribute(
            SyntaxFactory.ParseName(typeof(GeneratedCodeAttribute).GetNiceName())
        ).WithArgumentList(
            SyntaxFactory.AttributeArgumentList(
                SyntaxFactory.SeparatedList(new List<AttributeArgumentSyntax>
                {
                    SyntaxFactory.AttributeArgument(
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            SyntaxFactory.Literal("test")
                        )
                    ),
                    SyntaxFactory.AttributeArgument(
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            SyntaxFactory.Literal("test")
                        )
                    )
                })
            )
        );

        var methodsDeclarationsResults = _clientModelOperationsList
            .Select(o => o.Generate())
            .ToList();

        var extraObjectMembers = methodsDeclarationsResults
            .SelectMany(p => p.ExtraObjectMembers)
            .ToList();

        var methodsDeclarations = methodsDeclarationsResults
            .Select(p => p.Member)
            .ToList();

        var interfaceDeclaration = SyntaxFactory.InterfaceDeclaration(NormalizedClientName)
            .WithAttributeLists(SyntaxFactory.List(new List<AttributeListSyntax>
            {
                SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new List<AttributeSyntax>
                {
                    generatedCodeAttributeSyntax
                }))
            }))
            .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithMembers(SyntaxFactory.List(methodsDeclarations));

        return new CodeModelGenerationResult<MemberDeclarationSyntax>
        {
            Member = interfaceDeclaration,
            ExtraObjectMembers = extraObjectMembers
        };
    }
}