using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Extensions;

namespace Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;

public class MultipleClientFromFirstTag : ISplitModelStrategy
{
    private readonly ModelStrategyOptions _modelStrategyOptions;

    public MultipleClientFromFirstTag(ModelStrategyOptions modelStrategyOptions)
    {
        _modelStrategyOptions = modelStrategyOptions;
    }

    public string GetClientName(string path, OperationType operationType, OpenApiOperation apiOperation)
    {
        var controllerName = apiOperation.Tags.FirstOrDefault()?.Name ?? string.Empty;
        
        return string.Format(_modelStrategyOptions.ClientPatternName, controllerName.ToPascalCase());
    }

    public string GetMethodName(string path, OperationType operationType, OpenApiOperation apiOperation)
    {
        string BuildAlternativeMethodName()
        {
            var methodNameFromPath = string.Concat(path.Split("/")
                .Select(p => p.ToPascalCase()));

            return $"{operationType.ToString().ToPascalCase()}{methodNameFromPath}";
        }

        var apiOperationOperationId = apiOperation.OperationId;

        if (string.IsNullOrWhiteSpace(apiOperationOperationId))
        {
            return BuildAlternativeMethodName();
        }

        return apiOperationOperationId.ToPascalCase();
    }
}