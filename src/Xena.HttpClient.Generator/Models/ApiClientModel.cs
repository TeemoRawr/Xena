using Microsoft.OpenApi.Models;

namespace Xena.HttpClient.Generator.Models;

public class ApiClientModel
{
    public string Name { get; set; }
    public List<ApiClientOperationModel> Operations { get; set; }
}

public class ApiClientOperationModel
{
    public string Path { get; set; }
    public OperationType MethodType { get; set; }
}