using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Xena.HttpClient.Generator.Models;

namespace Xena.HttpClient.Generator.Services.ModelGenerator;

public class ApiClientGeneratorService
{
    public List<ApiClientModel> Build(OpenApiDocument apiDocument, OpenApiDiagnostic apiDiagnostic)
    {
        var operationIds = apiDocument.Paths
            .SelectMany(p => p.Value.Operations.Values)
            .Select(p => p.OperationId)
            .ToList();

        var apiClients = new List<ApiClientModel>();

        foreach (var operationId in operationIds)
        {
            var operations = apiDocument.Paths
                .SelectMany(p => p.Value.Operations.Select(o => new
                {
                    Path = p.Key,
                    MethodType = o.Key,
                    Operation = o.Value
                }))
                .Where(p => p.Operation.OperationId == operationId)
                .ToList();

            var apiClientModel = new ApiClientModel
            {
                Name = operationId
            };

            var clientModelsOperations = operations.Select(operation =>
            {
                return new ApiClientOperationModel
                {
                    Path = operation.Path,
                    MethodType = operation.MethodType,
                };
            })
            .ToList();
            
            apiClientModel.Operations = clientModelsOperations;
            apiClients.Add(apiClientModel);
        }


        return apiClients;
    }
}