using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Models.ClientModel;
using Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;

namespace Xena.HttpClient.Generator.Parsers.ClientParser;

public class OpenApiClientParser
{
    private readonly ISplitModelStrategy _splitModelStrategy;

    public OpenApiClientParser(ISplitModelStrategy splitModelStrategy)
    {
        _splitModelStrategy = splitModelStrategy;
    }

    public IReadOnlyList<ClientModel> Parse(OpenApiPaths paths)
    {
        var operations = paths
            .Select(p => new
            {
                Path = p.Key,
                PathItem = p.Value
            })
            .SelectMany(p => p.PathItem.Operations.Select(o => new
            {
                p.Path,
                OperationType = o.Key,
                ApiOperation = o.Value
            }))
            .ToList();

        var splittedClients = operations.Select(p => new
        {
            Operation = p,
            ClientName = _splitModelStrategy.GetClientName(
                p.Path,
                p.OperationType,
                p.ApiOperation
            )
        })
            .ToLookup(p => p.ClientName, p => p.Operation)
            .Select(p =>
            {
                var clientModelOperationsList = p.Select(o => new ClientModelOperations(
                    _splitModelStrategy,
                    o.Path,
                    o.OperationType,
                    o.ApiOperation,
                    o.ApiOperation.Parameters
                )).ToList();
                

                return new ClientModel(p.Key, clientModelOperationsList);
            })
            .ToList();
        
        return splittedClients;
    }
}