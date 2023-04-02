using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.OpenApi.Models;
using RestEase;
using Xena.HttpClient.Generator.Extensions;
using Xena.HttpClient.Generator.Models.CodeModel;
using Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;

namespace Xena.HttpClient.Generator.Models.ClientModel;

public class ClientModelOperations
{
    private readonly ISplitModelStrategy _splitModelStrategy;
    private readonly string _path;
    private readonly OperationType _operationType;
    private readonly OpenApiOperation _apiOperation;
    private readonly IList<OpenApiParameter> _extraApiParameters;

    public ClientModelOperations(ISplitModelStrategy splitModelStrategy,
        string path,
        OperationType operationType,
        OpenApiOperation apiOperation, 
        IList<OpenApiParameter> extraApiParameters)
    {
        _operationType = operationType;
        _apiOperation = apiOperation;
        _extraApiParameters = extraApiParameters;
        _splitModelStrategy = splitModelStrategy;
        _path = path;
    }

    public CodeModelGenerationResult<MemberDeclarationSyntax> Generate()
    {
        var methodName = _splitModelStrategy.GetMethodName(_path, _operationType, _apiOperation);

        var parameterSyntaxList = _apiOperation.Parameters
            .Select(GenerateNonContentParameter)
            .ToList();

        if (_extraApiParameters?.Any() == true)
        {
            var parameterNamesFromMethod = _apiOperation.Parameters
                .Select(p => p.Name)
                .ToList();

            var extraParameterList = _extraApiParameters
                .Where(p => !parameterNamesFromMethod.Contains(p.Name))
                .Select(GenerateNonContentParameter)
                .ToList();

            parameterSyntaxList.AddRange(extraParameterList);
        }

        if (_apiOperation.RequestBody is not null)
        {
            var contentParameter = GenerateContentParameter(_apiOperation.RequestBody);

            if (contentParameter is not null)
            {
                parameterSyntaxList.Add(contentParameter);
            }
        }

        var responseParameterSyntax = GenerateResponse(_apiOperation.Responses);

        var operationTypeAttribute = _operationType switch
        {
            OperationType.Get => typeof(GetAttribute),
            OperationType.Put => typeof(PutAttribute),
            OperationType.Post => typeof(PostAttribute),
            OperationType.Delete => typeof(DeleteAttribute),
            OperationType.Options => typeof(OptionsAttribute),
            OperationType.Head => typeof(HeadAttribute),
            OperationType.Patch => typeof(PatchAttribute),
            OperationType.Trace => typeof(TraceAttribute),
            _ => throw new ArgumentOutOfRangeException(nameof(_operationType))
        };

        var attributeList = SyntaxFactory.List(new List<AttributeListSyntax>
        {
            SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new List<AttributeSyntax>
            {
                SyntaxFactory.Attribute(
                    SyntaxFactory.ParseName(operationTypeAttribute.GetNiceName()),
                    SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(new List<AttributeArgumentSyntax>
                    {
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Literal(_path)
                            )
                        )
                    }))
                )
            }))
        });

        var methodDeclarationSyntax = SyntaxFactory.MethodDeclaration(
            attributeList,
            SyntaxFactory.TokenList(),
            responseParameterSyntax,
            null,
            SyntaxFactory.Identifier(methodName),
            null,
            SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameterSyntaxList)),
            new SyntaxList<TypeParameterConstraintClauseSyntax>(),
            null,
            SyntaxFactory.Token(SyntaxKind.SemicolonToken)
        );

        return new CodeModelGenerationResult<MemberDeclarationSyntax>
        {
            Member = methodDeclarationSyntax
        };
    }

    private ParameterSyntax? GenerateContentParameter(OpenApiRequestBody apiOperationRequestBody)
    {

        var openApiMediaType = apiOperationRequestBody.Content
            .SingleOrDefault(c => c.Key.ToLower().Contains("json"))
            .Value;

        if (openApiMediaType is null)
        {
            return null;
        }

        var attributesToAdd = new List<AttributeSyntax>();

        if (apiOperationRequestBody.Required)
        {
            var requiredAttribute = SyntaxFactory.Attribute(
                SyntaxFactory.ParseName(typeof(RequiredAttribute).GetNiceName())
            );

            attributesToAdd.Add(requiredAttribute);
        }

        var bodyAttribute = SyntaxFactory.Attribute(
            SyntaxFactory.ParseName(typeof(BodyAttribute).GetNiceName())
        );

        attributesToAdd.Add(bodyAttribute);

        var parameterType = TypeResolver.Resolve(openApiMediaType.Schema);

        var parameterSyntax = SyntaxFactory.Parameter(
            SyntaxFactory.Identifier("body")
        )
            .WithType(SyntaxFactory.ParseTypeName(parameterType))
            .WithAttributeLists(
                SyntaxFactory.List(new List<AttributeListSyntax>
                {
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SeparatedList(attributesToAdd)
                    )
                })    
            );

        return parameterSyntax;
    }

    private TypeSyntax GenerateResponse(OpenApiResponses apiOperationResponses)
    {
        var responseType = typeof(Task).GetNiceName();

        var positiveResponseRange = Enumerable
            .Range(200, 199)
            .Select(p => p.ToString())
            .ToList();

        OpenApiResponse operationResponse;

        if (apiOperationResponses.Count > 1)
        {
            operationResponse = apiOperationResponses
                .FirstOrDefault(p => positiveResponseRange.Contains(p.Key))
                .Value;

            operationResponse ??= apiOperationResponses
                .SingleOrDefault(p => p.Key == "default")
                .Value;
        }
        else
        {
            operationResponse = apiOperationResponses.SingleOrDefault().Value;
        }

        OpenApiMediaType mediaType;

        if (operationResponse.Content.Count > 1)
        {
            mediaType = operationResponse.Content
                .SingleOrDefault(p => p.Key.ToLower().Contains("json"))
                .Value;
        }
        else
        {
            mediaType = operationResponse.Content.SingleOrDefault().Value;
        }

        if (mediaType?.Schema is not null)
        {
            responseType = TypeResolver.Resolve(mediaType.Schema);
            responseType = $"Task<{responseType}>";
        }

        return SyntaxFactory.ParseTypeName(responseType);
    }

    private ParameterSyntax GenerateNonContentParameter(OpenApiParameter apiParameter)
    {
        var attributesToAdd = new List<AttributeSyntax>();

        if (apiParameter.Required)
        {
            var requiredAttribute = SyntaxFactory.Attribute(
                SyntaxFactory.ParseName(typeof(RequiredAttribute).GetNiceName())
            );

            attributesToAdd.Add(requiredAttribute);
        }

        if (apiParameter.In is not null)
        {
            var apiParameterInType = apiParameter.In switch
            {
                ParameterLocation.Query => typeof(QueryAttribute),
                ParameterLocation.Header => typeof(HeadAttribute),
                ParameterLocation.Path => typeof(PathAttribute),
                _ => null
            };

            if (apiParameterInType is not null)
            {
                var inAttribute = SyntaxFactory.Attribute(
                    SyntaxFactory.ParseName(apiParameterInType.GetNiceName()),
                    SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(new List<AttributeArgumentSyntax>
                    {
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Literal(apiParameter.Name)
                            )
                        )
                    }))
                );

                attributesToAdd.Add(inAttribute);
            }
        }

        var parameterType = TypeResolver.Resolve(apiParameter.Schema);

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