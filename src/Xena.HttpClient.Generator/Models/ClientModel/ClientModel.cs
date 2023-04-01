using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.CodeDom.Compiler;
using Xena.HttpClient.Generator.Extensions;
using Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;

namespace Xena.HttpClient.Generator.Models.ClientModel;

public class ClientModel
{
    private readonly ISplitModelStrategy _splitModelStrategy;
    private readonly IReadOnlyList<ClientModelOperations> _clientModelOperationsList;

    public ClientModel(
        ISplitModelStrategy splitModelStrategy,
        string clientName,
        IReadOnlyList<ClientModelOperations> clientModelOperationsList)
    {
        _splitModelStrategy = splitModelStrategy;
        _clientModelOperationsList = clientModelOperationsList;
        ClientName = clientName;
    }

    public string ClientName { get; }
    public string NormalizedClientName => ClientName.ToPascalCase();

    public MemberDeclarationSyntax Generate()
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

        var methodsDeclarations = _clientModelOperationsList
            .Select(o => o.Generate())
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

        return interfaceDeclaration;
    }
}