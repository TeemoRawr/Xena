using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Xena.HttpClient.Generator.Models;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Xena.HttpClient.Generator.Services.CodeGenerator;

public class ApiClientGeneratorService
{
    public List<ApiClientModel> Build(OpenApiDocument apiDocument, OpenApiDiagnostic apiDiagnostic)
    {
        var operationIds = apiDocument.Paths
            .SelectMany(p => p.Value.Operations.Values)
            .Select(p => p.OperationId)
            .ToList();

        var a = new List<object>();
        
        foreach (var operationId in operationIds)
        {
            var operations = apiDocument.Paths
                .SelectMany(p => p.Value.Operations.Select(o => new
                {
                    MethodType = o.Key,
                    Operation = o.Value
                }))
                .Where(p => p.Operation.OperationId == operationId)
                .ToList();

            // var methods = operations.Select(o =>
            // {
            //     var a = SF.MethodDeclaration(
            //         SF.ParseTypeName()
            //     )
            // });
            
            var apiClientCode = SF.InterfaceDeclaration(SF.Identifier(operationId));
        }
        
        
        return new List<ApiClientModel>();
    }
}