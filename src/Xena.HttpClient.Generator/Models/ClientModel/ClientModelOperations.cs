using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using TypeNameFormatter;
using Xena.HttpClient.Generator.Extensions;
using Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;

namespace Xena.HttpClient.Generator.Models.ClientModel;

public class ClientModelOperations
{
    private readonly ISplitModelStrategy _splitModelStrategy;
    private readonly string _path;
    private readonly OperationType _operationType;
    private readonly OpenApiOperation _apiOperation;

    public ClientModelOperations(
        ISplitModelStrategy splitModelStrategy,
        string path,
        OperationType operationType,
        OpenApiOperation apiOperation)
    {
        _operationType = operationType;
        _apiOperation = apiOperation;
        _splitModelStrategy = splitModelStrategy;
        _path = path;
    }

    public MemberDeclarationSyntax Generate()
    {
        var methodName = _splitModelStrategy.GetMethodName(_path, _operationType, _apiOperation);

        var parameterSyntaxes = _apiOperation.Parameters
            .Select(GenerateParameter)
            .ToList();

        return SyntaxFactory.MethodDeclaration(
            new SyntaxList<AttributeListSyntax>(),
            SyntaxFactory.TokenList(),
            SyntaxFactory.ParseTypeName(typeof(string).Name),
            null,
            SyntaxFactory.Identifier(methodName),
            null,
            SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameterSyntaxes)),
            new SyntaxList<TypeParameterConstraintClauseSyntax>(),
            null,
            SyntaxFactory.Token(SyntaxKind.SemicolonToken)
        );
    }

    private ParameterSyntax GenerateParameter(OpenApiParameter apiParameter)
    {
        var attributesToAdd = new List<AttributeSyntax>();

        if (apiParameter.Required)
        {
            var requiredAttribute = SyntaxFactory.Attribute(
                SyntaxFactory.ParseName(typeof(RequiredAttribute).GetFormattedName())
            );

            attributesToAdd.Add(requiredAttribute);
        }

        var parameterType = TypeResolver.Resolve(
            apiParameter.Schema.Reference,
            apiParameter.Schema.Type,
            apiParameter.Schema.Format,
            apiParameter.Schema.Nullable);

        var parameterSyntax = SyntaxFactory.Parameter(
            SyntaxFactory.Identifier(apiParameter.Name)
        ).WithType(SyntaxFactory.ParseTypeName(parameterType));

        if (attributesToAdd.Any())
        {
            parameterSyntax = parameterSyntax.WithAttributeLists(
                SyntaxFactory.List(new List<AttributeListSyntax>
                {
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SeparatedList(attributesToAdd)
                    )
                })
            );
        }

        return parameterSyntax;
    }
}