using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        var methodsDeclarations = _clientModelOperationsList
            .Select(o => o.Generate())
            .ToList();

        var interfaceDeclaration = SyntaxFactory.InterfaceDeclaration(NormalizedClientName)
            .WithMembers(SyntaxFactory.List(methodsDeclarations));

        return interfaceDeclaration;
    }
}