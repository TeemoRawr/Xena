using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Xena.HttpClient.Generator.Extensions;

namespace Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;

public class MultipleClientsFromOperationId : ISplitModelStrategy
{
    private const string RegexPattern = @"((?<controller>.+?)_+)?(?<method>.+)";

    private readonly ModelStrategyOptions _modelStrategyOptions;

    public MultipleClientsFromOperationId(ModelStrategyOptions modelStrategyOptions)
    {
        _modelStrategyOptions = modelStrategyOptions;
    }

    public string GetClientName(string path, OperationType operationType, OpenApiOperation apiOperation)
    {
        var regex = new Regex(RegexPattern);

        var controllerName = string.Empty;

        var match = regex.Match(apiOperation.OperationId);

        if (match.Success)
        {
            var controllerGroup = match.Groups["controller"];
            controllerName = controllerGroup.Success && !string.IsNullOrWhiteSpace(controllerGroup.Value)
                ? controllerGroup.Value
                : string.Empty;
        }

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
        
        var methodName = string.Empty;
        var regex = new Regex(RegexPattern);
        var match = regex.Match(apiOperation.OperationId);

        if (match.Success)
        {
            var controllerGroup = match.Groups["method"];
            methodName = controllerGroup.Success && !string.IsNullOrWhiteSpace(controllerGroup.Value)
                ? controllerGroup.Value
                : string.Empty;
        }

        if (string.IsNullOrWhiteSpace(methodName))
        {
            methodName = BuildAlternativeMethodName();
        }

        return methodName.ToPascalCase();
    }
}