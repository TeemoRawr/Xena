using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Parsers.ClientParser.SplitModelStrategies;

public interface ISplitModelStrategy
{
    string GetClientName(string path, OperationType operationType, OpenApiOperation apiOperation);
    string GetMethodName(string path, OperationType operationType, OpenApiOperation apiOperation);
}

public class ModelStrategyOptions
{
    public string ClientPatternName { get; init; } = "{0}Controller";
}